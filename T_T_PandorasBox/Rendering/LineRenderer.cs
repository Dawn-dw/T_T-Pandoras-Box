using System.Numerics;
using Api;
using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Buffers;

namespace T_T_PandorasBox.Rendering;

//TODO Might make lags. Line renderer needs fixes. 
internal unsafe class LineRenderer : IDisposable
{
    private readonly Shader _shader;

    private readonly int _modelHash;
    private readonly int _colorHash;

    public Vector3[] Vertices;
    public uint[] Indices;
    VertexArrayObject<Vector3, uint> Vao { get; }
    
    private readonly BufferObject<Vector3> _vertexBuffer;
    private readonly GL _gl;
    public LineRenderer(GL gl)
    {
        _gl = gl;
        _shader = new Shader(gl, "BasicShader");
        _modelHash = _shader.GetHash("uModel");
        _colorHash = _shader.GetHash("color");
        
        Vertices = new Vector3[20];
        Indices = new uint[20];
        for (var i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] = Vector3.Zero;
            Indices[i] = (uint)i;
        }
        
        _vertexBuffer = new BufferObject<Vector3>(gl, Vertices.Length, BufferTargetARB.ArrayBuffer, true);
        var indexBuffer = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
        Vao = new VertexArrayObject<Vector3, uint>(gl, _vertexBuffer, indexBuffer);
        Vao.BindPointers(new VertexBinding(0, 3, false, VertexAttribPointerType.Float, 0));
    }

    public void RenderLines(Vector3[] positions, float size, Color color, Matrix4x4 matrix)
    {
        _shader.Use();
        _shader.SetMatrix(_modelHash, matrix);
        _shader.SetColor(_colorHash, color);

        var itemsCount = Math.Min(positions.Length, Vertices.Length);
        Array.Copy(positions, Vertices, itemsCount);
        _vertexBuffer.SetData(Vertices, 0, itemsCount);
        
        Vao.Bind();

        _gl.LineWidth(size);
        _gl.DrawElements(PrimitiveType.LineStrip, (uint)positions.Length, DrawElementsType.UnsignedInt, null);
    }

    private bool _disposed;
    private void Dispose(bool disposing)
    {
        if(_disposed) return;
        _shader.Dispose();
        Vao.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LineRenderer()
    {
        Dispose(false);
    }
}