using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.UI;
using static SDL2.SDL;

namespace JustConveyors.Source;

// ReSharper disable ClassNeverInstantiated.Global
internal class Program : IEventHolder
{
    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnLateUpdate;
    public event Action OnClose;

    public static void Main(string[] args)
    {
        Configuration.Load("config.json");

        Program @this = new Program();

        using (Display display = new("JustConveyor", Configuration.WindowSizeX, Configuration.WindowSizeY, false))
        using (SDLOpenGL glManager = new(display))
        using (Grid grid = new(display, glManager.Texture))
        using (DrawableManager drawableManager = new(display, glManager.Texture, @this))
        {
            glManager.Load();
            grid.Load();

            SDLEventHandler uiHandler = new(display, drawableManager);
            Camera2D.ChangeZoom(Camera2D.M);
            Camera2D.ChangeFocusScrRaw(Camera2D.FocusPxs);

            @this.OnStart?.Invoke();
            while (display.Window != 0)
            {
                if (!uiHandler.PollEvents())
                {
                    @this.OnClose?.Invoke();
                    break;
                }

                grid.Render();
                @this.OnUpdate?.Invoke();
                display.Renderer.NewFrame();
                glManager.Render();
                GUI.Draw();
                display.Renderer.Render();
                glManager.Cleanup();
                grid.Cleanup();
                SDL_GL_SwapWindow(display.Window);
                @this.OnLateUpdate?.Invoke();
                Time.NextFrame();
            }
        }
    }
}
