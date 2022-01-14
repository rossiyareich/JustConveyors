using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;

namespace JustConveyors.Source.Loop;

internal class DrawableManager : IDisposable
{
    public readonly Display Display;
    public readonly IEventHolder EventHolder;

    public readonly Texture Texture;

    public DrawableManager(Display display, Texture texture, IEventHolder eventHolder)
    {
        Drawables = new List<Drawable>();
        Display = display;
        Texture = texture;
        EventHolder = eventHolder;
        PoolResources = PoolResources.GetInstance();
    }

    public PoolResources PoolResources { get; }
    public List<Drawable> Drawables { get; set; }

    public void Dispose() => PoolResources.Dispose();

    public IEnumerable<Drawable> GetDrawables((int X, int Y) gridLocation) =>
        Drawables.Where(x => x.WorldSpaceTileTransform == gridLocation);

    public Drawable GetDrawable((int X, int Y) gridLocation, bool first) => first
        ? GetDrawables(gridLocation).FirstOrDefault()
        : GetDrawables(gridLocation).LastOrDefault();

    public IEnumerable<Drawable> GetDrawables((int X, int Y) gridLocation, Drawable exclude) =>
        Drawables.Where(x => x.WorldSpaceTileTransform == gridLocation && x != exclude);

    public Drawable GetDrawable((int X, int Y) gridLocation, Drawable exclude, bool first) => first
        ? GetDrawables(gridLocation, exclude).FirstOrDefault()
        : GetDrawables(gridLocation, exclude).LastOrDefault();

    public IEnumerable<Drawable> GetAllDrawables<TScript>() where TScript : IScript =>
        Drawables.Where(x => x.Script is TScript);

    public IEnumerable<Drawable> GetAllDrawables<TScript>(Drawable exclude) where TScript : IScript =>
        Drawables.Where(x => x.Script is TScript && x != exclude);

    public IEnumerable<Drawable> GetDrawables<TScript>((int X, int Y) gridLocation) where TScript : IScript =>
        Drawables.Where(x => x.WorldSpaceTileTransform == gridLocation && x.Script is TScript);

    public IEnumerable<Drawable> GetDrawables<TScript>(params (int X, int Y)[] gridLocations) where TScript : IScript =>
        Drawables.Where(x => gridLocations.Any(y => y == x.WorldSpaceTileTransform) && x.Script is TScript);

    public IEnumerable<Drawable> GetDrawables<TScript>((int X, int Y) gridLocation, Drawable exclude)
        where TScript : IScript =>
        Drawables.Where(x => x.WorldSpaceTileTransform == gridLocation && x != exclude && x.Script is TScript);

    public Drawable GetDrawable<TScript>((int X, int Y) gridLocation, bool first) where TScript : IScript => first
        ? GetDrawables<TScript>(gridLocation).FirstOrDefault()
        : GetDrawables<TScript>(gridLocation).LastOrDefault();

    public Drawable GetDrawable<TScript>((int X, int Y) gridLocation, Drawable exclude, bool first)
        where TScript : IScript => first
        ? GetDrawables<TScript>(gridLocation, exclude).FirstOrDefault()
        : GetDrawables<TScript>(gridLocation, exclude).LastOrDefault();
}
