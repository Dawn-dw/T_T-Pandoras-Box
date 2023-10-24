using Api.Game.Data;
using Api.Game.ObjectNameMappers;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class PlantReader : AttackableUnitReader, IPlantReader
{
    private readonly IPlantNameTypeMapper _plantNameTypeMapper;
    
    public PlantReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        IPlantNameTypeMapper plantNameTypeMapper,
        UnitDataDictionary unitDataDictionary)
        : base(memory, gameObjectOffsets, attackableUnitOffsets, unitDataDictionary)
    {
        _plantNameTypeMapper = plantNameTypeMapper;
    }

    public bool ReadPlant(IPlant? plant)
    {
        if (plant is null || !ReadAttackableUnit(plant))
        {
            return false;
        }
        
        plant.PlantType = _plantNameTypeMapper.Map(plant.ObjectNameHash);
        
        return true;
    }

    public bool ReadPlant(IPlant? plant, BatchReadContext batchReadContext)
    {        
        if (plant is null || !ReadAttackableUnit(plant, batchReadContext))
        {
            return false;
        }

        plant.PlantType = _plantNameTypeMapper.Map(plant.ObjectNameHash);
        
        return true;
    }
}