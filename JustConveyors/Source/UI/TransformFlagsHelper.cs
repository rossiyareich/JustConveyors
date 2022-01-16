using JustConveyors.Source.Drawing;
using SDL2;

namespace JustConveyors.Source.UI;

internal static class TransformFlagsHelper
{
    public static SDL.SDL_Rect TryGetScrSpaceArw(this (int X, int Y) position, TransformFlags direction) =>
        direction switch
        {
            TransformFlags.DirN => new SDL.SDL_Rect
            {
                h = 16,
                w = 16,
                x = position.X,
                y = position.Y - 18 < 0
                    ? 0
                    : position.Y - 18
            },
            TransformFlags.DirS => new SDL.SDL_Rect
            {
                h = 16,
                w = 16,
                x = position.X,
                y = position.Y + 18 > Configuration.WindowSizeY
                    ? Configuration.WindowSizeY - 14
                    : position.Y + 18
            },
            TransformFlags.DirE => new SDL.SDL_Rect
            {
                h = 16,
                w = 16,
                x = position.X + 18 > Configuration.WindowSizeX
                    ? Configuration.WindowSizeX - 14
                    : position.X + 18,
                y = position.Y
            },
            TransformFlags.DirW => new SDL.SDL_Rect
            {
                h = 16,
                w = 16,
                x = position.X - 18 < Configuration.ControlsWidth
                    ? Configuration.ControlsWidth
                    : position.X - 18,
                y = position.Y
            },
            _ => throw new Exception("Unsupported direction")
        };

    public static (int X, int Y) TryGetScrSpaceArwXY(this (int X, int Y) position, TransformFlags direction)
    {
        SDL.SDL_Rect org = TryGetScrSpaceArw(position, direction);
        return (org.x, org.y);
    }

    public static TransformFlags GetOppositeFour(this TransformFlags flag) => flag switch
    {
        TransformFlags.DirN => TransformFlags.DirS,
        TransformFlags.DirS => TransformFlags.DirN,
        TransformFlags.DirE => TransformFlags.DirW,
        TransformFlags.DirW => TransformFlags.DirE,
        _ => throw new Exception("Unsupported direction")
    };
}
