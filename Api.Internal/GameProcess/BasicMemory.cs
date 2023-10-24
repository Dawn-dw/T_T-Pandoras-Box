using System.Runtime.InteropServices;
using Api.GameProcess;
using WinApi;

namespace Api.Internal.GameProcess;

public class BasicMemory : IMemory
{
    private readonly ITargetProcess _targetProcess;
    
    public BasicMemory(ITargetProcess targetProcess)
    {
        _targetProcess = targetProcess;
    }

    public bool ReadModule(int offset, int numberOfBytes, out byte[] value)
    {
        return Read(_targetProcess.ModuleBase + offset, numberOfBytes, out value);
    }
    
    public bool ReadModule(int offset, byte[] value)
    {
        return Read(_targetProcess.ModuleBase + offset, value);
    }
    
    public bool Read(IntPtr memoryAddress, int numberOfBytes, out byte[] value)
    {
        value = new byte[numberOfBytes];
        return Read(memoryAddress, value);
    }
    
    public bool Read(IntPtr memoryAddress, byte[] value)
    {
        return WindowsApi.ReadProcessMemory(_targetProcess.Handle, memoryAddress, value, (uint)value.Length,
            out var _);
    }

    public bool ReadModule<T>(int offset, out T value) where T : struct
    {
        return Read(_targetProcess.ModuleBase + offset, out value);
    }
    
    public bool Read<T>(IntPtr memoryAddress, out T value) where T : struct
    {
        value = default;
        
        if (!Read(memoryAddress, Marshal.SizeOf(typeof(T)), out var bytes))
        {
            return false;
        }
        
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try
        {
            var obj = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            if (obj is not null)
            {
                value = (T)obj;
                return true;
            }
        }
        finally
        {
            handle.Free();
        }

        return false;
    }
    
    public bool ReadModule<T>(int offset, out T value, params int[] offsets) where T : struct
    {
        return Read(_targetProcess.ModuleBase + offset, out value, offsets);
    }

    public bool Read<T>(IntPtr memoryAddress, out T value, params int[] offsets) where T : struct
    {
        foreach (var offset in offsets)
        {
            if (!ReadPointer(memoryAddress, out var ptr) || ptr == IntPtr.Zero)
            {
                value = default;
                return false;
            }
        
            memoryAddress = ptr + offset;
        }
    
        return Read(memoryAddress, out value);
    }

    public bool ReadModulePointer(int offset, out IntPtr value)
    {
        return ReadPointer(_targetProcess.ModuleBase + offset, out value);
    }

    public bool ReadPointer(IntPtr memoryAddress, out IntPtr value)
    {
        if (Read(memoryAddress, IntPtr.Size, out var bytes))
        {
            value = (IntPtr)BitConverter.ToInt64(bytes, 0);
            return true;
        }

        value = IntPtr.Zero;
        return false;
    }

    public bool ReadModuleCachedBuffer(int memoryAddress, BatchReadContext value)
    {
        return ReadCachedBuffer(_targetProcess.ModuleBase + memoryAddress, value);
    }

    public bool ReadCachedBuffer(IntPtr memoryAddress, BatchReadContext value)
    {
        return Read(memoryAddress, value.Bytes);
    }
}