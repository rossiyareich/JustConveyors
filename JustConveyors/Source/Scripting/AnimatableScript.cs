using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;

namespace JustConveyors.Source.Scripting;

internal abstract class AnimatableScript : IScript
{
    protected readonly Animatable Animatable;
    protected readonly IEventHolder EventHolder;

    public AnimatableScript(Animatable animatable, IEventHolder eventHolder)
    {
        Animatable = animatable;
        EventHolder = eventHolder;
        EventHolder.OnStart += Start;
        EventHolder.OnUpdate += Update;
        EventHolder.OnLateUpdate += LateUpdate;
        EventHolder.OnClose += Close;
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    public void LateUpdate()
    {
    }

    public virtual void Close()
    {
        EventHolder.OnStart -= Start;
        EventHolder.OnUpdate -= Update;
        EventHolder.OnLateUpdate -= LateUpdate;
        EventHolder.OnClose -= Close;
    }
}
