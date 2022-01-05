using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Loop;

internal abstract class Component
{
    public Component(Display display) => Display = display;
    protected Display Display { get; }

    protected abstract void Start();
    protected abstract void Update();
    protected abstract void LateUpdate();
    protected abstract void Close();
}
