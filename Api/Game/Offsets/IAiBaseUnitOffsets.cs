﻿namespace Api.Game.Offsets;

public interface IAiBaseUnitOffsets
{
    OffsetData CurrentTargetIndex { get; }
    OffsetData AttackRange { get; }
    OffsetData BonusAttackSpeed { get; }
    OffsetData BaseAttackDamage { get; }
    OffsetData BonusAttackDamage { get; }
    OffsetData AbilityPower { get; }
    OffsetData MagicPenetration { get; }
    OffsetData Lethality { get; }
    OffsetData Level { get; set; }
    IEnumerable<OffsetData> GetOffsets();
}