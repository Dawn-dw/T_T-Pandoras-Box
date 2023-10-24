using System.Runtime.InteropServices;

namespace Api.GameProcess;

public class BatchReadContext : IDisposable
{
    private IntPtr _basePtr;
    private GCHandle _handle;
    public byte[] Bytes { get; private set; }
    public int Size => Bytes.Length;

    public BatchReadContext(byte[] bytes)
    {
        Bytes = bytes;
        _handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
        _basePtr = _handle.AddrOfPinnedObject();
    }

    public BatchReadContext(int size)
    {
        Bytes = new byte[size];
        _handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
        _basePtr = _handle.AddrOfPinnedObject();
    }

    public unsafe T Read<T>(int offset) where T : unmanaged
    {
        //FUCK MARSHALING
        return *(T*)(_basePtr + offset);
    }
    
    public T? ReadManaged<T>(int offset)
    {
        return Marshal.PtrToStructure<T>(_basePtr + offset);
    }

    public void Resize(int size)
    {
        if (size == Bytes.Length)
        {
            return;
        }
        
        _handle.Free();
        
        Bytes = new byte[size];
        _handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
        _basePtr = _handle.AddrOfPinnedObject();
    }

    private bool _disposed = false;
    public void Dispose()
    {
        if(_disposed) return;
        _disposed = true;
        _handle.Free();
        GC.SuppressFinalize(this);
    }

    ~BatchReadContext()
    {
        Dispose();
    }
}

public interface IMemory
{
    bool ReadModule(int offset, byte[] value);
    bool Read(IntPtr memoryAddress, byte[] value);
    bool ReadModule(int offset, int numberOfBytes, out byte[] value);
    bool Read(IntPtr memoryAddress, int numberOfBytes, out byte[] value);
    bool ReadModule<T>(int offset, out T value) where T : struct;
    bool Read<T>(IntPtr memoryAddress, out T value) where T : struct;
    bool ReadModule<T>(int offset, out T value, params int[] offsets) where T : struct;
    bool Read<T>(IntPtr memoryAddress, out T value, params int[] offsets) where T : struct;
    bool ReadModulePointer(int memoryAddress, out IntPtr value);
    bool ReadPointer(IntPtr memoryAddress, out IntPtr value);

    bool ReadModuleCachedBuffer(int offset, BatchReadContext value);
    bool ReadCachedBuffer(IntPtr memoryAddress, BatchReadContext value);
}