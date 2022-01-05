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
        using (Display display = new Display("JustConveyor", 1280, 800, false))
        {
            while (display.Window != 0)
            {
                while (SDL_PollEvent(out SDL_Event e) != 0)
                {
                    display.Renderer.ProcessEvent(e);
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            goto Exit;
                        case SDL_EventType.SDL_KEYDOWN:
                            break;
                        case SDL_EventType.SDL_KEYUP:
                            break;
                    }
                }

                display.Renderer.NewFrame();
                ImGui.ShowDemoWindow();
                display.Renderer.Render();

                SDL_GL_SwapWindow(display.Window);
                Time.NextFrame();
                Debug.WriteLine($@"FPS: {1 / Time.DeltaTime}
Frametime: {Time.DeltaTime * 1000d} ms");
            }
        }

        Exit: ;
    }
}
