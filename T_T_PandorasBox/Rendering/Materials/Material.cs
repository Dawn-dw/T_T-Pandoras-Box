using System.Numerics;

namespace T_T_PandorasBox.Rendering.Materials;

internal class Material : IDisposable
{
    private Shader? _shader;
    internal int ModelHash { get; private set; }
    internal Shader? Shader
    {
        get => _shader;
        set
        {
            _shader = value;
            OnShaderSet();
        } 
    }

    internal virtual void OnShaderSet()
    { 
        if (Shader is null) return;
        
        ModelHash = Shader.GetHash("uModel");
    }

    internal virtual bool Use(Matrix4x4 modelMatrix)
    {
        if (Shader is null)
            return false;
        
        Shader.Use();
        Shader.SetMatrix(ModelHash, modelMatrix);
        
        return true;
    }
    
    private bool disposed;
    private void Dispose(bool disposing)
    {
        if(disposed) return;
        disposed = true;
        Shader?.Dispose();
        Shader = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Material()
    {
        Dispose(false);
    }
}