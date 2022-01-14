using JustConveyors.Source.Drawing;
using JustConveyors.Source.Scripting;
using SDL2;

namespace JustConveyors.Source.Loop;

internal partial class DrawableManager
{
    public IEnumerable<Drawable> GetDrawables(SDL.SDL_Rect screenLocation) =>
        Drawables.Where(x => x.Transform.Equals(screenLocation));

    public Drawable GetDrawable(SDL.SDL_Rect screenLocation, bool first) => first
        ? GetDrawables(screenLocation).FirstOrDefault()
        : GetDrawables(screenLocation).LastOrDefault();

    public IEnumerable<Drawable> GetDrawables(SDL.SDL_Rect screenLocation, Drawable exclude) =>
        Drawables.Where(x => x.Transform.Equals(screenLocation) && x != exclude);

    public Drawable GetDrawable(SDL.SDL_Rect screenLocation, Drawable exclude, bool first) => first
        ? GetDrawables(screenLocation, exclude).FirstOrDefault()
        : GetDrawables(screenLocation, exclude).LastOrDefault();

    public IEnumerable<Drawable> GetDrawables<TScript>(SDL.SDL_Rect screenLocation) where TScript : IScript =>
        Drawables.Where(x => x.Transform.Equals(screenLocation) && x.Script is TScript);

    public IEnumerable<Drawable> GetDrawables<TScript>(params SDL.SDL_Rect[] screenLocations) where TScript : IScript =>
        Drawables.Where(x => screenLocations.Any(y => y.Equals(x.Transform)) && x.Script is TScript);

    public IEnumerable<Drawable> GetDrawables<TScript>(SDL.SDL_Rect screenLocation, Drawable exclude)
        where TScript : IScript =>
        Drawables.Where(x => x.Transform.Equals(screenLocation) && x != exclude && x.Script is TScript);

    public Drawable GetDrawable<TScript>(SDL.SDL_Rect screenLocation, bool first) where TScript : IScript => first
        ? GetDrawables<TScript>(screenLocation).FirstOrDefault()
        : GetDrawables<TScript>(screenLocation).LastOrDefault();

    public Drawable GetDrawable<TScript>(SDL.SDL_Rect screenLocation, Drawable exclude, bool first)
        where TScript : IScript => first
        ? GetDrawables<TScript>(screenLocation, exclude).FirstOrDefault()
        : GetDrawables<TScript>(screenLocation, exclude).LastOrDefault();
}
