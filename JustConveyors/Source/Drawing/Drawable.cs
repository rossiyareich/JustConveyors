using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Drawing;

internal class Drawable : Component
{
    public SDL.SDL_Rect Transform;

    public Drawable(Display display, Texture texture, ref SDL.SDL_Rect transform, TexturePool pool,
        int startSurfaceIndex, uint layer)
        : base(display, texture)
    {
        Surfaces = pool.Surfaces;
        Transform = transform;
        CurrentSurfaceIndex = startSurfaceIndex;
        Layer = layer;
    }

    public Drawable(Display display, Texture texture, ref SDL.SDL_Rect transform, IntPtr surface,
        int startSurfaceIndex, uint layer)
        : base(display, texture)
    {
        Surfaces = new List<nint>(1) { surface };
        Transform = transform;
        CurrentSurfaceIndex = startSurfaceIndex;
        Layer = layer;
    }

    public List<nint> Surfaces { get; }
    public int CurrentSurfaceIndex { get; set; }
    public uint Layer { get; set; }

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
            SDL.SDL_SetSurfaceBlendMode(Surfaces[i],
                SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetSurfaceAlphaMod(Surfaces[i], alpha);
        }
    }

    protected override void Update() => Texture.DrawSurface(Surfaces[CurrentSurfaceIndex], ref Transform, Layer);

    protected override void LateUpdate()
    {
    }

    public static Drawable Instantiate(DrawableManager manager, int screenSpaceCoordX, int screenSpaceCoordY,
        TexturePool pool,
        int startIndex, uint layer)
    {
        SDL.SDL_Rect parent = new() //Fix this: accept a different parent and try to separate logic from drawing
        {
            w = 16, h = 16, x = screenSpaceCoordX, y = screenSpaceCoordY
        };

        if (parent.x < Configuration.ControlsWidth)
        {
            return null;
        }

        if (parent.x > Configuration.WindowSizeX)
        {
            return null;
        }

        if (parent.y > Configuration.WindowSizeY)
        {
            return null;
        }

        if (parent.y < 0)
        {
            return null;
        }

        Drawable drawable = new Drawable(manager.Display, manager.Texture, ref parent, pool, startIndex, layer);
        manager.Drawables.Add(drawable);
        return drawable;
    }

    public static Drawable Instantiate(DrawableManager manager, int screenSpaceCoordX, int screenSpaceCoordY,
        IntPtr surface,
        int startIndex, uint layer)
    {
        SDL.SDL_Rect parent = new() //Fix this: accept a different parent and try to separate logic from drawing
        {
            w = 16, h = 16, x = screenSpaceCoordX, y = screenSpaceCoordY
        };

        if (parent.x < Configuration.ControlsWidth)
        {
            return null;
        }

        if (parent.x > Configuration.WindowSizeX)
        {
            return null;
        }

        if (parent.y > Configuration.WindowSizeY)
        {
            return null;
        }

        if (parent.y < 0)
        {
            return null;
        }

        Drawable drawable = new Drawable(manager.Display, manager.Texture, ref parent, surface, startIndex, layer);
        manager.Drawables.Add(drawable);
        return drawable;
    }
}
