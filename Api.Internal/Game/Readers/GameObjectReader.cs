using System.Numerics;
using System.Text;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;
using Api.Internal.Game.Objects;
using Api.Internal.Game.Offsets;
using Microsoft.Extensions.Options;

namespace Api.Internal.Game.Readers;

internal class GameObjectReader : BaseReader, IGameObjectReader
{
    private readonly List<int> invalidObjectNames = new()
    {
        "cube".GetHashCode()
    };
    
    protected readonly IGameObjectOffsets GameObjectOffsets;
    
    public GameObjectReader(IMemory memory, IGameObjectOffsets gameObjectOffsets) : base(memory)
    {
        GameObjectOffsets = gameObjectOffsets;
    }

    public bool ReadObject(IGameObject? gameObject)
    {
        if (gameObject is null || gameObject.Pointer.ToInt64() < 0x1000)
        {
            return false;
        }

        if (!StartRead(gameObject))
        {
            return false;
        }
        
        gameObject.IsVisible = ReadOffset<bool>(GameObjectOffsets.IsVisible);
        gameObject.Position = ReadOffset<Vector3>(GameObjectOffsets.Position);

        if (!gameObject.RequireFullUpdate)
        {
            return true;
        }
        
        gameObject.Name = ReadString(GameObjectOffsets.Name, Encoding.UTF8);
        gameObject.ObjectName = ReadString(GameObjectOffsets.ObjectName, Encoding.ASCII);
        if (string.IsNullOrWhiteSpace(gameObject.Name) && string.IsNullOrWhiteSpace(gameObject.ObjectName))
        {
            return false;
        }

        gameObject.ObjectNameHash = gameObject.ObjectName.GetHashCode();
        if (invalidObjectNames.Contains(gameObject.ObjectNameHash))
        {
            return false;
        }
        
        gameObject.Team = ReadOffset<int>(GameObjectOffsets.Team);
        gameObject.NetworkId = ReadOffset<int>(GameObjectOffsets.NetworkId);
        
        return true;
    }
    
    public bool ReadObject(IGameObject? gameObject, BatchReadContext batchReadContext)
    {
        if (gameObject is null || gameObject.Pointer == IntPtr.Zero)
        {
            return false;
        }
        
        gameObject.IsVisible = ReadOffset<bool>(GameObjectOffsets.IsVisible, batchReadContext);
        gameObject.Position = ReadOffset<Vector3>(GameObjectOffsets.Position, batchReadContext);

        if (!gameObject.RequireFullUpdate)
        {
            return true;
        }
        
        gameObject.Name = ReadString(GameObjectOffsets.Name, Encoding.UTF8, batchReadContext);
        gameObject.ObjectName = ReadString(GameObjectOffsets.ObjectName, Encoding.ASCII, batchReadContext);
        if (string.IsNullOrWhiteSpace(gameObject.Name) && string.IsNullOrWhiteSpace(gameObject.ObjectName))
        {
            return false;
        }

        gameObject.ObjectNameHash = gameObject.ObjectName.GetHashCode();
        if (invalidObjectNames.Contains(gameObject.ObjectNameHash))
        {
            return false;
        }
        
        gameObject.Team = ReadOffset<int>(GameObjectOffsets.Team, batchReadContext);
        gameObject.NetworkId = ReadOffset<int>(GameObjectOffsets.NetworkId, batchReadContext);
        
        return true;
    }

    public string ReadObjectName(BatchReadContext batchReadContext)
    {
        return ReadString(GameObjectOffsets.ObjectName, Encoding.ASCII, batchReadContext);
    }

    public int ReadObjectNetworkId(BatchReadContext batchReadContext)
    {
        return ReadOffset<int>(GameObjectOffsets.NetworkId, batchReadContext);
    }

    public virtual int GetBufferSize()
    {
        return GetSize(GameObjectOffsets.GetOffsets());
    }

    protected override BatchReadContext CreateBatchReadContext()
    {
        return new BatchReadContext(GetBufferSize());
    }
}