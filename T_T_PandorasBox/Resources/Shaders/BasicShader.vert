#version 330 core

layout (location = 0) in vec3 vPos;

out vec4 ourColor;

uniform mat4 uModel;
uniform vec4 color;

void main()
{
    gl_Position = uModel * vec4(vPos, 1.0);
    ourColor = color;
}
