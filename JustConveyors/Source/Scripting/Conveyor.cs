using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class Conveyor : DrawableScript
{
    protected Conveyor(Drawable drawable) : base(drawable)
    {
    }
}
