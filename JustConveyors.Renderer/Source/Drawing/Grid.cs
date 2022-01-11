using JustConveyors.Renderer.Source.ConfigurationNS;
using JustConveyors.Renderer.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Renderer.Source.Drawing;

internal class Grid : IRenderHolder
{
    private readonly Texture _texture;
    private IntPtr _currentSurface;
    public SDL_Rect ScreenRect;

    public Grid(Display display, Texture texture)
    {
        _display = display;
        _texture = texture;
        Zoom.OnZoomChanged += OnZoomChanged;
    }

    public void Dispose()
    {
        if (_currentSurface != IntPtr.Zero)
        {
            SDL_FreeSurface(_currentSurface);
        }
    }

    public Display _display { get; }

    public void Load() =>
        ScreenRect = new SDL_Rect
        {
            w = Configuration.WindowSizeX, h = Configuration.WindowSizeY, x = Configuration.ControlsWidth
        };

    public void Cleanup()
    {
    }

    public void Render() => _texture.DrawSurface(_currentSurface, ref ScreenRect);

    private void OnZoomChanged(float newZoom) => RedrawGrid(newZoom);


    private void RedrawGrid(float zoom)
    {
        DrawGrid(zoom);
        if (_currentSurface != IntPtr.Zero)
        {
            SDL_FreeSurface(_currentSurface);
        }

        GenerateGridSurface();
    }

    private void DrawGrid(float zoom)
    {
        SDL_RenderSetScale(_display.SDLRenderer, 1f / zoom, 1f / zoom);
        SDL_SetRenderDrawColor(_display.SDLRenderer, 0, 0, 0, 255);
        SDL_RenderClear(_display.SDLRenderer);
        SDL_SetRenderDrawColor(_display.SDLRenderer, 25, 25, 25, 255);

        for (int i = 0; i < ScreenRect.w * zoom * 2f; i += (int)(16 * zoom))
        {
            SDL_RenderDrawLine(_display.SDLRenderer, i, 0, i, (int)(ScreenRect.h * zoom));
        }

        for (int j = 0; j < ScreenRect.h * zoom; j += (int)(16 * zoom))
        {
            SDL_RenderDrawLine(_display.SDLRenderer, 0, j, (int)(ScreenRect.w * zoom * 2f), j);
        }
    }

    private void GenerateGridSurface()
    {
        uint Rmask = 0x00ff0000;
        uint Gmask = 0x0000ff00;
        uint Bmask = 0x000000ff;
        uint Amask = 0xff000000;
        IntPtr frameSurfaceHandle =
            SDL_CreateRGBSurface(0, ScreenRect.w, ScreenRect.h, 32, Rmask, Gmask, Bmask,
                Amask);
        SDL_Surface frameSurface = Texture.PtrToSurface(frameSurfaceHandle);
        SDL_RenderReadPixels(_display.SDLRenderer, ref ScreenRect, SDL_PIXELFORMAT_ARGB8888,
            frameSurface.pixels,
            frameSurface.pitch);
        _currentSurface = frameSurfaceHandle;
    }
}
