using static JustConveyors.Renderer.Libraries.ImGuiGL.OpenGL.GL;

namespace JustConveyors.Renderer.Libraries.ImGuiGL.OpenGL.Constructs;

public sealed class GLShader : IDisposable
{
    public GLShader(string source, ShaderType type)
    {
        ShaderType = type;
        ShaderID = glCreateShader(type);

        ShaderSource(ShaderID, source);
        glCompileShader(ShaderID);

        if (!GetShaderCompileStatus(ShaderID))
        {
            throw new Exception(ShaderLog);
        }
    }

    /// <summary>
    ///     Specifies the OpenGL ShaderID.
    /// </summary>
    public uint ShaderID { get; private set; }

    /// <summary>
    ///     Specifies the type of shader.
    /// </summary>
    public ShaderType ShaderType { get; }

    /// <summary>
    ///     Returns Gl.GetShaderInfoLog(ShaderID), which contains any compilation errors.
    /// </summary>
    public string ShaderLog => GetShaderInfoLog(ShaderID);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~GLShader() => Dispose(false);

    private void Dispose(bool disposing)
    {
        if (ShaderID != 0)
        {
            glDeleteShader(ShaderID);
            ShaderID = 0;
        }
    }
}
