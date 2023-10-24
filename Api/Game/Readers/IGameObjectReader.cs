using System.Numerics;
using System.Text;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.GameProcess;

namespace Api.Game.Readers;

public interface IGameObjectReader
{
    bool ReadBuffer(IntPtr ptr, BatchReadContext batchReadContext);
    T ReadOffset<T>(OffsetData offsetData, BatchReadContext batchReadContext) where T : unmanaged;
    string ReadString(OffsetData offsetData, Encoding encoding, BatchReadContext batchReadContext);
    bool ReadObject(IGameObject? gameObject);
    /// <summary>
    /// batchReadContext must be initialized and read manully before with ReadBuffer(IntPtr ptr, BatchReadContext batchReadContext)
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="batchReadContext"></param>
    /// <returns></returns>
    bool ReadObject(IGameObject? gameObject, BatchReadContext batchReadContext);
    string ReadObjectName(BatchReadContext batchReadContext);
    int ReadObjectNetworkId(BatchReadContext batchReadContext);
    public int GetSize(IEnumerable<OffsetData> offsetData);
    public int GetBufferSize();
}