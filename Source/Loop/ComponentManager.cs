using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Loop;

internal class ComponentManager
{
    public ComponentManager() => Components = new List<Component>();

    public List<Component> Components { get; }

    public T AddComponent<T>(T component) where T : Component
    {
        Components.Add(component);
        return component;
    }

    /// <remarks>
    /// Put all component initializations here
    /// </remarks>
    public void InitializeComponents(Display display)
    {
    }
}
