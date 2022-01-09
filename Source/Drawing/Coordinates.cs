using System.Numerics;
using static SDL2.SDL;

namespace JustConveyors.Source.Drawing;

internal static class Coordinates
{
    public static (int x, int y) PointingToTile;

    public static Vector2 PointingToScreenSpace;

    public static void UpdatePointer()
    {
        SDL_GetMouseState(out int mouseX, out int mouseY);
        PointingToScreenSpace.X = mouseX;
        PointingToScreenSpace.Y = mouseY;

        PointingToTile = ((int)(PointingToScreenSpace.X / 16), (int)(PointingToScreenSpace.Y / 16));
    }
}
