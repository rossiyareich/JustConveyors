using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal class TitaniumConveyorBaseScript : ConveyorScript
{
    public TitaniumConveyorBaseScript(Drawable drawable) : base(drawable)
    {
        Speed = 300f;
        Direction = Drawable.Rotation;
    }

    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override void Close()
    {
        base.Close();
    }
}
