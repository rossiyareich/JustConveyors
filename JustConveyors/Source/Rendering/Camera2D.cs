using System.Numerics;
using JustConveyors.Source.ConfigurationNS;

namespace JustConveyors.Source.Rendering;

internal class Camera2D
{
    public Camera2D(Vector2 focusPosition, float zoom) => Zoom = new Zoom(zoom, focusPosition);

    public static Zoom Zoom { get; private set; }

    public static Matrix4x4 GetProjectionMatrix()
    {
        Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(0, Configuration.WindowSizeX,
            Configuration.WindowSizeY, 0, 0.01f, 100f);
        Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom.M / 2f);

        return orthoMatrix * zoomMatrix;
    }
}
