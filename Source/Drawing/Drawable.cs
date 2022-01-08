using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Drawing;

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

    protected override void Update()
    {
        SDL.SDL_SetSurfaceBlendMode(_surfaces[CurrentIndex],
            SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND); // Do something about this :D
        SDL.SDL_SetSurfaceAlphaMod(_surfaces[CurrentIndex], 100); // Do something about this :D
        Texture.DrawSurface(_surfaces[CurrentIndex], ref Transform);
    }

    protected override void LateUpdate()
    {
    }
}
