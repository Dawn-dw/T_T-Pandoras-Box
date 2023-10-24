using Silk.NET.OpenGL;

namespace Api.States;

public class RenderContext
{
    public GL? Gl { get; private set; }

    public delegate void RenderContextChanged(GL gl);
    public event RenderContextChanged? OnRenderContextChanged;
    
    public void SetGl(GL gl)
    {
        Gl = gl;
        OnRenderContextChanged?.Invoke(gl);
    }
}