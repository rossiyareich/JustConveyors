using JustConveyors.Source.ConfigurationNS;
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

    public Drawable InstantiateDrawable<T>(int x, int y, TexturePool pool, int startIndex) where T : Drawable
    {
        SDL.SDL_Rect parent = new() { w = 16, h = 16, x = x, y = y };

        if (typeof(T) == typeof(Animatable))
        {
            return new Animatable(_display, _texture, ref parent, pool, startIndex, 100);
        }
        else if (typeof(T) == typeof(Drawable))
        {
            return new Drawable(_display, _texture, ref parent, pool, startIndex);
        }

        throw new TypeInitializationException(nameof(T), new Exception($"Generic type is not a {nameof(T)}"));
    }

    public void InitializeComponents(Display display, Texture texture)
    {
        _display = display;
        _texture = texture;
        PoolResources = PoolResources.GetInstance();

        //for (int i = Configuration.ControlsWidth; i < Configuration.WindowSizeX; i += 16)
        //{
        //    for (int j = 0; j < Configuration.WindowSizeY; j += 16)
        //    {
        //        SDL.SDL_Rect parent = new() { w = 16, h = 16, x = i, y = j };
        //        InstantiateDrawable<Drawable>(i, j, PoolResources.JunctionPool, 0);
        //    }
        //}
    }
}
