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
                        SDL_GetMouseState(out int x, out int y);
                        if (x > Configuration.ControlsWidth)
                            s_componentManager.InstantiateDrawable<Animatable>(x, y, PoolResources.JunctionPool, 0);
                        s_isWaitingMouseUp = true;
                    }

                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (s_isWaitingMouseUp)
                    {
                        s_isWaitingMouseUp = false;
                    }

                    break;
            }
        }

        return true;
    }
}
