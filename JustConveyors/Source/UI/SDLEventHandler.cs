using System.Numerics;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;
using static SDL2.SDL;

namespace JustConveyors.Source.UI;

internal class SDLEventHandler
{
    private readonly Display _display;
    private readonly DrawableManager _drawableManager;
    private Vector2 _clickDownFocus;
    private Vector2 _clickDownInitFocus;

    private bool _isLimitDrawingToHorizontal;
    private bool _isLimitDrawingToVertical;

    private bool _isMouseLeftDragging;
    private bool _isMouseRightDragging;
    private bool _isStopDrawingGuides;
    private bool _isWaitingMouseMiddleUp;
    private (int X, int Y) _oldTileScreenSpaceMouseAlways;

    private (int X, int Y) _oldTileScreenSpaceMouseLeft;
    private (int X, int Y) _oldTileScreenSpaceMouseRight;
    private int _permX;
    private int _permY;

    public SDLEventHandler(Display display, DrawableManager drawableManager)
    {
        _display = display;
        _drawableManager = drawableManager;
        GUI.OnActiveSurfaceChanged += OnActiveSurfaceChanged;
        GUI.OnActiveSurfaceTransformChanged += OnActiveSurfaceTransformChanged;
        OnActiveSurfaceChanged(PoolResources.BridgeConveyorPool);
    }

    public static Drawable ActiveBlock { get; private set; }

    private void OnActiveSurfaceTransformChanged(TransformFlags newFlag) => ActiveBlock.SetSurface(newFlag);

    private void OnActiveSurfaceChanged(TexturePool newSurface)
    {
        ActiveBlock?.ParentPool?.Dispose();
        ActiveBlock?.Close();
        ActiveBlock = Drawable.Instantiate(_drawableManager, Configuration.ControlsWidth, Configuration.WindowSizeY,
            new TexturePool(newSurface), 0, 5,
            TransformFlags.IndexZero);
        ActiveBlock.SetAlpha(100, 0, ActiveBlock.Surfaces.Count);
    }

    public bool PollEvents()
    {
        while (SDL_PollEvent(out SDL_Event e) != 0)
        {
            _display.Renderer.ProcessEvent(e);
            Coordinates.UpdatePointer();
            ActiveBlock.Transform = ActiveBlock.Transform with
            {
                x = Coordinates.PointingToTileScreenSpace.X, y = Coordinates.PointingToTileScreenSpace.Y
            };

            if (_isWaitingMouseMiddleUp)
            {
                Camera2D.ChangeFocusScrRaw(_clickDownInitFocus + (Coordinates.PointingToRaw - _clickDownFocus));
            }

            if (_oldTileScreenSpaceMouseAlways != Coordinates.PointingToTileScreenSpace)
            {
                if (_isLimitDrawingToVertical && _isLimitDrawingToHorizontal)
                {
                    if (_oldTileScreenSpaceMouseAlways.X != Coordinates.PointingToTileScreenSpace.X)
                    {
                        _isLimitDrawingToVertical = false;
                        _permY = Coordinates.PointingToTileScreenSpace.Y;
                        ScriptHelper.DestroyGuideLines(_drawableManager, (_oldTileScreenSpaceMouseAlways.X, 0),
                            (_oldTileScreenSpaceMouseAlways.X, Configuration.WindowSizeY), TransformFlags.Vertical);
                        ScriptHelper.DestroyGuideLines(_drawableManager,
                            (Configuration.ControlsWidth, _oldTileScreenSpaceMouseAlways.Y),
                            (Configuration.WindowSizeX, _oldTileScreenSpaceMouseAlways.Y), TransformFlags.Horizontal);
                        ScriptHelper.DrawGuideLines(_drawableManager,
                            (Configuration.ControlsWidth, Coordinates.PointingToTileScreenSpace.Y),
                            (Configuration.WindowSizeX, Coordinates.PointingToTileScreenSpace.Y),
                            TransformFlags.Horizontal);
                    }
                    else if (_oldTileScreenSpaceMouseAlways.Y != Coordinates.PointingToTileScreenSpace.Y)
                    {
                        _isLimitDrawingToHorizontal = false;
                        _permX = Coordinates.PointingToTileScreenSpace.X;
                        ScriptHelper.DestroyGuideLines(_drawableManager,
                            (Configuration.ControlsWidth, _oldTileScreenSpaceMouseAlways.Y),
                            (Configuration.WindowSizeX, _oldTileScreenSpaceMouseAlways.Y),
                            TransformFlags.Horizontal);
                        ScriptHelper.DestroyGuideLines(_drawableManager, (_oldTileScreenSpaceMouseAlways.X, 0),
                            (_oldTileScreenSpaceMouseAlways.X, Configuration.WindowSizeY), TransformFlags.Vertical);
                        ScriptHelper.DrawGuideLines(_drawableManager, (Coordinates.PointingToTileScreenSpace.X, 0),
                            (Coordinates.PointingToTileScreenSpace.X, Configuration.WindowSizeY),
                            TransformFlags.Vertical);
                    }

                    _isStopDrawingGuides = true;
                }

                if (!_isStopDrawingGuides)
                {
                    if (_isLimitDrawingToVertical)
                    {
                        ScriptHelper.DestroyGuideLines(_drawableManager, (_oldTileScreenSpaceMouseAlways.X, 0),
                            (_oldTileScreenSpaceMouseAlways.X, Configuration.WindowSizeY), TransformFlags.Vertical);
                        ScriptHelper.DrawGuideLines(_drawableManager, (Coordinates.PointingToTileScreenSpace.X, 0),
                            (Coordinates.PointingToTileScreenSpace.X, Configuration.WindowSizeY),
                            TransformFlags.Vertical);
                    }

                    if (_isLimitDrawingToHorizontal)
                    {
                        ScriptHelper.DestroyGuideLines(_drawableManager,
                            (Configuration.ControlsWidth, _oldTileScreenSpaceMouseAlways.Y),
                            (Configuration.WindowSizeX, _oldTileScreenSpaceMouseAlways.Y), TransformFlags.Horizontal);
                        ScriptHelper.DrawGuideLines(_drawableManager,
                            (Configuration.ControlsWidth, Coordinates.PointingToTileScreenSpace.Y),
                            (Configuration.WindowSizeX, Coordinates.PointingToTileScreenSpace.Y),
                            TransformFlags.Horizontal);
                    }

                    _oldTileScreenSpaceMouseAlways = Coordinates.PointingToTileScreenSpace;
                }
            }

            if (_isLimitDrawingToHorizontal)
            {
                Coordinates.PointingToTileScreenSpace.Y = _permY;
            }
            else if (_isLimitDrawingToVertical)
            {
                Coordinates.PointingToTileScreenSpace.X = _permX;
            }

            if (_isMouseLeftDragging && _oldTileScreenSpaceMouseLeft != Coordinates.PointingToTileScreenSpace &&
                Coordinates.IsInCanvasBounds)
            {
                if (!_drawableManager.GetDrawables(Coordinates.GetWorldSpaceTile(Coordinates.PointingToTileScreenSpace),
                        ActiveBlock).Any(x =>
                        ScriptHelper.NonInteractableScriptTypes.All(y => y != x.ParentPool.ScriptType)))
                {
                    if (ActiveBlock.ParentPool.IsAninmatable)
                    {
                        Animatable animatable = Animatable.Instantiate(_drawableManager,
                            Coordinates.PointingToTileScreenSpace.X,
                            Coordinates.PointingToTileScreenSpace.Y
                            , ActiveBlock.ParentPool.OriginalPool, 0,
                            100, true,
                            2);
                        animatable.Script =
                            Activator.CreateInstance(animatable.ParentPool.ScriptType, animatable) as
                                AnimatableScript;
                    }
                    else
                    {
                        Drawable drawable = Drawable.Instantiate(_drawableManager,
                            Coordinates.PointingToTileScreenSpace.X,
                            Coordinates.PointingToTileScreenSpace.Y, ActiveBlock.ParentPool.OriginalPool, 0,
                            2,
                            ActiveBlock.Rotation);
                        drawable.Script =
                            Activator.CreateInstance(drawable.ParentPool.ScriptType, drawable) as
                                DrawableScript;
                    }
                }

                _oldTileScreenSpaceMouseLeft = Coordinates.PointingToTileScreenSpace;
            }

            if (_isMouseRightDragging && _oldTileScreenSpaceMouseRight != Coordinates.PointingToTileScreenSpace)
            {
                _drawableManager.GetDrawables(Coordinates.GetWorldSpaceTile(Coordinates.PointingToTileScreenSpace),
                    ActiveBlock)?.FirstOrDefault(x =>
                    ScriptHelper.NonInteractableScriptTypes.All(y => y != x.ParentPool.ScriptType))?.Close();

                _oldTileScreenSpaceMouseRight = Coordinates.PointingToTileScreenSpace;
            }

            switch (e.type)
            {
                case SDL_EventType.SDL_QUIT:
                    return false;
                case SDL_EventType.SDL_KEYDOWN:
                    if (e.key.repeat == 0)
                    {
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_LCTRL:
                                _isLimitDrawingToHorizontal = true;
                                _isLimitDrawingToVertical = true;
                                ScriptHelper.DrawGuideLines(_drawableManager,
                                    (Configuration.ControlsWidth, Coordinates.PointingToTileScreenSpace.Y),
                                    (Configuration.WindowSizeX, Coordinates.PointingToTileScreenSpace.Y),
                                    TransformFlags.Horizontal);
                                ScriptHelper.DrawGuideLines(_drawableManager,
                                    (Coordinates.PointingToTileScreenSpace.X, 0),
                                    (Coordinates.PointingToTileScreenSpace.X, Configuration.WindowSizeY),
                                    TransformFlags.Vertical);
                                _permX = Coordinates.PointingToTileScreenSpace.X;
                                _permY = Coordinates.PointingToTileScreenSpace.Y;

                                break;
                        }
                    }

                    break;
                case SDL_EventType.SDL_KEYUP:
                    if (e.key.repeat == 0)
                    {
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_LCTRL:
                                _isStopDrawingGuides = false;
                                _isLimitDrawingToHorizontal = false;
                                _isLimitDrawingToVertical = false;
                                ScriptHelper.DestroyGuideLines(_drawableManager, (_oldTileScreenSpaceMouseAlways.X, 0),
                                    (_oldTileScreenSpaceMouseAlways.X, Configuration.WindowSizeY),
                                    TransformFlags.Vertical);
                                ScriptHelper.DestroyGuideLines(_drawableManager,
                                    (Configuration.ControlsWidth, _oldTileScreenSpaceMouseAlways.Y),
                                    (Configuration.WindowSizeX, _oldTileScreenSpaceMouseAlways.Y),
                                    TransformFlags.Horizontal);
                                _permX = Coordinates.PointingToTileScreenSpace.X;
                                _permY = Coordinates.PointingToTileScreenSpace.Y;
                                break;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:

                    if (e.button.button == SDL_BUTTON_MIDDLE &&
                        Coordinates.PointingToRaw.X > Configuration.ControlsWidth)
                    {
                        _clickDownFocus = Coordinates.PointingToRaw;
                        _clickDownInitFocus = Camera2D.FocusPxs;
                        _isWaitingMouseMiddleUp = true;
                    }

                    if (Coordinates.IsInCanvasBounds)
                    {
                        if (e.button.button == SDL_BUTTON_LEFT)
                        {
                            _isMouseLeftDragging = true;
                        }

                        if (e.button.button == SDL_BUTTON_RIGHT)
                        {
                            _isMouseRightDragging = true;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (e.button.button == SDL_BUTTON_MIDDLE &&
                        Coordinates.PointingToRaw.X > Configuration.ControlsWidth)
                    {
                        _isWaitingMouseMiddleUp = false;
                    }

                    if (Coordinates.IsInCanvasBounds)
                    {
                        if (e.button.button == SDL_BUTTON_LEFT)
                        {
                            _isMouseLeftDragging = false;
                        }
                        else if (e.button.button == SDL_BUTTON_RIGHT)
                        {
                            _isMouseRightDragging = false;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEWHEEL:
                    if (e.wheel.y > 0) // scroll up
                    {
                        switch (ActiveBlock.ParentPool.ScrollType)
                        {
                            case ScrollTransformFlags.NoScroll:
                                break;
                            case ScrollTransformFlags.SixDirections: //5->10
                                if ((int)ActiveBlock.Rotation is >= 5 and < 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation + 1));
                                }
                                else if ((int)ActiveBlock.Rotation == 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)5);
                                }

                                break;
                            case ScrollTransformFlags.HorizontalVertical: //9->10
                                if ((int)ActiveBlock.Rotation is >= 9 and < 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation + 1));
                                }
                                else if ((int)ActiveBlock.Rotation == 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)9);
                                }

                                break;
                        }
                    }
                    else if (e.wheel.y < 0) // scroll down
                    {
                        switch (ActiveBlock.ParentPool.ScrollType)
                        {
                            case ScrollTransformFlags.NoScroll:
                                break;
                            case ScrollTransformFlags.SixDirections:
                                if ((int)ActiveBlock.Rotation is > 5 and <= 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation - 1));
                                }
                                else if ((int)ActiveBlock.Rotation == 5)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)10);
                                }

                                break;
                            case ScrollTransformFlags.HorizontalVertical: //9->10
                                if ((int)ActiveBlock.Rotation is > 9 and <= 10)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation + 1));
                                }
                                else if ((int)ActiveBlock.Rotation == 9)
                                {
                                    ActiveBlock.SetSurface((TransformFlags)10);
                                }

                                break;
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
