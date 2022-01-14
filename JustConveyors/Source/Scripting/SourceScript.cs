using System.Diagnostics;
using System.Drawing;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal class SourceScript : DrawableScript
{
    public SourceScript(Drawable drawable) : base(drawable)
    {
    }

    public override void Start()
    {
    }

    //TODO: There's some weird rounding error here that's causing a weird behaviour on rubies spawned on conveyors
    public override void Update()
    {
        var x = Drawable.WorldSpaceTileTransform.X;
        var y = Drawable.WorldSpaceTileTransform.Y;
        var X = Drawable.Transform.x;
        var Y = Drawable.Transform.y;

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>((x + 1, y), true) is not null &&
            Drawable.Manager.GetDrawable<RubyScript>((x + 1, y), true) is null)
        {
            var drawable = Drawable.Instantiate(Drawable.Manager, X + 12, Y, PoolResources.RubyPool, 0,
                6,
                TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable, new SDL.SDL_Rect() { x = 1 });
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>((x - 1, y), true) is not null &&
            Drawable.Manager.GetDrawable<RubyScript>((x - 1, y), true) is null)
        {
            var drawable = Drawable.Instantiate(Drawable.Manager, X - 12, Y, PoolResources.RubyPool, 0, 6,
                TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable, new SDL.SDL_Rect() { x = -1 });
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>((x, y + 1), true) is not null &&
            Drawable.Manager.GetDrawable<RubyScript>((x, y + 1), true) is null)
        {
            var drawable =
                Drawable.Instantiate(Drawable.Manager, X, Y + 12, PoolResources.RubyPool, 0, 6,
                    TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable, new SDL.SDL_Rect() { y = 1 });
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>((x, y - 1), true) is not null &&
            Drawable.Manager.GetDrawable<RubyScript>((x, y - 1), true) is null)
        {
            var drawable =
                Drawable.Instantiate(Drawable.Manager, X, Y - 12, PoolResources.RubyPool, 0, 6,
                    TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable, new SDL.SDL_Rect() { y = -1 });
        }
    }

    public override void Close()
    {
    }
}
