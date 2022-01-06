using JustConveyors.Source.Conveyor;
using JustConveyors.Source.Rendering;
using SDL2;

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
    ///     Put all component initializations here
    /// </remarks>
    public void InitializeComponents(Display display)
    {
        var parent = new SDL.SDL_Rect() { w = 16, h = 16, x = 128, y = 128 };
        new Aninmatable(display, ref parent, 100, @"Resources\Assets\junction\output (Junction) 0.png");
    }
}
