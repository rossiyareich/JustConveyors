﻿using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Controls;
using JustConveyors.Source.Drawing;
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

        using (Display display = new("JustConveyor", Configuration.WindowSizeX - 3, Configuration.WindowSizeY,
                   false)) //Added margin correction :P
        using (SDLOpenGL glHelper = new(display))
        using (Grid grid = new(display, glHelper.Texture))
        using (ComponentManager componentManager = new())
        {
            glHelper.Load();
            componentManager.InitializeComponents(display, glHelper.Texture);
            SDLEventHandler.Load(display, componentManager);
            grid.Load();

            Zoom.ChangeZoom(Zoom.M, out _);
            Zoom.ChangeFocus(Zoom.FocusPosition, out _);

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
