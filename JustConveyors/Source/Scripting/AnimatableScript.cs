using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class AnimatableScript : IScript
{
    protected readonly Animatable Animatable;

    public AnimatableScript(Animatable animatable) => Animatable = animatable;

    public abstract void Start();

    public abstract void Update();

    public abstract void LateUpdate();

    public abstract void Close();
}
