using System.Diagnostics;
using System.Runtime.InteropServices;
using ImGuiNET;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDLImGuiGL;
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
            ComponentManager componentManager = new();
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

                SDL_RenderClear(display.SDLRenderer);
                OnUpdate?.Invoke();

                display.Renderer.NewFrame();
                ImGui.ShowDemoWindow();

                unsafe
                {
                    GL.glClearColor(0f, 0f, 0f, 1f);
                    GL.glClear(GL.ClearBufferMask.ColorBufferBit);

                    int scrX = (int)display.WindowSize.X;
                    int scrY = (int)display.WindowSize.Y;
                    var rect = new SDL_Rect() { h = scrX, w = scrY, x = 0, y = 0 };

                    var frameSurfaceHandle = SDL_CreateRGBSurface(0, scrX, scrY, 32, 0x00ff0000,
                        0x0000ff00,
                        0x000000ff,
                        0xff000000);
                    var frameSurface = Marshal.PtrToStructure<SDL_Surface>(frameSurfaceHandle);
                    SDL_RenderReadPixels(display.SDLRenderer, ref rect, SDL_PIXELFORMAT_ARGB8888,
                        frameSurface.pixels,
                        frameSurface.pitch);

                    uint textureId = GL.GenTexture();
                    GL.glBindTexture(GL.TextureTarget.Texture2D, textureId);
                    GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMinFilter,
                        GL.TextureParameter.Nearest);
                    GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMagFilter,
                        GL.TextureParameter.Nearest);
                    GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapS,
                        GL.TextureParameter.Repeat);
                    GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapT,
                        GL.TextureParameter.Repeat);
                    GL.glTexImage2D(GL.TextureTarget.Texture2D, 0, GL.PixelInternalFormat.Rgba8, scrX, scrY, 0,
                        GL.PixelFormat.Rgba, GL.PixelType.UnsignedByte, frameSurface.pixels);

                    SDL_FreeSurface(frameSurfaceHandle);

                    display.Renderer.Render();
                    GL.DeleteTexture(textureId);

                    SDL_GL_SwapWindow(display.Window);
                }

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
