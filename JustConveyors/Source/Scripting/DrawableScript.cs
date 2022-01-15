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
        if (Drawable.Transform.x > Configuration.WindowSizeX ||
            Drawable.Transform.x < Configuration.ControlsWidth ||
            Drawable.Transform.y > Configuration.WindowSizeY || Drawable.Transform.y < 0)
        {
            Drawable.Close();
        }
    }

    public abstract void Close();
}
