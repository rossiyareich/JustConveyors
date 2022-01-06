using SDLImGuiGL;

namespace JustConveyors.Source.Rendering;

internal class Vertex : IRenderComponent
{
    //Interleaved data => X, Y, (0 => inf)TexX, TexY
    private readonly float[] vertices =
    {
        -0.5f, 0.5f, 0f, 1f, //top left
        0.5f, 0.5f, 1f, 1f, //top right
        -0.5f, -0.5f, 0f, 0f, //bottom left
        0.5f, -0.5f, 1f, 0f //bottom right
    };

    private uint _VAO;
    private uint _VBO;

    public void Dispose()
    {
        GL.DeleteVertexArray(_VAO);
        GL.DeleteBuffer(_VBO);
    }

    public unsafe void Load()
    {
        _VAO = GL.GenVertexArray();
        _VBO = GL.GenBuffer();

        GL.glBindVertexArray(_VAO);
        GL.glBindBuffer(GL.BufferTarget.ArrayBuffer, _VBO);

        fixed (float* v = vertices)
        {
            GL.glBufferData(GL.BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * vertices.Length), (IntPtr)v,
                GL.BufferUsageHint.StaticDraw);
        }

        //Set the position attributes of the interleaved data => attribute index 0, size of the pair is 2, type is float, data not normalized, 4 floats between pair (two points), first apperance after 0 floats
        GL.glVertexAttribPointer(0, 2, GL.VertexAttribPointerType.Float, false, sizeof(float) * 4, (IntPtr)0);
        GL.glEnableVertexAttribArray(0); //Enable attribute index 0
        //Set the texture attributes of the interleaved data => attribute index 1, size of the pair is 2, type is float, data not normalized, 4 floats between pair (two points), first apperance after 2 floats
        GL.glVertexAttribPointer(1, 2, GL.VertexAttribPointerType.Float, false, sizeof(float) * 4,
            (IntPtr)(sizeof(float) * 2));
        GL.glEnableVertexAttribArray(1); //Enable attribute index 1

        //Unbind VAO and VBO once done creating them (VAO & VBO are setup)
        GL.glBindVertexArray(0);
        GL.glBindBuffer(GL.BufferTarget.ArrayBuffer, 0);
    }

    public void Cleanup() => GL.glBindVertexArray(0);

    public Display _display { get; }

    public void BindVertexArray() => GL.glBindVertexArray(_VAO);

    public void DrawVertexArray() => GL.glDrawArrays((int)GL.BeginMode.TriangleStrip, 0, vertices.Length / 4);
}
