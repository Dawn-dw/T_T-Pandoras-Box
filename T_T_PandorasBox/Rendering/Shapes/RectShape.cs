using System.Numerics;
using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Buffers;

namespace T_T_PandorasBox.Rendering.Shapes
{
    internal class RectShape : IShape
    {
        public static readonly Vector3[] Vertices = {
            new(-1.0f, 1.0f, 0.0f),
            new(1.0f, 1.0f, 0.0f),
            new(1.0f, -1.0f, 0.0f),
            new(-1.0f, -1.0f, 0.0f),
        };

        public static readonly Vector2[] Uv =
        {
            new(0.0f, 0.0f),
            new( 1.0f, 0.0f),
            new(1.0f, 1.0f),
            new(0.0f, 1.0f)
        };
    
        public static readonly uint[] Indices =
        {
            0, 1, 2,
            2, 3, 0
        };

        public VertexArrayObject<VertexPositionUv, uint> Vao { get; }

        public PrimitiveType PrimitiveType => PrimitiveType.Triangles;
        public int IndicesCount => Indices.Length;

        public RectShape(GL gl)
        {
            var vertexData = new VertexPositionUv[Vertices.Length];
            for (var i = 0; i < Vertices.Length; i++)
            {
                vertexData[i] = new VertexPositionUv
                {
                    Position = Vertices[i],
                    Uv = Uv[i]
                };
            }
            var vertexBuffer = new BufferObject<VertexPositionUv>(gl, vertexData, BufferTargetARB.ArrayBuffer);
            var indexBuffer = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vao = new VertexArrayObject<VertexPositionUv, uint>(gl, vertexBuffer, indexBuffer);
            Vao.BindPointers(VertexPositionUv.VertexBindings);
        }

        public void Dispose()
        {
            Vao.Dispose();
        }
    }
}
