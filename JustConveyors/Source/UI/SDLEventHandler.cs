using System.Numerics;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source.UI;

internal class SDLEventHandler
{
    public static Drawable ActiveBlock { get; set; }

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

        //Temp: move this later
        IntPtr cursorSurface = SDL_CreateRGBSurface(0, 16, 16, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000);
        SDL_BlitSurface(PoolResources.JunctionPool.Surfaces[0], IntPtr.Zero, cursorSurface, IntPtr.Zero);
        ActiveBlock = Drawable.Instantiate(_drawableManager, (int)Coordinates.CenterScr.X,
            (int)Coordinates.CenterScr.Y, cursorSurface, 0, 5);
        ActiveBlock.SetAlpha(100, 0, 1);
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
                                Animatable.Instantiate(_drawableManager, Coordinates.PointingToTileScreenSpace.X,
                                    Coordinates.PointingToTileScreenSpace.Y, PoolResources.JunctionPool, 0, 100, true,
                                    2);
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
