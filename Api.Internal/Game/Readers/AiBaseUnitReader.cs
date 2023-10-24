using Api.Game.Data;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class AiBaseUnitReader : AttackableUnitReader, IAiBaseUnitReader
{
    private readonly IAiBaseUnitOffsets _aiBaseUnitOffsets;
    public AiBaseUnitReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        UnitDataDictionary unitDataDictionary,
        IAiBaseUnitOffsets aiBaseUnitOffsets) 
        : base(memory, gameObjectOffsets, attackableUnitOffsets, unitDataDictionary)
    {
        _aiBaseUnitOffsets = aiBaseUnitOffsets;
    }
    
    public bool ReadAiBaseUnit(IAiBaseUnit? aiBaseUnit)
    { 
        if (aiBaseUnit is null || !ReadAttackableUnit(aiBaseUnit))
        {
            return false;
        }
        
        aiBaseUnit.CurrentTargetIndex = ReadOffset<int>(_aiBaseUnitOffsets.CurrentTargetIndex);
        aiBaseUnit.BaseAttackRange = ReadOffset<float>(_aiBaseUnitOffsets.AttackRange);
        aiBaseUnit.BonusAttackSpeed = ReadOffset<float>(_aiBaseUnitOffsets.BonusAttackSpeed);
        
        aiBaseUnit.BaseAttackDamage = ReadOffset<float>(_aiBaseUnitOffsets.BaseAttackDamage);
        aiBaseUnit.BonusAttackDamage = ReadOffset<float>(_aiBaseUnitOffsets.BonusAttackDamage);
        
        aiBaseUnit.AbilityPower = ReadOffset<float>(_aiBaseUnitOffsets.AbilityPower);
        aiBaseUnit.MagicPenetration = ReadOffset<float>(_aiBaseUnitOffsets.MagicPenetration);
        aiBaseUnit.Lethality = ReadOffset<float>(_aiBaseUnitOffsets.Lethality);
        
        aiBaseUnit.Level = ReadOffset<int>(_aiBaseUnitOffsets.Level);
        
        if (aiBaseUnit.Level is > 30 or < 1)
        {
            aiBaseUnit.Level = 1;
        }
        
        if (!aiBaseUnit.RequireFullUpdate)
        {
            return true;
        }
        
        if (aiBaseUnit.UnitData is not null)
        {
            aiBaseUnit.BasicAttackWindup = aiBaseUnit.UnitData.BasicAttackWindup;
        }
        else
        {
            aiBaseUnit.BasicAttackWindup = 0.3f;
        }
        
        return true;
    }

    public bool ReadAiBaseUnit(IAiBaseUnit? aiBaseUnit, BatchReadContext batchReadContext)
    {
        if (aiBaseUnit is null || !ReadAttackableUnit(aiBaseUnit, batchReadContext))
        {
            return false;
        }

        aiBaseUnit.CurrentTargetIndex = ReadOffset<int>(_aiBaseUnitOffsets.CurrentTargetIndex, batchReadContext);
        aiBaseUnit.BaseAttackRange = ReadOffset<float>(_aiBaseUnitOffsets.AttackRange, batchReadContext);
        aiBaseUnit.BonusAttackSpeed = ReadOffset<float>(_aiBaseUnitOffsets.BonusAttackSpeed, batchReadContext);
        
        aiBaseUnit.BaseAttackDamage = ReadOffset<float>(_aiBaseUnitOffsets.BaseAttackDamage, batchReadContext);
        aiBaseUnit.BonusAttackDamage = ReadOffset<float>(_aiBaseUnitOffsets.BonusAttackDamage, batchReadContext);
        
        aiBaseUnit.AbilityPower = ReadOffset<float>(_aiBaseUnitOffsets.AbilityPower, batchReadContext);
        aiBaseUnit.MagicPenetration = ReadOffset<float>(_aiBaseUnitOffsets.MagicPenetration, batchReadContext);
        aiBaseUnit.Lethality = ReadOffset<float>(_aiBaseUnitOffsets.Lethality, batchReadContext);
        aiBaseUnit.Level = ReadOffset<int>(_aiBaseUnitOffsets.Level, batchReadContext);

        if (aiBaseUnit.Level is > 30 or < 1)
        {
            aiBaseUnit.Level = 1;
        }
        
        if (!aiBaseUnit.RequireFullUpdate)
        {
            return true;
        }
        
        if (aiBaseUnit.UnitData is not null)
        {
            aiBaseUnit.BasicAttackWindup = aiBaseUnit.UnitData.BasicAttackWindup;
        }
        
        return true;
    }
    
    public override int GetBufferSize()
    {
        return Math.Max(base.GetBufferSize(), GetSize(_aiBaseUnitOffsets.GetOffsets()));
    }
}