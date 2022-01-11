using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Loop;

internal class DrawableManager : IDisposable
{
    public readonly Display Display;

    public readonly Texture Texture;

    public DrawableManager(Display display, Texture texture)
    {
        Drawables = new List<Drawable>();
        Display = display;
        Texture = texture;
        PoolResources = PoolResources.GetInstance();
    }

    public PoolResources PoolResources { get; }
    public List<Drawable> Drawables { get; set; }

    public void Dispose() => PoolResources.Dispose();
}
