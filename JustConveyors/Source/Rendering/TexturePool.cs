using JustConveyors.Source.Drawing;
using SDL2;

namespace JustConveyors.Source.Rendering;

internal class TexturePool : IDisposable
{
    public TexturePool(TexturePool existing) : this(
        existing.IsAninmatable, existing.ScriptType, existing.ImageList.ToArray())
    {
        foreach ((IntPtr surface, TransformFlags flag) oldSurface in existing.Surfaces)
        {
            Surfaces.Add((Texture.Copy(oldSurface.surface), oldSurface.flag));
        }

        OriginalPool = existing;
        ScrollType = existing.ScrollType;
    }

    public TexturePool(bool isAnimatable, Type scriptType, params (string, TransformFlags)[] images)
    {
        IsAninmatable = isAnimatable;
        ScriptType = scriptType;
        Surfaces = new List<(IntPtr, TransformFlags)>();
        ImageList = new List<(string, TransformFlags)>();
        ImageList.AddRange(images);
    }

    public TexturePool OriginalPool { get; }

    public ScrollTransformFlags ScrollType { get; init; } = ScrollTransformFlags.NoScroll;
    public Type ScriptType { get; }
    public bool IsAninmatable { get; }
    public List<(IntPtr surface, TransformFlags flag)> Surfaces { get; }
    public List<(string, TransformFlags)> ImageList { get; }

    public void Dispose()
    {
        foreach ((IntPtr surface, TransformFlags flag) surface in Surfaces)
        {
            if (surface.surface != IntPtr.Zero)
            {
                SDL.SDL_FreeSurface(surface.surface);
            }
        }
    }

    public void Generate()
    {
        SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);

        foreach ((string path, TransformFlags flag) image in ImageList)
        {
            Surfaces.Add((SDL_image.IMG_Load(image.path), image.flag));
        }

        SDL_image.IMG_Quit();
    }
}
