using System.Numerics;
using Api;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Materials;

internal class OutlineRectMaterial : Material
{
    internal Color FillColor { get; set; } = Color.White;
    internal int FillColorHash { get; private set; }
    
    internal OutlineRectMaterial(GL gl)
    {
        Shader = new Shader(gl, "OutlineRect");
        
        if (Shader is null) return;
        
        FillColorHash = Shader.GetHash("fillColor");
    }
    
    internal override void OnShaderSet()
    {
        base.OnShaderSet();
        
        if (Shader is null) return;
        
        Shader?.SetColor(FillColorHash, FillColor);
    }

    internal override bool Use(Matrix4x4 modelMatrix)
    {
        if (!base.Use(modelMatrix)) return false;
        
        Shader?.SetColor(FillColorHash, FillColor);
        
        return true;
    }
}