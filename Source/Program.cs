using System.Diagnostics;
using System.Numerics;
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
        using (Display display = new("JustConveyor", 1920, 896, false))
        using (SDLOpenGL glHelper = new(display))
        using (ComponentManager componentManager = new())
        {
            glHelper.Load();
            componentManager.InitializeComponents(display, glHelper.Texture);

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

                SDL_RenderClear(display.SDLRenderer);
                OnUpdate?.Invoke();

                display.Renderer.NewFrame();

                glHelper.Render();

                ImGui.Begin("Controls", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                        ImGuiWindowFlags.NoMove);
                ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.8f, 0.47f, 0.55f, 0.4f));
                ImGui.SetWindowSize(new Vector2(320, 896));
                ImGui.SetWindowPos(new Vector2(0, 0));

                ImGui.End();

                display.Renderer.Render();

                glHelper.Cleanup();
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
