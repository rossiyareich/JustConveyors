using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;
using SDL2;

namespace JustConveyors.Source.Drawing;

internal class Drawable : Component
{
    protected IScript _script;
    public SDL.SDL_Rect Transform;

    protected Drawable(Display display, Texture texture, IEventHolder eventHolder, ref SDL.SDL_Rect transform,
        TexturePool pool,
        int startSurfaceIndex, uint layer)
        : base(display, texture, eventHolder)
    {
        Surfaces = pool.Surfaces;
        Transform = transform;
        CurrentSurfaceIndex = startSurfaceIndex;
        Layer = layer;
        ParentPool = pool;
        SetSurface(startSurfaceIndex);
    }

    /// <summary>
    /// </summary>
    /// <param name="display"></param>
    /// <param name="texture"></param>
    /// <param name="eventHolder"></param>
    /// <param name="transform"></param>
    /// <param name="pool"></param>
    /// <param name="startSurfaceIndex">startSurfaceIndex is ignored.</param>
    /// <param name="layer"></param>
    /// <param name="startRotation"></param>
    public Drawable(Display display, Texture texture, IEventHolder eventHolder, ref SDL.SDL_Rect transform,
        TexturePool pool,
        int startSurfaceIndex, uint layer, TransformFlags startRotation)
        : this(display, texture, eventHolder, ref transform, pool, startSurfaceIndex, layer) =>
        SetSurface(startRotation);

    public Drawable(Display display, Texture texture, IEventHolder eventHolder, ref SDL.SDL_Rect transform,
        IntPtr surface,
        int startSurfaceIndex, uint layer)
        : base(display, texture, eventHolder)
    {
        Surfaces = new List<(IntPtr, TransformFlags)>(1) { (surface, TransformFlags.None) };
        Transform = transform;
        CurrentSurfaceIndex = startSurfaceIndex;
        Layer = layer;
        SetSurface(startSurfaceIndex);
    }

    public DrawableManager Manager { get; init; }

    public TexturePool ParentPool { get; }

    public (int X, int Y) WorldSpaceTileTransform => Coordinates.GetWorldSpaceTile(Transform);

    public TransformFlags Rotation { get; protected set; }

    public List<(IntPtr surface, TransformFlags flag)> Surfaces { get; }
    public int CurrentSurfaceIndex { get; protected set; }
    public uint Layer { get; set; }

    public IScript Script
    {
        get => _script;
        set
        {
            _script?.Close();
            _script = value;
            _script?.Start();
        }
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
            SDL.SDL_SetSurfaceBlendMode(Surfaces[i].surface,
                SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetSurfaceAlphaMod(Surfaces[i].surface, alpha);
        }
    }

    public void SetSurface(TransformFlags flag)
    {
        if (flag != TransformFlags.IndexZero)
        {
            Rotation = flag;
            CurrentSurfaceIndex = Surfaces.FindIndex(x => x.flag == flag);
        }
        else
        {
            Rotation = Surfaces[0].flag;
            CurrentSurfaceIndex = 0;
        }
    }

    public void SetSurface(int index)
    {
        CurrentSurfaceIndex = index;
        Rotation = Surfaces[index].flag;
    }

    protected override void Update()
    {
        _script?.Update();
        Texture.DrawSurface(Surfaces[CurrentSurfaceIndex].surface, ref Transform, Layer);
    }

    protected override void LateUpdate() => _script?.LateUpdate();

    public static Drawable Instantiate(DrawableManager manager, int screenSpaceCoordX, int screenSpaceCoordY,
        TexturePool pool,
        int startIndex, uint layer, TransformFlags startRotation)
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

        Drawable drawable = new(manager.Display, manager.Texture, manager.EventHolder, ref parent, pool, startIndex,
            layer, startRotation) { Manager = manager };
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

        Drawable drawable = new(manager.Display, manager.Texture, manager.EventHolder, ref parent, surface,
            startIndex, layer) { Manager = manager };
        manager.Drawables.Add(drawable);
        return drawable;
    }

    public override void Close()
    {
        Script = null;
        base.Close();
    }
}
