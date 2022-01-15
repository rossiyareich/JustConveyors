using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Scripting;

internal class SourceScript : DrawableScript
{
    public SourceScript(Drawable drawable) : base(drawable)
    {
    }

    public override void Start()
    {
    }

    //TODO: Only pump out more when next conveyor is not clogged
    public override void Update()
    {
        int X = Drawable.Transform.x;
        int Y = Drawable.Transform.y;

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>(Drawable.Transform with { x = X + 16 }, true) is
                not
                null &&
            Drawable.Manager.GetDrawable<RubyScript>(Drawable.Transform with { x = X + 12 }, true) is null)
        {
            Drawable drawable = Drawable.Instantiate(Drawable.Manager, X + 12, Y, PoolResources.RubyPool, 0,
                6,
                TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable);
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>(Drawable.Transform with { x = X - 16 }, true) is
                not
                null &&
            Drawable.Manager.GetDrawable<RubyScript>(Drawable.Transform with { x = X - 12 }, true) is null)
        {
            Drawable drawable = Drawable.Instantiate(Drawable.Manager, X - 12, Y, PoolResources.RubyPool, 0, 6,
                TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable);
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>(Drawable.Transform with { y = Y + 16 }, true) is
                not
                null &&
            Drawable.Manager.GetDrawable<RubyScript>(Drawable.Transform with { y = Y + 12 }, true) is null)
        {
            Drawable drawable =
                Drawable.Instantiate(Drawable.Manager, X, Y + 12, PoolResources.RubyPool, 0, 6,
                    TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable);
        }

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>(Drawable.Transform with { y = Y - 16 }, true) is
                not
                null &&
            Drawable.Manager.GetDrawable<RubyScript>(Drawable.Transform with { y = Y - 12 }, true) is null)
        {
            Drawable drawable =
                Drawable.Instantiate(Drawable.Manager, X, Y - 12, PoolResources.RubyPool, 0, 6,
                    TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable);
        }
    }

    public override void Close()
    {
    }
}
