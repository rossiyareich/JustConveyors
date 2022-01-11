using System.Drawing;
using System.Numerics;
using JustConveyors.Libraries.ImGuiGL.OpenGL;
using JustConveyors.Source.ConfigurationNS;

namespace JustConveyors.Source.Rendering;

internal class SDLOpenGL : IRenderHolder
{
    public SDLOpenGL(Display display)
    {
        _display = display;
        Texture = new Texture(display);
        Shaders = new Shaders(display, @"Source/Rendering/SDLVertex.glsl", @"Source/Rendering/SDLFragment.glsl");
        Vertex = new Vertex(display);
        Camera = new Camera2D(Configuration.CenterScr, 1f);
    }

    public static Matrix4x4 TranslationMatrix { get; set; }

    public Texture Texture { get; }
    public Shaders Shaders { get; }
    public Vertex Vertex { get; }
    public Camera2D Camera { get; }
    public Display _display { get; }

    public void Load()
    {
        TranslationMatrix = Matrix4x4.CreateTranslation(Configuration.CenterScr.X, Configuration.CenterScr.Y, 0f);

        Shaders.Load();

        Texture.Load();
        Shaders.ApplyShaders();
        Shaders.ResetTexture();
        GL.glEnable(GL.EnableCap.Blend);
        GL.glBlendFunc(GL.BlendingFactorSrc.SrcAlpha, GL.BlendingFactorDest.OneMinusSrcAlpha);
        GL.glBindTexture(GL.TextureTarget.Texture2D, 0);
        Vertex.Load();
    }

    public void Cleanup()
    {
        Texture.Cleanup();
        Shaders.Cleanup();
        Vertex.Cleanup();
    }


    public void Dispose()
    {
        Vertex.Dispose();
        Shaders.Dispose();
        Texture.Dispose();
    }

    /// <remarks>
    ///     Set up pipeline here.
    /// </remarks>
    /// >
    public void Render()
    {
        Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(Configuration.WindowSizeX, Configuration.WindowSizeY, 1);
        Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationZ(0);

        ClearToColor(Color.Black);
        Texture.RendererToTexture();
        Shaders.ApplyShaders();
        Shaders.SetMatrix4x4("model", scaleMatrix * rotationMatrix * TranslationMatrix);
        Shaders.SetMatrix4x4("projection", Camera2D.GetProjectionMatrix());
        Vertex.BindVertexArray();
        Vertex.DrawVertexArray();
    }

    private static void ClearToColor(Color color)
    {
        GL.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        GL.glClear(GL.ClearBufferMask.ColorBufferBit);
    }
}
