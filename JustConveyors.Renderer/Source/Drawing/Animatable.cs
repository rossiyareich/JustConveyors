using System.Diagnostics;
using JustConveyors.Renderer.Source.Rendering;
using SDL2;

namespace JustConveyors.Renderer.Source.Drawing;

internal class Animatable : Drawable
{
    private readonly Stopwatch _animationTimer = Stopwatch.StartNew();

    public Animatable(Display display, Texture texture, ref SDL.SDL_Rect transform, TexturePool pool, int startIndex,
        long frameIntervalMilliseconds) : base(display, texture, ref transform, pool, startIndex) =>
        FrameIntervalMilliseconds = frameIntervalMilliseconds;

    public bool IsAnimating { get; private set; } = true;
    public long FrameIntervalMilliseconds { get; private set; }

    protected override void Start()
    {
    }

    public void ChangeAnimation(bool state, long intervalSeconds, bool restart)
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
            CurrentIndex++;
            _animationTimer.Restart();
        }

        if (CurrentIndex >= _surfaces.Count)
        {
            CurrentIndex = 0;
        }

        base.Update();
    }

    protected override void LateUpdate()
    {
    }
}
