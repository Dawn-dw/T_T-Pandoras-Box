using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Buffers;

namespace T_T_PandorasBox.Rendering.Shapes
{
    internal interface IShape : IDisposable
    {
        VertexArrayObject<VertexPositionUv, uint> Vao { get; }
        PrimitiveType PrimitiveType { get; }
        public int IndicesCount { get; }
    }
}
