using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;

namespace JustConveyors.Source.Scripting;

internal abstract class DrawableScript : IScript
{
    protected readonly Drawable Drawable;
    protected readonly IEventHolder EventHolder;

    public DrawableScript(Drawable drawable, IEventHolder eventHolder)
    {
        Drawable = drawable;
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
