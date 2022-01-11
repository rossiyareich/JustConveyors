using System.Runtime.InteropServices;
using JustConveyors.Libraries.ImGuiGL.OpenGL;
using static SDL2.SDL;

namespace JustConveyors.Source.Rendering;

internal class Texture : IRenderComponent
{
    private uint _currentGLTexture;
    private IntPtr _currentSDLSurface;

    public Texture(Display display)
    {
        _display = display;
        _surfaces = new List<(uint layer, IntPtr surface, SDL_Rect area)>();
    }

    private int _scrX => (int)_display.WindowSize.X;
    private int _scrY => (int)_display.WindowSize.Y;
    private List<(uint layer, IntPtr surface, SDL_Rect area)> _surfaces;
    public Display _display { get; }

    public void Dispose()
    {
        Cleanup();
        GL.DeleteTexture(_currentGLTexture);
    }

    public void Load()
    {
        _currentSDLSurface = SDL_CreateRGBSurface(0, _scrX, _scrY, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000);

        _currentGLTexture = GL.GenTexture();
        GL.glBindTexture(GL.TextureTarget.Texture2D, _currentGLTexture);
        GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMinFilter,
            GL.TextureParameter.Nearest);
        GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureMagFilter,
            GL.TextureParameter.Nearest);
        GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapS,
            GL.TextureParameter.Repeat);
        GL.glTexParameteri(GL.TextureTarget.Texture2D, GL.TextureParameterName.TextureWrapT,
            GL.TextureParameter.Repeat);

        RendererToTexture();
    }

    public void Cleanup()
    {
        GL.glBindTexture(GL.TextureTarget.Texture2D, 0);
        SDL_FillRect(_currentSDLSurface, IntPtr.Zero, SDL_MapRGB(PtrToSurface(_currentSDLSurface).format, 0, 0, 0));
        _surfaces.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="surface"></param>
    /// <param name="area"></param>
    /// <param name="layer">
    /// Layer must be greater than 1
    /// </param>
    public void DrawSurface(IntPtr surface, ref SDL_Rect area, uint layer)
    {
        _surfaces.Add((layer, surface, area));
    }

    public void RendererToTexture()
    {
        if (_surfaces.Any())
            for (int i = 0; i <= _surfaces.Max(x => x.layer); i++)
            {
                foreach (var surface in _surfaces.Where(x => x.layer == i))
                {
                    var area = surface.area;
                    SDL_BlitSurface(surface.surface, IntPtr.Zero, _currentSDLSurface, ref area);
                }
            }

        GL.glBindTexture(GL.TextureTarget.Texture2D, _currentGLTexture);
        GL.glTexImage2D(GL.TextureTarget.Texture2D, 0, GL.PixelInternalFormat.Rgba8, _scrX, _scrY, 0,
            GL.PixelFormat.Rgba, GL.PixelType.UnsignedByte, PtrToSurface(_currentSDLSurface).pixels);
    }

    public static SDL_Surface PtrToSurface(IntPtr ptr) => Marshal.PtrToStructure<SDL_Surface>(ptr);
}
