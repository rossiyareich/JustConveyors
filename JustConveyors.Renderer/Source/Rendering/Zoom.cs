using System.Numerics;
using JustConveyors.Renderer.Source.Controls;

namespace JustConveyors.Renderer.Source.Rendering;

internal class Zoom
{
    public static Action<float> OnZoomChanged;

    public Zoom(float m, Vector2 focusPxs)
    {
        M = m;
        FocusPxs = focusPxs;
        GUI.OnZoomChanged += ChangeZoom;
    }

    public static float M { get; private set; }
    public static Vector2 FocusPxs { get; private set; }

    public static void ChangeZoom(float newZoom)
    {
        M = newZoom;
        OnZoomChanged?.Invoke(M);
    }

    /// <summary>
    ///     Change focus on raw screen coordinates
    /// </summary>
    /// <param name="newFocus"></param>
    public static void ChangeFocusPxs(Vector2 newFocus)
    {
        FocusPxs = newFocus;
        SDLOpenGL.TranslationMatrix = Matrix4x4.CreateTranslation(FocusPxs.X, FocusPxs.Y, 0f);
    }
}
