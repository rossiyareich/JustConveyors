using SDL2;

namespace JustConveyors.Renderer.Source.Rendering;

internal class TexturePool : IDisposable
{
    public TexturePool(params string[] imagePaths)
    {
        Surfaces = new List<IntPtr>();
        ImagePathList = new List<string>();
        ImagePathList.AddRange(imagePaths);
    }

    public List<IntPtr> Surfaces { get; }
    public List<string> ImagePathList { get; }

    public void Dispose()
    {
        foreach (nint surface in Surfaces)
        {
            if (surface != 0)
            {
                SDL.SDL_FreeSurface(surface);
            }
        }
    }

    public void Generate()
    {
        SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);

        foreach (string imagePath in ImagePathList)
        {
            Surfaces.Add(SDL_image.IMG_Load(imagePath));
        }

        SDL_image.IMG_Quit();
    }
}
