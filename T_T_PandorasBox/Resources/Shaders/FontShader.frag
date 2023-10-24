#version 330

uniform sampler2D TextureSampler;

in vec2 TexCoord;
in vec4 Color;

out vec4 FragColor;

void main()
{
    FragColor = Color * texture2D(TextureSampler, TexCoord);
}