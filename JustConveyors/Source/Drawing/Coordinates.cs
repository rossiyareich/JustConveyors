using System.Numerics;
using JustConveyors.Source.Rendering;
using static SDL2.SDL;

namespace JustConveyors.Source.Drawing;

internal static class Coordinates
{
    public static Vector2 PointingToRaw;
    public static Vector2 PointingToScreenSpace;
    public static (int X, int Y) PointingToTileScreenSpace;
    public static Vector2 CenterScr => new Vector2(Configuration.WindowSizeX, Configuration.WindowSizeY) / 2f;

    public static void UpdatePointer()
    {
        SDL_GetMouseState(out int mouseX, out int mouseY);
        PointingToRaw.X = mouseX;
        PointingToRaw.Y = mouseY;

        PointingToScreenSpace = GetScaledOffset();
        PointingToTileScreenSpace = GetClosestScaledOffset();
    }

    private static Vector2 GetScaledOffset()
    {
        float pointingX = MapHelper.Map(
            PointingToRaw.X - (Camera2D.FocusPxs.X - CenterScr.X) * 1.5f * Camera2D.M,
            0f, Configuration.WindowSizeX,
            Camera2D.FocusPxs.X - Configuration.WindowSizeX / 2f / Camera2D.M,
            Camera2D.FocusPxs.X + Configuration.WindowSizeX / 2f / Camera2D.M);
        float pointingY = MapHelper.Map(
            PointingToRaw.Y - (Camera2D.FocusPxs.Y - CenterScr.Y) * 1.5f * Camera2D.M,
            0f, Configuration.WindowSizeY,
            Camera2D.FocusPxs.Y - Configuration.WindowSizeY / 2f / Camera2D.M,
            Camera2D.FocusPxs.Y + Configuration.WindowSizeY / 2f / Camera2D.M);
        return new Vector2(pointingX, pointingY);
    }

    private static (int X, int Y) GetClosestScaledOffset()
    {
        int pointingX = (int)MapHelper.Map(
            PointingToRaw.X - (Camera2D.FocusPxs.X - CenterScr.X) * 1.5f * Camera2D.M,
            0f, Configuration.WindowSizeX,
            Camera2D.FocusPxs.X - Configuration.WindowSizeX / 2f / Camera2D.M,
            Camera2D.FocusPxs.X + Configuration.WindowSizeX / 2f / Camera2D.M);
        int pointingY = (int)MapHelper.Map(
            PointingToRaw.Y - (Camera2D.FocusPxs.Y - CenterScr.Y) * 1.5f * Camera2D.M,
            0f, Configuration.WindowSizeY,
            Camera2D.FocusPxs.Y - Configuration.WindowSizeY / 2f / Camera2D.M,
            Camera2D.FocusPxs.Y + Configuration.WindowSizeY / 2f / Camera2D.M);
        return (pointingX / 16 * 16, pointingY / 16 * 16);
    }
}
