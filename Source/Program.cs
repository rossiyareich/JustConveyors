using System.Diagnostics;
using ImGuiNET;
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
        using (Display display = new("JustConveyor", 1280, 800, false))
        {
            ComponentManager componentManager = new ComponentManager();
            componentManager.InitializeComponents(display);

            OnStart?.Invoke();
            while (display.Window != 0)
            {
                while (SDL_PollEvent(out SDL_Event e) != 0)
                {
                    display.Renderer.ProcessEvent(e);
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            OnClose?.Invoke();
                            goto Exit;
                        case SDL_EventType.SDL_KEYDOWN:
                            //TODO: Keyboard events
                            break;
                        case SDL_EventType.SDL_KEYUP:
                            //TODO: Keyboard events
                            break;
                    }
                }

                OnUpdate?.Invoke();

                display.Renderer.NewFrame();
                ImGui.ShowDemoWindow();
                display.Renderer.Render();

                SDL_GL_SwapWindow(display.Window);

                OnLateUpdate?.Invoke();
                Time.NextFrame();

#if DEBUG
                Debug.WriteLine($@"FPS: {1 / Time.DeltaTime}
Frametime: {Time.DeltaTime * 1000d} ms");
#endif
            }
        }

        Exit: ;
    }
}
