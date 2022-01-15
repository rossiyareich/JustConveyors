﻿using JustConveyors.Source.Drawing;
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
}
