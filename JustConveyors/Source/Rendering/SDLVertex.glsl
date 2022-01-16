#version 330 core

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTex;
out vec2 texCoord;

uniform mat4 projection;
uniform mat4 model;

void main()
{
    gl_Position = projection * model * vec4(aPosition.xy, 0, 1.0);
    texCoord = aTex;
}
