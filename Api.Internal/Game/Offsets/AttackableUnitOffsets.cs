using System.Numerics;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Internal.Game.Readers;
using Microsoft.Extensions.Configuration;

namespace Api.Internal.Game.Offsets;

internal class AttackableUnitOffsets : IAttackableUnitOffsets
{
    public OffsetData IsDead { get; }
    public OffsetData Mana { get; }
    public OffsetData MaxMana { get; }
    public OffsetData Health { get; }
    public OffsetData MaxHealth { get; }
    public OffsetData Armor { get; }
    public OffsetData BonusArmor { get; }
    public OffsetData MagicResistance { get; }
    public OffsetData BonusMagicResistance { get; }
    public OffsetData Targetable { get; }
    
    public AttackableUnitOffsets(IConfiguration configuration)
    {
        var cs = configuration.GetSection(nameof(AttackableUnitOffsets));
            
        IsDead = new OffsetData(nameof(IsDead), Convert.ToInt32(cs[nameof(IsDead)], 16), typeof(ObfuscatedBool));
        Mana = new OffsetData(nameof(Mana), Convert.ToInt32(cs[nameof(Mana)], 16), typeof(float));
        MaxMana = new OffsetData(nameof(MaxMana), Convert.ToInt32(cs[nameof(MaxMana)], 16), typeof(float));
        Health = new OffsetData(nameof(Health), Convert.ToInt32(cs[nameof(Health)], 16), typeof(float));
        MaxHealth = new OffsetData(nameof(MaxHealth), Convert.ToInt32(cs[nameof(MaxHealth)], 16), typeof(float));
        Armor = new OffsetData(nameof(Armor), Convert.ToInt32(cs[nameof(Armor)], 16), typeof(float));
        BonusArmor = new OffsetData(nameof(BonusArmor), Convert.ToInt32(cs[nameof(BonusArmor)], 16), typeof(float));
        MagicResistance = new OffsetData(nameof(MagicResistance), Convert.ToInt32(cs[nameof(MagicResistance)], 16), typeof(float));
        BonusMagicResistance = new OffsetData(nameof(BonusMagicResistance), Convert.ToInt32(cs[nameof(BonusMagicResistance)], 16), typeof(float));
        Targetable = new OffsetData(nameof(Targetable), Convert.ToInt32(cs[nameof(Targetable)], 16), typeof(bool));
    }
        
    public IEnumerable<OffsetData> GetOffsets()
    {
        yield return IsDead;
        yield return Mana;
        yield return MaxMana;
        yield return Health;
        yield return MaxHealth;
        yield return Armor;
        yield return BonusArmor;
        yield return MagicResistance;
        yield return BonusMagicResistance;
        yield return Targetable;
    }
}