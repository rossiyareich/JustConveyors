namespace JustConveyors.Source.Loop;

public interface IEventHolder
{
    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnLateUpdate;
    public event Action OnClose;
}
