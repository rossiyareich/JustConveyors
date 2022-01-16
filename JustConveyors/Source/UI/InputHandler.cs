using System.Numerics;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;
using static SDL2.SDL;

namespace JustConveyors.Source.UI;

internal partial class SDLEventHandler
{
    private readonly Display _display;

    private Vector2 _clickDownFocus;
    private Vector2 _clickDownInitFocus;

    private bool _isLimitDrawingToHorizontal;
    private bool _isLimitDrawingToVertical;

    private bool _isMouseLeftDragging;
    private bool _isMouseRightDragging;
    private bool _isStopDrawingGuides;
    private bool _isWaitingMouseMiddleUp;
    private (int X, int Y) _oldTileScreenSpaceMouseAlways;

    private int _permX;
    private int _permY;

    private void SetPermCoord()
    {
        _permX = Coordinates.PointingToTileScreenSpace.X;
        _permY = Coordinates.PointingToTileScreenSpace.Y;
    }

    private void SetScrollActive(int lower, int upper, int delta)
    {
        if (delta > 0)
        {
            if ((int)ActiveBlock.Rotation >= lower && (int)ActiveBlock.Rotation < upper)
            {
                ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation + delta));
            }
            else if ((int)ActiveBlock.Rotation == upper)
            {
                ActiveBlock.SetSurface((TransformFlags)lower);
            }
        }
        else if (delta < 0)
        {
            if ((int)ActiveBlock.Rotation > lower && (int)ActiveBlock.Rotation <= upper)
            {
                ActiveBlock.SetSurface((TransformFlags)((int)ActiveBlock.Rotation + delta));
            }
            else if ((int)ActiveBlock.Rotation == lower)
            {
                ActiveBlock.SetSurface((TransformFlags)upper);
            }
        }
    }

    public bool PollEvents()
    {
        while (SDL_PollEvent(out SDL_Event e) != 0)
        {
            _display.Renderer.ProcessEvent(e);

            bool isMouseMiddleInFocus = e.button.button == SDL_BUTTON_MIDDLE &&
                                        Coordinates.PointingToRaw.X > Configuration.ControlsWidth;
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
                                SetVertical(Coordinates.PointingToTileScreenSpace);
                                ScriptHelper.DrawGuideLines(_drawableManager, _startLine, _endLine, _hv);
                                SetHorizontal(Coordinates.PointingToTileScreenSpace);
                                ScriptHelper.DrawGuideLines(_drawableManager, _startLine, _endLine, _hv);
                                SetPermCoord();
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
                                SetVertical(Coordinates.PointingToTileScreenSpace);
                                ScriptHelper.DestroyGuideLines(_drawableManager, _startLine, _endLine, _hv);
                                SetHorizontal(Coordinates.PointingToTileScreenSpace);
                                ScriptHelper.DestroyGuideLines(_drawableManager, _startLine, _endLine, _hv);
                                SetPermCoord();
                                break;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (isMouseMiddleInFocus)
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
                    if (isMouseMiddleInFocus)
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

                        TryDestroyArrowSwallow();
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
                                SetScrollActive(5, 10, 1);
                                break;
                            case ScrollTransformFlags.HorizontalVertical: //9->10
                                SetScrollActive(9, 10, 1);
                                break;
                            case ScrollTransformFlags.FourDirections: //1->4
                                TryDestroyArrowSwallow();
                                SetScrollActive(1, 4, 1);
                                DrawGuideFromActive();
                                break;
                        }
                    }
                    else if (e.wheel.y < 0) // scroll down
                    {
                        switch (ActiveBlock.ParentPool.ScrollType)
                        {
                            case ScrollTransformFlags.NoScroll:
                                break;
                            case ScrollTransformFlags.SixDirections: //5->10
                                SetScrollActive(5, 10, -1);
                                break;
                            case ScrollTransformFlags.HorizontalVertical: //9->10
                                SetScrollActive(9, 10, -1);
                                break;
                            case ScrollTransformFlags.FourDirections: //1->4
                                TryDestroyArrowSwallow();
                                SetScrollActive(1, 4, -1);
                                DrawGuideFromActive();
                                break;
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
