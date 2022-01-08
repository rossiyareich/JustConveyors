using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Loop;

internal class ComponentManager : IDisposable
{
    private Display _display;

    private Texture _texture;
    public ComponentManager() => Components = new List<Component>();
    public PoolResources PoolResources { get; private set; }

    public List<Component> Components { get; }

    public void Dispose() => PoolResources.Dispose();

    public T AddComponent<T>(T component) where T : Component
    {
        Components.Add(component);
        return component;
    }

    public Component InstantiateDrawable<T>(int x, int y, TexturePool pool, int startIndex) where T : Component
    {
        SDL.SDL_Rect parent = new() { w = 16, h = 16, x = x, y = y };

        if (typeof(T) == typeof(Animatable))
        {
            return new Animatable(_display, _texture, ref parent, pool, startIndex, 100);
        }

        if (typeof(T) == typeof(Drawable))
        {
            return new Drawable(_display, _texture, ref parent, pool, startIndex);
        }

        throw new TypeInitializationException(nameof(T), new Exception("Generic type is not a drawable"));
    }

    public void InitializeComponents(Display display, Texture texture)
    {
        _display = display;
        _texture = texture;
        PoolResources = PoolResources.GetInstance();
    }
}
