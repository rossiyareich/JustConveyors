using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Loop;

internal abstract class Component
{
    private readonly IEventHolder _eventHolder;

    public Component(Display display, Texture texture, IEventHolder eventHolder)
    {
        Display = display;
        Texture = texture;
        _eventHolder = eventHolder;
        _eventHolder.OnStart += Start;
        _eventHolder.OnUpdate += Update;
        _eventHolder.OnLateUpdate += LateUpdate;
        _eventHolder.OnClose += Close;
    }

    protected Display Display { get; }
    protected Texture Texture { get; }
    protected abstract void Start();
    protected abstract void Update();
    protected abstract void LateUpdate();

    public virtual void Close()
    {
        _eventHolder.OnStart -= Start;
        _eventHolder.OnUpdate -= Update;
        _eventHolder.OnLateUpdate -= LateUpdate;
        _eventHolder.OnClose -= Close;
    }
}
