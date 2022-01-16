using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class ConveyorScript : DrawableScript
{
    protected ConveyorScript(Drawable drawable) : base(drawable)
    {
    }

    public float Speed { get; protected set; }
    public TransformFlags Direction { get; protected set; }

    public override void Close()
    {
        IEnumerable<Drawable> rubies = Drawable.Manager.GetDrawables<RubyScript>(Drawable.WorldSpaceTileTransform)
            .ToList();
        foreach (Drawable ruby in rubies)
        {
            ruby.CloseStateless();
        }
        //TODO: Clear by clearing the main texture, not calling close on each


        Drawable.Manager.Drawables.RemoveAll(x => rubies.Any(y => y == x));
    }
}
