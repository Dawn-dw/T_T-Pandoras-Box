using System.Numerics;
using Api;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Materials;

internal class BasicShapeMaterial : Material
{
    internal Color Color { get; set; }
    internal int ColorHash { get; private set; }
    
    internal BasicShapeMaterial(GL gl)
    {
        Color = Color.White;
        Shader = new Shader(gl, "BasicShader");
        
        if (Shader is null) return;
        
        ColorHash = Shader.GetHash("color");
    }
    
    
    internal override void OnShaderSet()
    {
        base.OnShaderSet();
        
        if (Shader is null) return;
        
        ColorHash = Shader.GetHash("color");
    }

    internal override bool Use(Matrix4x4 modelMatrix)
    {
        if (!base.Use(modelMatrix)) return false;
        
        Shader?.SetColor(ColorHash, Color);
        
        return true;
    }
}