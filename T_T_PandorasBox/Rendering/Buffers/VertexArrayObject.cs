using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Buffers
{
    internal class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private readonly uint _handle;
        private readonly uint _stride;
        private readonly BufferObject<TVertexType> _vbo;
        private readonly BufferObject<TIndexType> _ebo;
        private readonly GL _gl;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
        {
            _gl = gl;
            _stride = (uint)Marshal.SizeOf<TVertexType>();
            _handle = _gl.GenVertexArray();
            _vbo = vbo;
            _ebo = ebo;
            
            _vbo.Bind();
            _ebo.Bind();
            Bind();
            BindPointers();
        }
        
        public unsafe void VertexAttributePointer(uint location, int size, bool normalized, VertexAttribPointerType type, int offset)
        {
            _gl.EnableVertexAttribArray(location);
            _gl.VertexAttribPointer(location, size, type, normalized, _stride, (void*)offset);
        }

        public void BindPointers(params VertexBinding[] vertexBindings)
        {
            foreach (var vertexBinding in vertexBindings)
            {
                VertexAttributePointer(vertexBinding.Location, vertexBinding.Size, vertexBinding.Normalized, vertexBinding.VertexAttribPointerType, vertexBinding.Offset);
            }
        }
        
        public void Bind()
        {
            _gl.BindVertexArray(_handle);
            _vbo.Bind();
            _ebo.Bind();
        }

        public void Dispose()
        {
            _gl.DeleteVertexArray(_handle);
            _vbo.Dispose();
            _ebo.Dispose();
        }
    }
}
