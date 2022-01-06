using System.Runtime.InteropServices;
using SDLImGuiGL;
using static SDL2.SDL;

namespace JustConveyors.Source.Rendering;

internal class Texture : IRenderComponent
{
    private uint _currentTexture;
    private SDL_Rect _displayRect;

    public Texture(Display display)
    {
        _display = display;
        _displayRect = new SDL_Rect { h = _scrX, w = _scrY, x = 0, y = 0 };
    }

    private int _scrX => (int)_display.WindowSize.X;
    private int _scrY => (int)_display.WindowSize.Y;
    public Display _display { get; }

    public void Dispose() => Cleanup();

    public void Load() => RendererToTexture();

    public void Cleanup()
    {
        GL.glBindTexture(GL.TextureTarget.Texture2D, 0);
        GL.DeleteTexture(_currentTexture);
    }

    public uint RendererToTexture()
    {
        IntPtr frameSurfaceHandle = SDL_CreateRGBSurface(0, _scrX, _scrY, 32, 0x00ff0000,
            0x0000ff00,
            0x000000ff,
            0xff000000);
        SDL_Surface frameSurface = Marshal.PtrToStructure<SDL_Surface>(frameSurfaceHandle);
        SDL_RenderReadPixels(_display.SDLRenderer, ref _displayRect, SDL_PIXELFORMAT_ARGB8888,
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
        GL.glTexImage2D(GL.TextureTarget.Texture2D, 0, GL.PixelInternalFormat.Rgba8, _scrX, _scrY, 0,
            GL.PixelFormat.Rgba, GL.PixelType.UnsignedByte, frameSurface.pixels);

        SDL_FreeSurface(frameSurfaceHandle);

        _currentTexture = textureId;
        return textureId;
    }
}
