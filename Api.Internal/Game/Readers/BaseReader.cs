using System.Runtime.InteropServices;
using System.Text;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.GameProcess;
using Api.Internal.Game.Types;

namespace Api.Internal.Game.Readers;

public abstract class BaseReader : IDisposable
{
    private readonly byte[] _stringBuffer;
    private BatchReadContext? _batchReadContext;
    protected readonly byte[] CharArray = new byte[64];
    protected readonly IMemory Memory;
    
    protected BatchReadContext BatchReadContext => _batchReadContext ??= CreateBatchReadContext();

    protected BaseReader(IMemory memory)
    {
        Memory = memory;
        _stringBuffer = new byte[255];
    }

    protected abstract BatchReadContext CreateBatchReadContext();

       
    protected bool StartRead(IBaseObject baseObject)
    {
        return StartRead(baseObject, BatchReadContext);
    }
    
    protected T ReadOffset<T>(OffsetData offsetData) where T : unmanaged
    {
        return ReadOffset<T>(offsetData, BatchReadContext);
    }
    
    public bool StartRead(IBaseObject baseObject, BatchReadContext batchReadContext)
    {
        var result = ReadBuffer(baseObject.Pointer, batchReadContext);
        baseObject.IsValid = result;

        return result;
    }
    
    public bool StartRead(IntPtr ptr)
    {
        return ReadBuffer(ptr, BatchReadContext);
    }
    
    public bool ReadBuffer(IntPtr ptr, BatchReadContext batchReadContext)
    {
        return Memory.ReadCachedBuffer(ptr, batchReadContext);
    }

    public T ReadOffset<T>(OffsetData offsetData, BatchReadContext batchReadContext) where T : unmanaged
    {
        return batchReadContext.Read<T>(offsetData.Offset);
    }

    public string ReadString(OffsetData offsetData, Encoding encoding, BatchReadContext batchReadContext)
    {
        var ts = ReadOffset<TString>(offsetData, batchReadContext);
        if (ts._maxContentLength <= 0 || ts._contentLength <= 0)
        {
            return string.Empty;
        }
        
        if (ts._maxContentLength < 16)
        {
            return encoding.GetString(ts.GetSpan());
        }

        var ptr = ts.GetPtr();
        if (ptr == IntPtr.Zero || !Memory.Read(ptr, _stringBuffer))
        {
            return string.Empty;
        }
            
        var length = _stringBuffer.TakeWhile(t => t != 0).Count();
        return encoding.GetString(_stringBuffer, 0, length);
    }
    
    protected string ReadString(OffsetData offsetData, Encoding encoding)
    {
        return ReadString(offsetData, encoding, BatchReadContext);
    }
    
    public string ReadString(IntPtr pointer, Encoding encoding)
    {
        if (!Memory.Read<TString>(pointer, out var ts))
        {
            return string.Empty;
        }
        if (ts._maxContentLength <= 0 || ts._contentLength <= 0)
        {
            return string.Empty;
        }
        
        if (ts._maxContentLength < 16)
        {
            return encoding.GetString(ts.GetSpan());
        }

        var ptr = ts.GetPtr();
        if (ptr == IntPtr.Zero || !Memory.Read(ptr, _stringBuffer))
        {
            return string.Empty;
        }
            
        var length = _stringBuffer.TakeWhile(t => t != 0).Count();
        return encoding.GetString(_stringBuffer, 0, length);
    }

    public string ReadCharArray(IntPtr strPtr, Encoding encoding)
    {
        if (!Memory.Read(strPtr, CharArray))
        {
            return string.Empty;
        }
            
        var length = CharArray.TakeWhile(t => t != 0).Count();
        if (length <= 0)
        {
            return string.Empty;
        }
        return encoding.GetString(CharArray, 0, length);
    }
    
    public int GetSize(IEnumerable<OffsetData> offsetData)
    {
        return offsetData.Select(data => data.Offset + data.TargetSize).Prepend(0).Max();
    }
    
    public virtual void Dispose()
    {
        _batchReadContext?.Dispose();
    }
}