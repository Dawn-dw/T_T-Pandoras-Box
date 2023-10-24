#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vColor;
layout (location = 2) in vec2 vUv;

uniform mat4 uModel;

out vec2 TexCoord;
out vec4 Color;

void main()
{
    gl_Position = uModel * vec4(vPos, 1.0);
    TexCoord = vUv;
    Color = vColor;
}