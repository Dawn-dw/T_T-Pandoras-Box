using System.Runtime.InteropServices;
using Api.GameProcess;

namespace Api.Internal.Game.Types;

public class TArray : IDisposable
{
    private readonly IMemory _memory;
    private readonly int _offset;
    private readonly BatchReadContext _doubleIntBatchReadContext;
    private BatchReadContext? _batchReadContext;
    private readonly int _intPtrSize;

    private int _size;
    public int Size => _size;

    private IntPtr _listPtr;
    public IntPtr ListPtr => _listPtr;
    
    public TArray(IMemory memory, int offset)
    {
        _memory = memory;
        _offset = offset;

        _listPtr = IntPtr.Zero;
        _size = 0;
        
        _intPtrSize = Marshal.SizeOf<IntPtr>();
        _doubleIntBatchReadContext = new BatchReadContext(_intPtrSize + Marshal.SizeOf<int>());
        _batchReadContext = null;
    }

    
    public bool Read()
    {
        if (!_memory.ReadModulePointer(_offset, out var tArray))
        {
            return false;
        }
        
        if (!_memory.ReadCachedBuffer(tArray + 0x8, _doubleIntBatchReadContext))
        {
         return false;
        }
        
        _listPtr = _doubleIntBatchReadContext.Read<IntPtr>(0);
        _size = _doubleIntBatchReadContext.Read<int>(_intPtrSize) * 0x8;
        
        if (_batchReadContext is null)
        {
            _batchReadContext = new BatchReadContext(_size);
        }
        else
        {
            _batchReadContext.Resize(_size);
        }

        return _memory.ReadCachedBuffer(_listPtr, _batchReadContext);
    }

    public IEnumerable<IntPtr> GetPointers()
    {
        if (_batchReadContext is null)
        {
            yield break;
        }
        
        for (var i = 0; i < _size; i++)
        {
            var ptr = _batchReadContext.Read<IntPtr>(0x8 * i);
            if (ptr != IntPtr.Zero && ptr.ToInt64() > 0x1000)
            {
                yield return ptr;
            }
        }
    }
    
    public void Dispose()
    {
        _batchReadContext?.Dispose();
    }
}