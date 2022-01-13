using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class AnimatableScript : IScript
{
    protected readonly Animatable Animatable;

    public AnimatableScript(Animatable animatable) => Animatable = animatable;

    public abstract void Start();

    public abstract void Update();

    public virtual void LateUpdate()
    {
        if (Animatable.Transform.x > Configuration.WindowSizeX ||
            Animatable.Transform.x < Configuration.ControlsWidth ||
            Animatable.Transform.y > Configuration.WindowSizeY || Animatable.Transform.y < 0)
            Animatable.Close();
    }

    public abstract void Close();
}
