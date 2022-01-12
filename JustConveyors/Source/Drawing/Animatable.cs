using System.Diagnostics;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Drawing;

internal class Animatable : Drawable
{
    private readonly Stopwatch _animationTimer = Stopwatch.StartNew();
    public bool IsAnimating;

    public Animatable(Display display, Texture texture, IEventHolder eventHolder, ref SDL.SDL_Rect transform,
        TexturePool pool,
        int startSurfaceIndex, uint layer, long frameIntervalMilliseconds, bool startState)
        : base(display, texture, eventHolder, ref transform, pool, startSurfaceIndex, layer)
    {
        FrameIntervalMilliseconds = frameIntervalMilliseconds;
        IsAnimating = startState;
    }

    public long FrameIntervalMilliseconds { get; private set; }

    public void Change(bool state, long intervalSeconds, bool restart)
    {
        IsAnimating = state;
        FrameIntervalMilliseconds = intervalSeconds;
        if (restart)
        {
            _animationTimer.Restart();
        }
    }

    protected override void Update()
    {
        if (IsAnimating && _animationTimer.ElapsedMilliseconds >= FrameIntervalMilliseconds)
        {
            CurrentSurfaceIndex++;
            _animationTimer.Restart();
        }

        if (CurrentSurfaceIndex >= Surfaces.Count)
        {
            CurrentSurfaceIndex = 0;
        }

        base.Update();
    }

    public static Animatable Instantiate(DrawableManager manager, int screenSpaceCoordX, int screenSpaceCoordY,
        TexturePool pool, int startIndex, long frameIntervalMilliseconds, bool startState, uint layer)
    {
        SDL.SDL_Rect parent = new() //Fix this: accept a different parent and try to separate logic from drawing
        {
            w = 16, h = 16, x = screenSpaceCoordX, y = screenSpaceCoordY
        };

        if (parent.x < Configuration.ControlsWidth)
        {
            return null;
        }

        if (parent.x > Configuration.WindowSizeX)
        {
            return null;
        }

        if (parent.y > Configuration.WindowSizeY)
        {
            return null;
        }

        if (parent.y < 0)
        {
            return null;
        }

        Animatable animatable = new(manager.Display, manager.Texture, manager.EventHolder, ref parent, pool, startIndex,
            layer,
            frameIntervalMilliseconds, startState);
        manager.Drawables.Add(animatable);
        return animatable;
    }
}
