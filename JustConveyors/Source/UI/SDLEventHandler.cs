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

    //private bool _isWaitingMouseLeftUp;
    private bool _isMouseLeftDragging;

    //private bool _isWaitingMouseRightUp;
    private bool _isMouseRightDragging;

    private bool _isWaitingMouseMiddleUp;

    private (int X, int Y) _oldTileScreenSpaceMouseLeft;
    private (int X, int Y) _oldTileScreenSpaceMouseRight;

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


            if (_isMouseLeftDragging && _oldTileScreenSpaceMouseLeft != Coordinates.PointingToTileScreenSpace)
            {
                if (ActiveBlock.ParentPool.IsAninmatable)
                {
                    Animatable animatable = Animatable.Instantiate(_drawableManager,
                        Coordinates.PointingToTileScreenSpace.X,
                        Coordinates.PointingToTileScreenSpace.Y, ActiveBlock.ParentPool.OriginalPool, 0,
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

                _oldTileScreenSpaceMouseLeft = Coordinates.PointingToTileScreenSpace;
            }

            if (_isMouseRightDragging && _oldTileScreenSpaceMouseRight != Coordinates.PointingToTileScreenSpace)
            {
                _drawableManager
                    .GetDrawables(Coordinates.GetWorldSpaceTile(Coordinates.PointingToTileScreenSpace),
                        ActiveBlock)?.LastOrDefault(x =>
                        ScriptHelper.NonInteractableScriptTypes.Any(y => y != x.ParentPool.ScriptType))
                    ?.Close();
                _isMouseRightDragging = false;
                _oldTileScreenSpaceMouseRight = Coordinates.PointingToTileScreenSpace;
            }

            switch (e.type)
            {
                case SDL_EventType.SDL_QUIT:
                    return false;
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
                            //_isWaitingMouseLeftUp = true;
                            _isMouseLeftDragging = true;
                        }

                        if (e.button.button == SDL_BUTTON_RIGHT)
                        {
                            //_isWaitingMouseRightUp = true;
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
                            //_isWaitingMouseLeftUp = false;
                            _isMouseLeftDragging = false;
                        }
                        else if (e.button.button == SDL_BUTTON_RIGHT)
                        {
                            //_isWaitingMouseRightUp = false;
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
