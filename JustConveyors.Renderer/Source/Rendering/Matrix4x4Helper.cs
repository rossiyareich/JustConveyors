using System.Numerics;

namespace JustConveyors.Renderer.Source.Rendering;

internal static class Matrix4x4Helper
{
    public static float[] ToFloatArray(this Matrix4x4 m) => new[]
    {
        m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43,
        m.M44
    };
}
