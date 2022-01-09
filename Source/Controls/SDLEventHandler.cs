using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source.Controls;

internal static class SDLEventHandler
{
    private static Display s_display;
    private static ComponentManager s_componentManager;

    private static bool s_isWaitingMouseUp;

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
                    if (!s_isWaitingMouseUp)
                    {
                        if (Coordinates.PointingToScreenSpace.X > Configuration.ControlsWidth)
                        {
                            s_componentManager.InstantiateDrawable<Animatable>(PoolResources.JunctionPool, 0);
                        }

                        s_isWaitingMouseUp = true;
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (s_isWaitingMouseUp)
                    {
                        s_isWaitingMouseUp = false;
                    }

                    break;
                case SDL_EventType.SDL_MOUSEWHEEL:
                    if (Coordinates.PointingToScreenSpace.X > Configuration.ControlsWidth)
                    {
                        if (e.wheel.y > 0 && Zoom.M < 3f) // scroll up
                        {
                            Zoom.ChangeZoom(Zoom.M + 0.5f);
                            Zoom.ChangeFocusTile();
                        }
                        else if (e.wheel.y < 0 && Zoom.M > 1f) // scroll down
                        {
                            Zoom.ChangeZoom(Zoom.M - 0.5f);
                            Zoom.ChangeFocusTile();
                        }
                        else if (e.wheel.y < 0)
                        {
                            Zoom.ChangeZoom(1f);
                            Zoom.ChangeFocusPxs(Configuration.CenterScr);
                        }
                    }

                    break;
            }
        }

        return true;
    }
}
