using System.Numerics;
using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Rendering;

internal class Zoom
{
    public Zoom(float m, Vector2 focusPxs)
    {
        M = m;
        FocusPxs = focusPxs;
    }

    public static float M { get; private set; }
    public static Vector2 FocusPxs { get; private set; }

    public static event Action<float> OnZoomChanged;
    public static event Action<Vector2> OnFocusChanged;

    public static void ChangeZoom(float newZoom)
    {
        M = newZoom;
        OnZoomChanged?.Invoke(M);
    }

    public static void ChangeFocusPxs(Vector2 newFocus)
    {
        FocusPxs = newFocus;
        //SDLOpenGL.TranslationMatrix = Matrix4x4.CreateTranslation(-FocusPxs.X, -FocusPxs.Y, 0f);
        OnFocusChanged?.Invoke(FocusPxs);
    }

    public static void ChangeFocusTile()
    {
        ChangeFocusPxs(Coordinates.PointingToScreenSpace);
    }
}
