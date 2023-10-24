using Api.Game.Data;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class AttackableUnitReader : GameObjectReader, IAttackableUnitReader
{
    private readonly IAttackableUnitOffsets _attackableUnitOffsets;
    protected readonly UnitDataDictionary UnitDataDictionary;
    
    public AttackableUnitReader(
        IMemory memory,
        IGameObjectOffsets gameObjectOffsets,
        IAttackableUnitOffsets attackableUnitOffsets,
        UnitDataDictionary unitDataDictionary) : base(memory, gameObjectOffsets)
    {
        _attackableUnitOffsets = attackableUnitOffsets;
        UnitDataDictionary = unitDataDictionary;
    }

    public bool ReadAttackableUnit(IAttackableUnit? attackableUnit)
    {
        if (attackableUnit is null || !ReadObject(attackableUnit))
        {
            return false;
        }

        // var isDeadObfuscation = ReadOffset<ObfuscatedBool>(_attackableUnitOffsets.IsDead);
        // try
        // {
        //     attackableUnit.IsDead = isDeadObfuscation.Deobfuscate();
        // }
        // catch (Exception ex)
        // {
        //     attackableUnit.IsDead = false;
        // }

        attackableUnit.IsDead = false;
        attackableUnit.Mana = ReadOffset<float>(_attackableUnitOffsets.Mana);
        attackableUnit.MaxMana = ReadOffset<float>(_attackableUnitOffsets.MaxMana);
        attackableUnit.Health = ReadOffset<float>(_attackableUnitOffsets.Health);
        attackableUnit.MaxHealth = ReadOffset<float>(_attackableUnitOffsets.MaxHealth);
        attackableUnit.Armor = ReadOffset<float>(_attackableUnitOffsets.Armor);
        attackableUnit.BonusArmor = ReadOffset<float>(_attackableUnitOffsets.BonusArmor);
        attackableUnit.MagicResistance = ReadOffset<float>(_attackableUnitOffsets.MagicResistance);
        attackableUnit.BonusMagicResistance = ReadOffset<float>(_attackableUnitOffsets.BonusMagicResistance);
        attackableUnit.Targetable = ReadOffset<bool>(_attackableUnitOffsets.Targetable);

        if (!attackableUnit.RequireFullUpdate)
        {
            return true;
        }
        
        var unitData = UnitDataDictionary[attackableUnit.ObjectNameHash];
        if (unitData is not null)
        {
            attackableUnit.UnitData = unitData;
            attackableUnit.CollisionRadius = unitData.GameplayCollisionRadius;
        }
        else
        {
            attackableUnit.CollisionRadius = 65.0f;
        }
        
        return true;
    }

    public bool ReadAttackableUnit(IAttackableUnit? attackableUnit, BatchReadContext batchReadContext)
    {
        if (attackableUnit is null || !ReadObject(attackableUnit, batchReadContext))
        {
            return false;
        }

        // var isDeadObfuscation = ReadOffset<ObfuscatedBool>(_attackableUnitOffsets.IsDead);
        // attackableUnit.IsDead = isDeadObfuscation.Deobfuscate();
        attackableUnit.IsDead = false;
        attackableUnit.Mana = ReadOffset<float>(_attackableUnitOffsets.Mana, batchReadContext);
        attackableUnit.MaxMana = ReadOffset<float>(_attackableUnitOffsets.MaxMana, batchReadContext);
        attackableUnit.Health = ReadOffset<float>(_attackableUnitOffsets.Health, batchReadContext);
        attackableUnit.MaxHealth = ReadOffset<float>(_attackableUnitOffsets.MaxHealth, batchReadContext);
        attackableUnit.Armor = ReadOffset<float>(_attackableUnitOffsets.Armor, batchReadContext);
        attackableUnit.BonusArmor = ReadOffset<float>(_attackableUnitOffsets.BonusArmor, batchReadContext);
        attackableUnit.MagicResistance = ReadOffset<float>(_attackableUnitOffsets.MagicResistance, batchReadContext);
        attackableUnit.BonusMagicResistance = ReadOffset<float>(_attackableUnitOffsets.BonusMagicResistance, batchReadContext);
        attackableUnit.Targetable = ReadOffset<bool>(_attackableUnitOffsets.Targetable, batchReadContext);

        if (!attackableUnit.RequireFullUpdate)
        {
            return true;
        }
        
        var unitData = UnitDataDictionary[attackableUnit.ObjectNameHash];
        if (unitData is not null)
        {
            attackableUnit.UnitData = unitData;
            attackableUnit.CollisionRadius = unitData.GameplayCollisionRadius;
        }
        else
        {
            attackableUnit.CollisionRadius = 65.0f;
        }
        
        return true;
    }

    public override int GetBufferSize()
    {
        return Math.Max(base.GetBufferSize(), GetSize(_attackableUnitOffsets.GetOffsets()));
    }
}