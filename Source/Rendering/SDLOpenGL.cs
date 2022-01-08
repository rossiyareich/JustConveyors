using System.Drawing;
using System.Numerics;
using JustConveyors.Source.ConfigurationNS;
using SDLImGuiGL;

namespace JustConveyors.Source.Rendering;

internal class SDLOpenGL : IRenderHolder
{
    public SDLOpenGL(Display display)
    {
        _display = display;
        Texture = new Texture(display);
        Shaders = new Shaders(display, @"Source/Rendering/SDLVertex.glsl", @"Source/Rendering/SDLFragment.glsl");
        Vertex = new Vertex(display);
        Camera = new Camera2D(new Vector2(Configuration.WindowSizeX, Configuration.WindowSizeY) / 2f, _display.Zoom);
    }

    public Texture Texture { get; }
    public Shaders Shaders { get; }
    public Vertex Vertex { get; }
    public Camera2D Camera { get; }
    public Display _display { get; }

    public void Load()
    {
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
        Matrix4x4 translationMatrix =
            Matrix4x4.CreateTranslation(Configuration.WindowSizeX / 2f, Configuration.WindowSizeY / 2f, 0f);

        ClearToColor(Color.Black);
        Texture.RendererToTexture();
        Shaders.ApplyShaders();
        Shaders.SetMatrix4x4("model", scaleMatrix * rotationMatrix * translationMatrix);
        Shaders.SetMatrix4x4("projection", Camera.GetProjectionMatrix());
        Vertex.BindVertexArray();
        Vertex.DrawVertexArray();
    }

    private static void ClearToColor(Color color)
    {
        GL.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        GL.glClear(GL.ClearBufferMask.ColorBufferBit);
    }
}
