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
    private bool _isWaitingMouseLeftUp;
    private bool _isWaitingMouseMiddleUp;

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

    //TODO: Add scroll wheel rotation cycling
    //TODO: Add right click to delete
    //TODO: Add dragging
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

            switch (e.type)
            {
                case SDL_EventType.SDL_QUIT:
                    return false;
                case SDL_EventType.SDL_KEYDOWN:
                    //TODO: Keyboard events
                    break;
                case SDL_EventType.SDL_KEYUP:
                    //TODO: Keyboard events
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
                            if (!_isWaitingMouseLeftUp)
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
                            }

                            _isWaitingMouseLeftUp = true;
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
                            if (_isWaitingMouseLeftUp)
                            {
                                _isWaitingMouseLeftUp = false;
                            }
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

                                break;
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
