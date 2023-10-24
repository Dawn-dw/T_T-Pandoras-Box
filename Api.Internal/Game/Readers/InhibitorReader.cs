using Api.Game.Data;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class InhibitorReader : AttackableUnitReader, IInhibitorReader
{
    public InhibitorReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        UnitDataDictionary unitDataDictionary)
        : base(memory, gameObjectOffsets, attackableUnitOffsets, unitDataDictionary)
    {
    }

    public bool ReadInhibitor(IInhibitor? inhibitor)
    {
        if (inhibitor is null || !ReadAttackableUnit(inhibitor))
        {
            return false;
        }
        
        return true;
    }

    public bool ReadInhibitor(IInhibitor? inhibitor, BatchReadContext batchReadContext)
    {
        if (inhibitor is null || !ReadAttackableUnit(inhibitor, batchReadContext))
        {
            return false;
        }
        
        return true;
    }
}