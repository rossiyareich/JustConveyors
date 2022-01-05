using System.Numerics;
using SDLImGuiGL;
using static SDL2.SDL;

namespace JustConveyors.Source.Rendering;

internal class Display : IDisposable
{
    public Display(string title, int width, int height, bool vsync)
    {
        (Window, GlContext) = ImGuiGL.CreateWindowAndGLContext(title, width, height);
        Renderer = new ImGuiGLRenderer(Window, GlContext);

        WindowSize = new Vector2(width, height);

        SDL_GL_SetSwapInterval(vsync ? 1 : 0);
    }

    public ImGuiGLRenderer Renderer { get; }
    public nint Window { get; }
    public nint GlContext { get; }
    public Vector2 WindowSize { get; }

    public void Dispose()
    {
        SDL_GL_DeleteContext(GlContext);
        SDL_DestroyWindow(Window);
        SDL_Quit();
    }
}
