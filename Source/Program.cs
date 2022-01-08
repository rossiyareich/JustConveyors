using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Controls;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source;

// ReSharper disable ClassNeverInstantiated.Global
internal class Program
{
    public static event Action OnStart;
    public static event Action OnUpdate;
    public static event Action OnLateUpdate;
    public static event Action OnClose;

    public static void Main(string[] args)
    {
        Configuration.Load("config.json");

        using (Display display = new("JustConveyor", Configuration.WindowSizeX, Configuration.WindowSizeY, false))
        using (SDLOpenGL glHelper = new(display))
        using (ComponentManager componentManager = new())
        {
            glHelper.Load();
            componentManager.InitializeComponents(display, glHelper.Texture);
            SDLEventHandler.Load(display, componentManager);
            OnStart?.Invoke();

            while (display.Window != 0)
            {
                if (!SDLEventHandler.PollEvents())
                {
                    OnClose?.Invoke();
                    break;
                }

                SDL_RenderClear(display.SDLRenderer);
                OnUpdate?.Invoke();
                display.Renderer.NewFrame();
                glHelper.Render();
                GUI.Draw();
                display.Renderer.Render();
                glHelper.Cleanup();
                SDL_GL_SwapWindow(display.Window);
                OnLateUpdate?.Invoke();
                Time.NextFrame();
            }
        }
    }
}
