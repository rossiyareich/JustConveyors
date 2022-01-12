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
        ActiveBlock.SetAlpha(100, 0, 1);
    }

    //TODO: Add scroll wheel rotation cycling
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
                    if (Coordinates.PointingToRaw.X > Configuration.ControlsWidth)
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
                                    animatable.Script = new testConveyorScript(animatable);
                                }
                                else
                                {
                                    Drawable.Instantiate(_drawableManager, Coordinates.PointingToTileScreenSpace.X,
                                        Coordinates.PointingToTileScreenSpace.Y, ActiveBlock.ParentPool.OriginalPool, 0,
                                        2,
                                        ActiveBlock.Rotation);
                                }
                            }

                            _isWaitingMouseLeftUp = true;
                        }
                        else if (e.button.button == SDL_BUTTON_MIDDLE)
                        {
                            _clickDownFocus = Coordinates.PointingToRaw;
                            _clickDownInitFocus = Camera2D.FocusPxs;
                            _isWaitingMouseMiddleUp = true;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (Coordinates.PointingToRaw.X > Configuration.ControlsWidth)
                    {
                        if (e.button.button == SDL_BUTTON_LEFT)
                        {
                            if (_isWaitingMouseLeftUp)
                            {
                                _isWaitingMouseLeftUp = false;
                            }
                        }
                        else if (e.button.button == SDL_BUTTON_MIDDLE)
                        {
                            _isWaitingMouseMiddleUp = false;
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
