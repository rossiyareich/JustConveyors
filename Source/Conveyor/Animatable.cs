using System.Diagnostics;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Conveyor;

internal class Aninmatable : Component
{
    private readonly Stopwatch _animationTimer = Stopwatch.StartNew();
    private readonly List<nint> _surfaces;
    private SDL.SDL_Rect _parentTransform;

    public Aninmatable(Display display, Texture texture, ref SDL.SDL_Rect parentTransform,
        long frameIntervalMilliseconds,
        TexturePool pool) : base(display, texture)
    {
        _surfaces = pool.Surfaces;
        _parentTransform = parentTransform;
        FrameIntervalMilliseconds = frameIntervalMilliseconds;
    }

    public bool IsAnimating { get; set; } = true;
    public long FrameIntervalMilliseconds { get; }
    public int CurrentAnimationIndex { get; private set; }

    protected override void Start()
    {
    }

    protected override void Update()
    {
        if (IsAnimating && _animationTimer.ElapsedMilliseconds >= FrameIntervalMilliseconds)
        {
            CurrentAnimationIndex++;
            _animationTimer.Restart();
        }

        if (CurrentAnimationIndex >= _surfaces.Count)
        {
            CurrentAnimationIndex = 0;
        }


        Texture.DrawSurface(_surfaces[CurrentAnimationIndex], ref _parentTransform);
    }

    protected override void LateUpdate()
    {
    }
}
