using System.Numerics;

namespace JustConveyors.Source.Rendering;

internal class Camera2D
{
    public static Action<float> OnZoomChanged;

    public Camera2D(Vector2 focusPxs, float zoom)
    {
        M = zoom;
        FocusPxs = focusPxs;
    }

    public static float M { get; private set; }
    public static Vector2 FocusPxs { get; private set; }

    public static Matrix4x4 GetProjectionMatrix()
    {
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(0, Configuration.WindowSizeX,
            Configuration.WindowSizeY, 0, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(M / 2f);

        return orthoMatrix * zoomMatrix;
    }

    public static void ChangeZoom(float newZoom)
    {
        M = newZoom;
        OnZoomChanged?.Invoke(M);
    }

    /// <summary>
    ///     Change focus on raw screen coordinates
    /// </summary>
    /// <param name="newFocus"></param>
    public static void ChangeFocusScrRaw(Vector2 newFocus)
    {
        FocusPxs = newFocus;
        SDLOpenGL.TranslationMatrix = Matrix4x4.CreateTranslation(FocusPxs.X, FocusPxs.Y, 0f);
    }
}
