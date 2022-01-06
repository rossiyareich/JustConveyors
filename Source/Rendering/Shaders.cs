﻿using SDLImGuiGL;

namespace JustConveyors.Source.Rendering;

internal class Shaders : IRenderComponent
{
    private string _fragmentShaderRaw;
    private GLShaderProgram _shader;
    private string _vertexShaderRaw;

    public Shaders(string vertexShaderPath, string fragmentShaderPath)
    {
        _vertexShaderPath = vertexShaderPath;
        _fragmentShaderPath = fragmentShaderPath;
    }

    private string _vertexShaderPath { get; }
    private string _fragmentShaderPath { get; }

    public void Dispose()
    {
        GL.glDeleteShader(_shader.ProgramID);
        _shader = null;
    }

    public void Load()
    {
        _vertexShaderRaw = File.ReadAllText(_vertexShaderPath);
        _fragmentShaderRaw = File.ReadAllText(_fragmentShaderPath);

        GLShader vertexShader = default;
        GLShader fragmentShader = default;

        try
        {
            vertexShader = new GLShader(_vertexShaderRaw, GL.ShaderType.VertexShader);
        }
        catch
        {
            throw new Exception($"Error compiling vertex shader: {vertexShader.ShaderLog}");
        }

        try
        {
            fragmentShader = new GLShader(_fragmentShaderRaw, GL.ShaderType.FragmentShader);
        }
        catch
        {
            throw new Exception($"Error compiling vertex shader: {fragmentShader.ShaderLog}");
        }

        _shader = new GLShaderProgram(vertexShader, fragmentShader);

        GL.glDetachShader(_shader.ProgramID, vertexShader.ShaderID);
        GL.glDetachShader(_shader.ProgramID, fragmentShader.ShaderID);

        vertexShader.Dispose();
        fragmentShader.Dispose();
    }

    public void Cleanup()
    {
    }

    public Display _display { get; }

    public void ApplyShaders() => GL.glUseProgram(_shader.ProgramID);

    public void SetTexture()
    {
        int texUniformLocation = GL.glGetUniformLocation(_shader.ProgramID, "tex");
        GL.glUniform1i(texUniformLocation, 0);
    }
}
