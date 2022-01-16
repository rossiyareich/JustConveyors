using JustConveyors.Source.Drawing;
using JustConveyors.Source.UI;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal abstract class ConveyorScript : DrawableScript
{
    protected ConveyorScript(Drawable drawable) : base(drawable)
    {
    }

    public float Speed { get; protected set; }
    public TransformFlags Direction { get; protected set; }
    public bool IsClogged => Drawable.Manager.GetDrawables<RubyScript>(Drawable.WorldSpaceTileTransform).Count() >= 2;

    public bool IsEndOfLine =>
        Drawable.Manager.GetDrawable<ConveyorScript>(Drawable.Transform.TryGetAdjacentCoords(Direction, 16, 16), true)
            is null;

    public bool IsStopRuby(SDL.SDL_Rect rect) =>
        IsEndOfLine && IsClogged &&
        rect.TryGetRubyOffset(Direction.GetOppositeFour(), 16).Equals(Drawable.Transform);

    public bool IsAllowedInDirection(TransformFlags direction)
    {
        if (!IsClogged)
        {
            return true;
        }

        return false;
    }

    public override void Close()
    {
        IEnumerable<Drawable> rubies = Drawable.Manager.GetDrawables<RubyScript>(Drawable.WorldSpaceTileTransform)
            .ToList();
        foreach (Drawable ruby in rubies)
        {
            ruby.BaseClose();
        }

        Drawable.Manager.Drawables.RemoveAll(x => rubies.Any(y => y == x));
    }
}
