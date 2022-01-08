using System.Numerics;
using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source.Drawing;

internal class Grid : IRenderHolder
{
    public SDL_Rect WorkingAreaRect;

    private IntPtr _currentSurface;

    public Grid(Display display, Texture texture)
    {
        _display = display;
        _texture = texture;
        Zoom.OnZoomChanged += OnZoomChanged;
        Zoom.OnFocusChanged += OnFocusChanged;
    }

    private void OnZoomChanged(float newZoom)
    {
        RedrawGrid(newZoom, Zoom.FocusPosition);
    }

    private void OnFocusChanged(Vector2 newFocus)
    {
        RedrawGrid(Zoom.M, newFocus);
    }

    void RedrawGrid(float zoom, Vector2 focus)
    {
        DrawGrid(zoom, focus);
        if (_currentSurface != IntPtr.Zero)
            SDL_FreeSurface(_currentSurface);
        GenerateGridSurface();
    }

    public void Dispose()
    {
        if (_currentSurface != IntPtr.Zero)
            SDL_FreeSurface(_currentSurface);
    }

    private Texture _texture;
    public Display _display { get; }

    public void Load()
    {
        WorkingAreaRect = new SDL_Rect()
        {
            w = Configuration.WindowSizeX - Configuration.ControlsWidth,
            h = Configuration.WindowSizeY,
            x = Configuration.ControlsWidth,
            y = 0
        };
    }

    public void Cleanup()
    {
    }

    private void DrawGrid(float zoom, Vector2 focus)
    {
        SDL_RenderSetScale(_display.SDLRenderer, zoom * 2, zoom * 2);
        SDL_SetRenderDrawColor(_display.SDLRenderer, 0, 0, 0, 255);
        SDL_RenderClear(_display.SDLRenderer);
        SDL_SetRenderDrawColor(_display.SDLRenderer, 100, 100, 100, 255);

        for (var i = Configuration.ControlsWidth; i < Configuration.WindowSizeX; i += 16)
        {
            for (var j = 0; j < Configuration.WindowSizeY; j += 16)
            {
                SDL_RenderDrawLine(_display.SDLRenderer, i, 0, i, Configuration.WindowSizeY);
                SDL_RenderDrawLine(_display.SDLRenderer, Configuration.ControlsWidth, j, Configuration.WindowSizeX, j);
            }
        }
    }

    private unsafe void GenerateGridSurface()
    {
        uint Rmask = 0x00ff0000;
        uint Gmask = 0x0000ff00;
        uint Bmask = 0x000000ff;
        uint Amask = 0xff000000;
        IntPtr frameSurfaceHandle = SDL_CreateRGBSurface(0, WorkingAreaRect.w, WorkingAreaRect.h, 32,
            Rmask, Gmask, Bmask, Amask);
        SDL_Surface frameSurface = Texture.PtrToSurface(frameSurfaceHandle);
        SDL_RenderReadPixels(_display.SDLRenderer, ref WorkingAreaRect, SDL_PIXELFORMAT_ARGB8888,
            frameSurface.pixels,
            frameSurface.pitch);
        _currentSurface = frameSurfaceHandle;
    }

    public void Render()
    {
        _texture.DrawSurface(_currentSurface, ref WorkingAreaRect);
    }
}
