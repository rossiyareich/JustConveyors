using System.Numerics;
using JustConveyors.Renderer.Source.ConfigurationNS;
using JustConveyors.Renderer.Source.Drawing;
using JustConveyors.Renderer.Source.Loop;
using JustConveyors.Renderer.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Renderer.Source.Controls;

internal static class SDLEventHandler
{
    private static Display s_display;
    private static ComponentManager s_componentManager;

    private static bool s_isWaitingMouseLeftUp;
    private static bool s_isWaitingMouseMiddleUp;
    private static Vector2 s_ClickDownPos;
    private static Vector2 s_ClickDownFocus;

    public static void Load(Display display, ComponentManager componentManager)
    {
        s_display = display;
        s_componentManager = componentManager;
    }

    public static bool PollEvents()
    {
        while (SDL_PollEvent(out SDL_Event e) != 0)
        {
            s_display.Renderer.ProcessEvent(e);
            Coordinates.UpdatePointer();
            if (s_isWaitingMouseMiddleUp)
            {
                Zoom.ChangeFocusPxs(s_ClickDownFocus + (Coordinates.PointingToScreenSpace - s_ClickDownPos));
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
                    if (Coordinates.PointingToScreenSpace.X > Configuration.ControlsWidth)
                    {
                        if (e.button.button == SDL_BUTTON_LEFT)
                        {
                            if (!s_isWaitingMouseLeftUp)
                            {
                                s_componentManager.InstantiateDrawable<Animatable>(PoolResources.JunctionPool, 0);
                            }

                            s_isWaitingMouseLeftUp = true;
                        }
                        else if (e.button.button == SDL_BUTTON_MIDDLE)
                        {
                            s_ClickDownPos = Coordinates.PointingToScreenSpace;
                            s_ClickDownFocus = Zoom.FocusPxs;
                            s_isWaitingMouseMiddleUp = true;
                        }
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (Coordinates.PointingToScreenSpace.X > Configuration.ControlsWidth)
                    {
                        if (e.button.button == SDL_BUTTON_LEFT)
                        {
                            if (s_isWaitingMouseLeftUp)
                            {
                                s_isWaitingMouseLeftUp = false;
                            }
                        }
                        else if (e.button.button == SDL_BUTTON_MIDDLE)
                        {
                            s_isWaitingMouseMiddleUp = false;
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
