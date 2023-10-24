using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Buffers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexPositionUv
{
    public Vector3 Position;
    public Vector2 Uv;

    public static VertexBinding[] VertexBindings =
    {
        new VertexBinding(0, 3, false, VertexAttribPointerType.Float, 0),
        new VertexBinding(1, 2, false, VertexAttribPointerType.Float, Marshal.SizeOf<Vector3>()),
    };
}