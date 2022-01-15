using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal class RubyScript : DrawableScript
{
    private SDL.SDL_Rect _deltaRect;

    public RubyScript(Drawable drawable) : base(drawable)
    {
    }

    public bool IsStopped { get; private set; }

    public override void Start()
    {
    }

    public override void Update()
    {
        Drawable underDrawable = Drawable.Manager.GetDrawable<ConveyorScript>(Drawable.WorldSpaceTileTransform, true);
        if (underDrawable is null)
        {
            IsStopped = true;
            return;
        }

        ConveyorScript underScript = underDrawable.Script as ConveyorScript;
        _deltaRect = underScript.Direction switch
        {
            TransformFlags.DirN => new SDL.SDL_Rect { x = 0, y = -1 },
            TransformFlags.DirS => new SDL.SDL_Rect { x = 0, y = 1 },
            TransformFlags.DirE => new SDL.SDL_Rect { x = 1, y = 0 },
            TransformFlags.DirW => new SDL.SDL_Rect { x = -1, y = 0 },
            _ => throw new Exception("Unsupported direction")
        };

        SDL.SDL_Rect frontRect = underScript.Direction switch
        {
            TransformFlags.DirN => new SDL.SDL_Rect
            {
                h = 16, w = 16, x = Drawable.Transform.x, y = Drawable.Transform.y - 8
            },
            TransformFlags.DirS => new SDL.SDL_Rect
            {
                h = 16, w = 16, x = Drawable.Transform.x, y = Drawable.Transform.y + 8
            },
            TransformFlags.DirE => new SDL.SDL_Rect
            {
                h = 16, w = 16, x = Drawable.Transform.x + 8, y = Drawable.Transform.y
            },
            TransformFlags.DirW => new SDL.SDL_Rect
            {
                h = 16, w = 16, x = Drawable.Transform.x - 8, y = Drawable.Transform.y
            },
            _ => throw new Exception("Unsupported direction")
        };

        IsStopped = Drawable.Manager.GetDrawable<RubyScript>(frontRect, true) is not null;

        if (!IsStopped)
        {
            Drawable.Transform.x += (int)(_deltaRect.x * underScript.Speed * Time.DeltaTime);
            Drawable.Transform.y += (int)(_deltaRect.y * underScript.Speed * Time.DeltaTime);
        }
    }

    public override void Close()
    {
    }
}
