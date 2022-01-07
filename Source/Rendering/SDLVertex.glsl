#version 330 core

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTex;
out vec2 texCoord;

void main()
{
    gl_Position = vec4(aPosition.xy, 0, 1.0);
    texCoord = vec2(aTex.s, 1.0 - aTex.t);
}
