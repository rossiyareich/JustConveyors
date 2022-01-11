namespace JustConveyors.Renderer.Source.Rendering;

internal interface IRenderComponent : IDisposable
{
    Display _display { get; }
    void Load();
    void Cleanup();
}
