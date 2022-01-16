using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Scripting;

internal static class ScriptHelper
{
    public static Type[] NonInteractableScriptTypes { get; } =
    {
        typeof(RubyScript), typeof(GuideScript), typeof(ArmoredConveyorArwScript),
        typeof(BridgeConveyorBridgeScript), typeof(ThoriumConveyorArwScript), typeof(TitaniumConveyorArwScript)
    };

    public static void DrawGuideLines(DrawableManager manager, (int X, int Y) start, (int X, int Y) end,
        TransformFlags hv)
    {
        if (hv is TransformFlags.Horizontal)
        {
            for (int i = start.X; i < end.X; i += 16)
            {
                Drawable drawable = Drawable.Instantiate(manager, i, start.Y, PoolResources.GuidePool, 0, 3, hv);
                drawable.Script = new GuideScript(drawable);
            }
        }
        else if (hv is TransformFlags.Vertical)
        {
            for (int i = start.Y; i < end.Y; i += 16)
            {
                Drawable drawable = Drawable.Instantiate(manager, start.X, i, PoolResources.GuidePool, 0, 3, hv);
                drawable.Script = new GuideScript(drawable);
            }
        }
    }

    public static void DestroyGuideLines(DrawableManager manager, (int X, int Y) start, (int X, int Y) end,
        TransformFlags hv)
    {
        if (hv is TransformFlags.Horizontal)
        {
            for (int i = start.X; i < end.X; i += 16)
            {
                manager.GetDrawable<GuideScript>(new SDL.SDL_Rect { w = 16, h = 16, x = i, y = start.Y }, false)
                    ?.Close();
            }
        }
        else if (hv is TransformFlags.Vertical)
        {
            for (int i = start.Y; i < end.Y; i += 16)
            {
                manager.GetDrawable<GuideScript>(new SDL.SDL_Rect { w = 16, h = 16, x = start.X, y = i }, false)
                    ?.Close();
            }
        }
    }

    public static void DrawArrow(DrawableManager manager, (int X, int Y) position, TexturePool arrow,
        TransformFlags direction)
    {
        Drawable drawable = Drawable.Instantiate(manager, position.X, position.Y, arrow, 0, 4, direction);
        drawable.Script = new ArrowScript(drawable);
    }

    public static void TryDestroyArrow(DrawableManager manager, (int X, int Y) position) =>
        manager.GetDrawable<ArrowScript>(new SDL.SDL_Rect { w = 16, h = 16, x = position.X, y = position.Y }, false)
            ?.Close();

    public static SDL.SDL_Rect TryGetAdjacentCoords(this SDL.SDL_Rect rect, TransformFlags fourDirection, int wh,
        int offset) =>
        fourDirection switch
        {
            TransformFlags.DirN => new SDL.SDL_Rect { h = wh, w = wh, x = rect.x, y = rect.y - offset },
            TransformFlags.DirS => new SDL.SDL_Rect { h = wh, w = wh, x = rect.x, y = rect.y + offset },
            TransformFlags.DirE => new SDL.SDL_Rect { h = wh, w = wh, x = rect.x + offset, y = rect.y },
            TransformFlags.DirW => new SDL.SDL_Rect { h = wh, w = wh, x = rect.x - offset, y = rect.y },
            _ => throw new Exception("Unsupported direction")
        };

    public static SDL.SDL_Rect TryGetDeltaRect(this TransformFlags direction) =>
        direction switch
        {
            TransformFlags.DirN => new SDL.SDL_Rect { x = 0, y = -1 },
            TransformFlags.DirS => new SDL.SDL_Rect { x = 0, y = 1 },
            TransformFlags.DirE => new SDL.SDL_Rect { x = 1, y = 0 },
            TransformFlags.DirW => new SDL.SDL_Rect { x = -1, y = 0 },
            _ => throw new Exception("Unsupported direction")
        };
}
