using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class ConveyorScript : DrawableScript
{
    public float Speed { get; protected set; }
    public TransformFlags Direction { get; protected set; }

    protected ConveyorScript(Drawable drawable) : base(drawable)
    {
    }

    public override void Close()
    {
        var rubies = Drawable.Manager.GetDrawables<RubyScript>(Drawable.WorldSpaceTileTransform);
        foreach (var ruby in rubies)
        {
            ruby.CloseStateless();
        }

        Drawable.Manager.Drawables.RemoveAll(x => rubies.Any(y => y == x));
    }
}
