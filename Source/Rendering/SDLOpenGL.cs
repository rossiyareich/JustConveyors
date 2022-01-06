using System.Drawing;
using SDLImGuiGL;

namespace JustConveyors.Source.Rendering;

internal class SDLOpenGL : IRenderComponent
{
    public SDLOpenGL(Display display)
    {
        Texture = new Texture(display);
        Shaders = new Shaders(@"Source/Rendering/SDLVertex.glsl", @"Source/Rendering/SDLFragment.glsl");
        Vertex = new Vertex();
    }

    public Texture Texture { get; }
    public Shaders Shaders { get; }
    public Vertex Vertex { get; }
    public Display _display { get; }

    public void Load()
    {
        Shaders.Load();

        Texture.Load();
        Shaders.ApplyShaders();
        Shaders.SetTexture();
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
        ClearToColor(Color.Black);
        GL.glBindTexture(GL.TextureTarget.Texture2D, Texture.RendererToTexture());
        Shaders.ApplyShaders();
        Vertex.BindVertexArray();
        Vertex.DrawVertexArray();
    }

    private void ClearToColor(Color color)
    {
        GL.glClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        GL.glClear(GL.ClearBufferMask.ColorBufferBit);
    }
}
