using Api.Game.Data;
using Api.Game.ObjectNameMappers;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class MinionReader : AiBaseUnitReader, IMinionReader
{
    private readonly IMinionNameTypeMapper _minionNameTypeMapper;
    
    public MinionReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        IMinionNameTypeMapper minionNameTypeMapper,
        UnitDataDictionary unitDataDictionary,
        IAiBaseUnitOffsets aiBaseUnitOffsets)
        : base(memory, gameObjectOffsets, attackableUnitOffsets, unitDataDictionary, aiBaseUnitOffsets)
    {
        _minionNameTypeMapper = minionNameTypeMapper;
    }

    public bool ReadMinion(IMinion? minion)
    {
        if (minion is null || !ReadAiBaseUnit(minion))
        {
            return false;
        }
        
        minion.MinionType = _minionNameTypeMapper.Map(minion.ObjectNameHash);
        
        return true;
    }

    public bool ReadMinion(IMinion? minion, BatchReadContext batchReadContext)
    {
        if (minion is null || !ReadAiBaseUnit(minion, batchReadContext))
        {
            return false;
        }

        minion.MinionType = _minionNameTypeMapper.Map(minion.ObjectNameHash);
        
        return true;
    }
}