using JustConveyors.Source.Conveyor;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Loop;

internal class ComponentManager : IDisposable
{
    public ComponentManager() => Components = new List<Component>();
    public PoolResources PoolResources { get; private set; }

    public List<Component> Components { get; }

    private Texture _texture;
    private Display _display;

    public void Dispose() => PoolResources.Dispose();

    public T AddComponent<T>(T component) where T : Component
    {
        Components.Add(component);
        return component;
    }

    public void InstantiateAnimatable(TexturePool obj, int ms, int x, int y)
    {
        SDL.SDL_Rect parent = new() { w = 16, h = 16, x = x, y = y };
        _ = new Aninmatable(_display, _texture, ref parent, ms, obj);
    }

    /// <remarks>
    ///     Put all component initializations here
    /// </remarks>
    public void InitializeComponents(Display display, Texture texture)
    {
        _display = display;
        _texture = texture;

        PoolResources = PoolResources.GetInstance();

        //for (int i = 320; i < 1920; i += 16)
        //{
        //    for (int j = 0; j < 896; j += 16)
        //    {
        //        SDL.SDL_Rect parent = new() { w = 16, h = 16, x = i, y = j };
        //        _ = new Aninmatable(_display, _texture, ref parent, 100, PoolResources.JunctionPool);
        //    }
        //}
    }
}
