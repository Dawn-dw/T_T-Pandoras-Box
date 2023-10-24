using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Buffers;

public struct VertexBinding
{
    public VertexBinding(uint location, int size, bool normalized, VertexAttribPointerType vertexAttribPointerType, int offset)
    {
        Location = location;
        Size = size;
        Normalized = normalized;
        VertexAttribPointerType = vertexAttribPointerType;
        Offset = offset;
    }

    public uint Location { get; }
    public int Size { get; }
    public bool Normalized { get; }
    public VertexAttribPointerType VertexAttribPointerType { get; }
    public int Offset { get; }
}