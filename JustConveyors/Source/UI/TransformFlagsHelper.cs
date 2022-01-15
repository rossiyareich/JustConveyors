using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustConveyors.Source.Drawing;
using SDL2;

namespace JustConveyors.Source.UI
{
    internal static class TransformFlagsHelper
    {
        public static SDL.SDL_Rect GetScrSpaceArw(this (int X, int Y) position, TransformFlags direction)
        {
            return direction switch
            {
                TransformFlags.DirN => new SDL.SDL_Rect()
                {
                    h = 16,
                    w = 16,
                    x = position.X,
                    y = (position.Y - 18 < 0
                        ? 0
                        : position.Y - 18)
                },
                TransformFlags.DirS => new SDL.SDL_Rect()
                {
                    h = 16,
                    w = 16,
                    x = position.X,
                    y = (position.Y + 18 > Configuration.WindowSizeY
                        ? Configuration.WindowSizeY - 14
                        : position.Y + 18)
                },
                TransformFlags.DirE => new SDL.SDL_Rect()
                {
                    h = 16,
                    w = 16,
                    x = (position.X + 18 > Configuration.WindowSizeX
                        ? Configuration.WindowSizeX - 14
                        : position.X + 18),
                    y = position.Y
                },
                TransformFlags.DirW => new SDL.SDL_Rect()
                {
                    h = 16,
                    w = 16,
                    x = (position.X - 18 < Configuration.ControlsWidth
                        ? Configuration.ControlsWidth
                        : position.X - 18),
                    y = position.Y
                },
                _ => throw new Exception("Unsupported direction")
            };
        }

        public static (int X, int Y) GetScrSpaceArwXY(this (int X, int Y) position, TransformFlags direction)
        {
            return (GetScrSpaceArw(position, direction).x, GetScrSpaceArw(position, direction).y);
        }
    }
}
