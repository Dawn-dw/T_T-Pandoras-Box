#version 330 core
in vec2 TexCoord;
out vec4 FragColor;
uniform vec4 fillColor = vec4(1.0, 1.0, 1.0, 1.0);

void main()
{
    FragColor = fillColor;
}