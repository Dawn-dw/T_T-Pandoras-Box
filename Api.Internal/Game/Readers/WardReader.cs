using Api.Game.Data;
using Api.Game.ObjectNameMappers;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class WardReader : AttackableUnitReader, IWardReader
{
    private readonly IWardNameTypeMapper _wardNameTypeMapper;
    
    public WardReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        IWardNameTypeMapper wardNameTypeMapper,
        UnitDataDictionary unitDataDictionary)
        : base(memory, gameObjectOffsets, attackableUnitOffsets, unitDataDictionary)
    {
        _wardNameTypeMapper = wardNameTypeMapper;
    }

    public bool ReadWard(IWard? ward)
    {
        if (ward is null || !ReadAttackableUnit(ward))
        {
            return false;
        }
        
        ward.WardType = _wardNameTypeMapper.Map(ward.ObjectNameHash);
        
        return true;
    }

    public bool ReadWard(IWard? ward, BatchReadContext batchReadContext)
    {
        if (ward is null || !ReadAttackableUnit(ward, batchReadContext))
        {
            return false;
        }

        ward.WardType = _wardNameTypeMapper.Map(ward.ObjectNameHash);
        
        return true;
    }
}