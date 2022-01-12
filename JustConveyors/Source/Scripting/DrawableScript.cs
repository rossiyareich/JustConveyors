using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class DrawableScript : IScript
{
    protected readonly Drawable Drawable;

    public DrawableScript(Drawable drawable) => Drawable = drawable;

    public abstract void Start();

    public abstract void Update();

    public abstract void LateUpdate();

    public abstract void Close();
}
