#version 330 core
in vec4 ourColor;                   // Color from vertex shader
out vec4 FragColor;

void main()
{
    FragColor = ourColor;  // Assign color to pixel
}