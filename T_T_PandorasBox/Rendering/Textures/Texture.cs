using System.Drawing;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Textures;

public unsafe  class Texture : IDisposable
{
    private readonly GL _gl;
    private readonly uint _handle;
    public int Width { get; }
    public int Height { get; }
    
    public Texture(GL gl, int width, int height)
    {
        _gl = gl;
        Width = width;
        Height = height;

        _handle = gl.GenTexture();
        Bind();

        gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)width, (uint)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);

        SetParameters();
    }

    private void SetParameters()
    {
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }
    
    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(textureSlot);
        _gl.BindTexture(TextureTarget.Texture2D, _handle);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(_handle);
    }
    
    public void SetData(Rectangle bounds, byte[] data)
    {
        Bind();
        fixed (byte* ptr = data)
        {
            _gl.TexSubImage2D(
                target: TextureTarget.Texture2D,
                level: 0,
                xoffset: bounds.X,
                yoffset: bounds.Y,
                width: (uint)bounds.Width,
                height: (uint)bounds.Height,
                format: PixelFormat.Rgba,
                type: PixelType.UnsignedByte,
                pixels: ptr
            );
        }
    }
}