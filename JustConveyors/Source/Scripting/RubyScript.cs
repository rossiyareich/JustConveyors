using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal class RubyScript : DrawableScript
{
    private SDL.SDL_Rect _deltaRect;
    private Drawable _realDrawable;
    private TransformFlags _direction;

    public RubyScript(Drawable drawable, TransformFlags direction) : base(drawable)
    {
        _direction = direction;
    }

    public bool IsStopped { get; private set; }

    public override void Start()
    {
        _realDrawable =
            Drawable.Manager.GetDrawable<ConveyorScript>(Drawable.Transform.TryGetAdjacentCoords(_direction, 16, 4),
                false);
    }

    public override void Update()
    {
        _realDrawable =
            Drawable.Manager.GetDrawable<ConveyorScript>(Drawable.Transform.TryGetRubyOffset(_direction, 16),
                false) ?? _realDrawable;


        ConveyorScript realScript = _realDrawable?.Script as ConveyorScript;

        if (realScript!.IsStopRuby(Drawable.Transform))
        {
            IsStopped = true;
            return;
        }

        _deltaRect = realScript!.Direction.TryGetDeltaRect();

        IsStopped = Drawable.Manager.GetDrawable<RubyScript>(
            Drawable.Transform.TryGetAdjacentCoords(realScript.Direction, 16, 8), true) is not null;

        if (!IsStopped)
        {
            Drawable.Transform.x += (int)(_deltaRect.x * realScript.Speed * (Time.DeltaTime == 0f ? 0f : 1f));
            Drawable.Transform.y += (int)(_deltaRect.y * realScript.Speed * (Time.DeltaTime == 0f ? 0f : 1f));
        }
    }

    public override void Close()
    {
    }
}
