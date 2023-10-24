using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace T_T_PandorasBox.Rendering.Buffers
{
    internal unsafe class BufferObject<T> : IDisposable where T : unmanaged
    {
        private readonly uint _handle;
        private readonly BufferTargetARB _bufferType;
        private readonly int _size;
        private readonly int _typeSize;
        private readonly GL _gl;

        public BufferObject(GL gl, Span<T> data, BufferTargetARB bufferType)
        {
            _gl = gl;
            _bufferType = bufferType;
            _handle = _gl.GenBuffer();
            _size = data.Length;
            _typeSize = Marshal.SizeOf<T>();
            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(bufferType, (nuint)(_size * _typeSize), d, BufferUsageARB.StaticDraw);
            }
        }
        
        public BufferObject(GL gl, T* data, int size, BufferTargetARB bufferType)
        {
            _gl = gl;
            _bufferType = bufferType;
            _handle = _gl.GenBuffer();
            _size = size;
            _typeSize = Marshal.SizeOf<T>();
            Bind();
            
            _gl.BufferData(bufferType, (nuint)(_size * _typeSize), data, BufferUsageARB.StaticDraw);
        }
        
        public BufferObject(GL gl, int size, BufferTargetARB bufferType, bool isDynamic)
        {
            _gl = gl;
            _bufferType = bufferType;
            _handle = _gl.GenBuffer();
            _size = size;
            _typeSize = Marshal.SizeOf<T>();
			
            Bind();

            var elementSizeInBytes = Marshal.SizeOf<T>();
            _gl.BufferData(bufferType, (nuint)(size * elementSizeInBytes), null, isDynamic ? BufferUsageARB.StreamDraw : BufferUsageARB.StaticDraw);
        }

        public void Bind()
        {
            _gl.BindBuffer(_bufferType, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_handle);
        }
        
        public void SetData(T[] data, int startIndex, int elementCount)
        {
            Bind();

            fixed(T* dataPtr = &data[startIndex])
            {
                _gl.BufferSubData(_bufferType, 0, (nuint)(elementCount * _typeSize), dataPtr);
            }
        }
        
        public void SetData(T* data, int startIndex, int elementCount)
        {
            Bind();
            _gl.BufferSubData(_bufferType, 0, (nuint)(elementCount * _typeSize), data);
        }
    }
}
