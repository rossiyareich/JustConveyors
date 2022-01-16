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
        for (int i = 1; i <= 4; i++)
        {
            Pump((TransformFlags)i, 12);
        }
    }

    public void Pump(TransformFlags direction, int rubyDrawOffset)
    {
        int X = direction.TryGetDeltaRect().x * rubyDrawOffset + Drawable.Transform.x;
        int Y = direction.TryGetDeltaRect().y * rubyDrawOffset + Drawable.Transform.y;

        if (Drawable.Manager.GetDrawable<TitaniumConveyorBaseScript>(
                Drawable.Transform.TryGetAdjacentCoords(direction, 16, 16), true) is not null &&
            Drawable.Manager.GetDrawable<RubyScript>(
                    Drawable.Transform.TryGetAdjacentCoords(direction, 8, rubyDrawOffset),
                    true) is
                null)
        {
            Drawable drawable = Drawable.Instantiate(Drawable.Manager, X, Y, PoolResources.RubyPool,
                0,
                6,
                TransformFlags.IndexZero);
            drawable.Script = new RubyScript(drawable);
        }
    }

    public override void Close()
    {
    }
}
