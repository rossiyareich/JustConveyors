using System.Diagnostics;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Conveyor;

internal class Aninmatable : Component
{
    private readonly List<nint> _textures;

    private readonly Stopwatch _animationTimer = Stopwatch.StartNew();
    private SDL.SDL_Rect _parentTransform;

    public Aninmatable(Display display, ref SDL.SDL_Rect parentTransform, long frameIntervalMilliseconds,
        params string[] imagePaths) : base(display)
    {
        _textures = new List<nint>();
        _parentTransform = parentTransform;
        FrameIntervalMilliseconds = frameIntervalMilliseconds;
        ImagePathList = new List<string>();
        ImagePathList.AddRange(imagePaths);
    }

    public bool IsAnimating { get; set; } = true;
    public List<string> ImagePathList { get; }

    public long FrameIntervalMilliseconds { get; }
    public int CurrentAnimationIndex { get; private set; }

    protected override void Start()
    {
        SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
        foreach (string imagePath in ImagePathList)
        {
            _textures.Add(SDL_image.IMG_LoadTexture(Display.SDLRenderer, imagePath));
        }

        SDL_image.IMG_Quit();
    }

    protected override void Update()
    {
        if (IsAnimating && _animationTimer.ElapsedMilliseconds >= FrameIntervalMilliseconds)
        {
            CurrentAnimationIndex++;
            _animationTimer.Restart();
        }

        if (CurrentAnimationIndex >= _textures.Count)
        {
            CurrentAnimationIndex = 0;
        }

        SDL.SDL_RenderCopy(Display.SDLRenderer, _textures[CurrentAnimationIndex], IntPtr.Zero,
            ref _parentTransform);
    }

    protected override void LateUpdate()
    {
    }

    protected override void Close()
    {
        base.Close();
        foreach (nint texture in _textures)
        {
            if (texture != 0)
            {
                SDL.SDL_DestroyTexture(texture);
            }
        }
    }
}
