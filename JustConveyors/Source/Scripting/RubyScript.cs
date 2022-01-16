using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal class RubyScript : DrawableScript
{
    private SDL.SDL_Rect _deltaRect;

    public RubyScript(Drawable drawable) : base(drawable)
    {
    }

    public bool IsStopped { get; private set; }

    public override void Start()
    {
    }

    //TODO: Offset the collision by 8px
    public override void Update()
    {
        Drawable underDrawable =
            Drawable.Manager.GetDrawable<ConveyorScript>(Drawable.WorldSpaceTileTransform, true);
        if (underDrawable is null)
        {
            IsStopped = true;
            return;
        }

        ConveyorScript underScript = underDrawable.Script as ConveyorScript;
        _deltaRect = underScript!.Direction.TryGetDeltaRect();

        IsStopped = Drawable.Manager.GetDrawable<RubyScript>(
            Drawable.Transform.TryGetAdjacentCoords(underScript.Direction, 16, 8), true) is not null;

        if (!IsStopped)
        {
            Drawable.Transform.x += (int)(_deltaRect.x * underScript.Speed * Time.DeltaTime);
            Drawable.Transform.y += (int)(_deltaRect.y * underScript.Speed * Time.DeltaTime);
        }
    }

    public override void Close()
    {
    }
}
