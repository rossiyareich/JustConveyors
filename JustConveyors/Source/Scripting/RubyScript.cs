using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal class RubyScript : DrawableScript
{
    private readonly SDL.SDL_Rect _deltaRect;

    public RubyScript(Drawable drawable, SDL.SDL_Rect deltaRect) : base(drawable)
    {
        _deltaRect.x = deltaRect.x;
        _deltaRect.y = deltaRect.y;
    }

    public override void Start()
    {
    }

    public override void Update()
    {
        Drawable.Transform.x += (int)(_deltaRect.x * 300f * Time.DeltaTime);
        Drawable.Transform.y += (int)(_deltaRect.y * 300f * Time.DeltaTime);
    }

    public override void Close()
    {
    }
}
