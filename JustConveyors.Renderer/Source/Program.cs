using JustConveyors.Renderer.Source.ConfigurationNS;
using JustConveyors.Renderer.Source.Controls;
using JustConveyors.Renderer.Source.Drawing;
using JustConveyors.Renderer.Source.Loop;
using JustConveyors.Renderer.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Renderer.Source;

// ReSharper disable ClassNeverInstantiated.Global
public class Program
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
        using (Grid grid = new(display, glHelper.Texture))
        using (ComponentManager componentManager = new())
        {
            glHelper.Load();
            componentManager.InitializeComponents(display, glHelper.Texture);
            SDLEventHandler.Load(display, componentManager);
            grid.Load();

            Zoom.ChangeZoom(Zoom.M);
            Zoom.ChangeFocusPxs(Zoom.FocusPxs);

            OnStart?.Invoke();

            while (display.Window != 0)
            {
                if (!SDLEventHandler.PollEvents())
                {
                    OnClose?.Invoke();
                    break;
                }

                grid.Render();
                OnUpdate?.Invoke();
                display.Renderer.NewFrame();
                glHelper.Render();
                GUI.Draw();
                display.Renderer.Render();
                glHelper.Cleanup();
                grid.Cleanup();
                SDL_GL_SwapWindow(display.Window);
                OnLateUpdate?.Invoke();
                Time.NextFrame();
            }
        }
    }
}
