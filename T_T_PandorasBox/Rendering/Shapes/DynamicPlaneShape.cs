using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Buffers;

namespace T_T_PandorasBox.Rendering.Shapes;

internal unsafe class DynamicPlaneShape : IShape
{
    public Vector3[] Vertices = {
        new(-1.0f, 0.0f, 1.0f),
        new(1.0f, 0.0f, 1.0f),
        new(1.0f, 0.0f, -1.0f),
        new(-1.0f, 0.0f, -1.0f),
    };

    public Vector2[] Uv =
    {
        new(0.0f, 0.0f),
        new( 1.0f, 0.0f),
        new(1.0f, 1.0f),
        new(0.0f, 1.0f)
    };
    
    public uint[] Indices =
    {
        0, 1, 2,
        2, 3, 0
    };

    public VertexArrayObject<VertexPositionUv, uint> Vao { get; }

    public PrimitiveType PrimitiveType => PrimitiveType.Triangles;
    public int IndicesCount => Indices.Length;

    private readonly VertexPositionUv[] _vertexPositionUv;
    private readonly BufferObject<VertexPositionUv> _vertexBuffer;
    
    public DynamicPlaneShape(GL gl)
    {
        _vertexPositionUv = new VertexPositionUv[4];
        for (var i = 0; i < Vertices.Length; i++)
        {
            _vertexPositionUv[i] = new VertexPositionUv
            {
                Position = Vertices[i],
                Uv = Uv[i]
            };
        }
        _vertexBuffer = new BufferObject<VertexPositionUv>(gl, _vertexPositionUv.Length, BufferTargetARB.ArrayBuffer, true);
        var indexBuffer = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
        Vao = new VertexArrayObject<VertexPositionUv, uint>(gl, _vertexBuffer, indexBuffer);
        Vao.BindPointers(VertexPositionUv.VertexBindings);
    }
    
    public void Set(Vector3 start, Vector3 end, float width)
    {
        var direction = Vector3.Normalize(end - start);

        var d = Math.Abs(Vector3.Dot(direction, Vector3.UnitY)) < 0.99f;
        var up = d ? Vector3.UnitY : Vector3.UnitX;
        var perp = Vector3.Normalize(Vector3.Cross(direction, up));

        var offset = perp * (width * 0.5f);

        SetPosition(0, start + offset);
        SetPosition(1, start - offset);
        SetPosition(3, end + offset);
        SetPosition(2, end - offset);
        _vertexBuffer.SetData(_vertexPositionUv, 0, 4);
    }

    private void SetPosition(int index, Vector3 position)
    {
        var v = _vertexPositionUv[index];
        v.Position = position;
        _vertexPositionUv[index] = v;
    }
    
    public void Dispose()
    {
        Vao.Dispose();
    }
}