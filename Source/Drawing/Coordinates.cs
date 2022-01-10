using System.Numerics;
using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source.Drawing;

internal static class Coordinates
{
    public static Vector2 PointingToScreenSpace;

    public static void UpdatePointer()
    {
        SDL_GetMouseState(out int mouseX, out int mouseY);
        PointingToScreenSpace.X = mouseX;
        PointingToScreenSpace.Y = mouseY;
    }

    public static (int X, int Y) GetClosestGridScaledOffset()
    {
        int pointingX = (int)MapHelper.Map(
            PointingToScreenSpace.X - (Zoom.FocusPxs.X - Configuration.CenterScr.X) * 1.5f * Zoom.M,
            0f, Configuration.WindowSizeX,
            Zoom.FocusPxs.X - Configuration.WindowSizeX / 2f / Zoom.M,
            Zoom.FocusPxs.X + Configuration.WindowSizeX / 2f / Zoom.M);
        int pointingY = (int)MapHelper.Map(
            PointingToScreenSpace.Y - (Zoom.FocusPxs.Y - Configuration.CenterScr.Y) * 1.5f * Zoom.M,
            0f, Configuration.WindowSizeY,
            Zoom.FocusPxs.Y - Configuration.WindowSizeY / 2f / Zoom.M,
            Zoom.FocusPxs.Y + Configuration.WindowSizeY / 2f / Zoom.M);
        return new ValueTuple<int, int>(pointingX / 16 * 16, pointingY / 16 * 16);
    }
}
