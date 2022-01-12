namespace JustConveyors.Source.Loop;

internal interface IScriptHolder<T> : IEventHolder
{
    T Script { get; set; }
}
