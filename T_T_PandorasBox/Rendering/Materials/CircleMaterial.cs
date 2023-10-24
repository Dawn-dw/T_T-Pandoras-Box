using System.Numerics;
using Api;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Materials;

internal class CircleMaterial : Material, IDisposable
{
    internal int Type { get; set; } = 0;
    internal float Speed { get; set; } = 1f;
    internal float Time { get; set; } = 1f;
    internal float Size { get; set; } = 1f;
    internal float Width { get; set; } = 5f;
    internal Color FillColor { get; set; } = Color.White;
    internal Color EmptyColor { get; set; } = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    
    internal int TypeHash { get; private set; }
    internal int SpeedHash { get; private set; }
    internal int TimeHash { get; private set; }
    internal int SizeAmountHash { get; private set; }
    internal int WidthAmountHash { get; private set; }
    internal int FillColorHash { get; private set; }
    internal int EmptyColorHash { get; private set; }
    
    internal CircleMaterial(GL gl)
    {
        Shader = new Shader(gl, "CircleShader");
    }

    internal override void OnShaderSet()
    {
        base.OnShaderSet();
        
        if (Shader is null) return;
        
        TypeHash = Shader.GetHash("uType");
        SpeedHash = Shader.GetHash("uSpeed");
        TimeHash = Shader.GetHash("uTime");
        SizeAmountHash = Shader.GetHash("uSize");
        WidthAmountHash = Shader.GetHash("uWidth");
        FillColorHash = Shader.GetHash("fillColor");
        EmptyColorHash = Shader.GetHash("emptyColor");
    }

    internal override bool Use(Matrix4x4 modelMatrix)
    {
        if (!base.Use(modelMatrix)) return false;
        
        Shader?.SetInt(TypeHash, Type);
        Shader?.SetFloat(SpeedHash, Speed);
        Shader?.SetFloat(TimeHash, Time);
        Shader?.SetFloat(SizeAmountHash, Size);
        Shader?.SetFloat(WidthAmountHash, Width);
        Shader?.SetColor(FillColorHash, FillColor);
        Shader?.SetColor(EmptyColorHash, EmptyColor);
        
        return true;
    }
}