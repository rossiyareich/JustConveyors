using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal abstract class AnimatableScript : IScript
{
    public readonly Animatable Animatable;

    public AnimatableScript(Animatable animatable) => Animatable = animatable;

    public abstract void Start();

    public abstract void Update();

    public virtual void LateUpdate()
    {
        if (!Coordinates.RectIsInScrSpaceBounds(Animatable.Transform))
        {
            Animatable.Close();
        }
    }

    public abstract void Close();
}
