using System.Numerics;
using JustConveyors.Source.ConfigurationNS;

namespace JustConveyors.Source.Rendering;

internal class Camera2D
{
    public Camera2D(Vector2 focusPosition, float zoom)
    {
        Zoom = new Zoom(zoom, focusPosition);
    }

    public static Zoom Zoom { get; private set; }

    public Matrix4x4 GetProjectionMatrix()
    {
        float left = Zoom.FocusPosition.X - Configuration.WindowSizeX / 2f;
        float right = Zoom.FocusPosition.X + Configuration.WindowSizeX / 2f;
        float top = Zoom.FocusPosition.Y - Configuration.WindowSizeY / 2f;
        float bottom = Zoom.FocusPosition.Y + Configuration.WindowSizeY / 2f;

        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom.M);

        return orthoMatrix * zoomMatrix;
    }
}
