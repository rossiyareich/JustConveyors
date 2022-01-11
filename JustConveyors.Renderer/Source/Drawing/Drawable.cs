using JustConveyors.Renderer.Source.Loop;
using JustConveyors.Renderer.Source.Rendering;
using SDL2;

namespace JustConveyors.Renderer.Source.Drawing;

internal class Drawable : Component
{
    protected readonly List<nint> _surfaces;
    public SDL.SDL_Rect Transform;

    public Drawable(Display display, Texture texture, ref SDL.SDL_Rect transform, TexturePool pool, int startIndex)
        : base(display, texture)
    {
        _surfaces = pool.Surfaces;
        Transform = transform;
        CurrentIndex = startIndex;
    }

    public int CurrentIndex { get; protected set; }

    public void ChangeProp(int index, SDL.SDL_Rect rect)
    {
        CurrentIndex = index;
        Transform = rect;
    }

    protected override void Start()
    {
    }

    /// <param name="alpha"></param>
    /// <param name="startIndex">
    ///     Start index is inclusive
    /// </param>
    /// <param name="endIndex">
    ///     End index is exclusive
    /// </param>
    public virtual void SetAlpha(byte alpha, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            SDL.SDL_SetSurfaceBlendMode(_surfaces[i],
                SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetSurfaceAlphaMod(_surfaces[i], alpha);
        }
    }

    protected override void Update() => Texture.DrawSurface(_surfaces[CurrentIndex], ref Transform);

    protected override void LateUpdate()
    {
    }
}
