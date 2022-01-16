using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class DrawableScript : IScript
{
    public readonly Drawable Drawable;

    public DrawableScript(Drawable drawable) => Drawable = drawable;

    public abstract void Start();

    public abstract void Update();

    public virtual void LateUpdate()
    {
        if (!Coordinates.RectIsInScrSpaceBounds(Drawable.Transform))
        {
            Drawable.Close();
        }
    }

    public abstract void Close();
}
