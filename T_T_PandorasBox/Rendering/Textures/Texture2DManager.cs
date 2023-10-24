using System.Drawing;
using FontStashSharp.Interfaces;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Textures;

public class Texture2DManager : ITexture2DManager
{
    private GL _gl;

    public Texture2DManager(GL gl)
    {
        _gl = gl;
    }
    
    public object CreateTexture(int width, int height)
    {
        return  new Texture(_gl, width, height);
    }

    public Point GetTextureSize(object texture)
    {
        var t = (Texture)texture;
        return new Point(t.Width, t.Height);
    }

    public void SetTextureData(object texture, Rectangle bounds, byte[] data)
    {
        var t = (Texture)texture;
        t.SetData(bounds, data);
    }
}