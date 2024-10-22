﻿using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Api.Game.Data;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class MissileReader : BaseReader, IMissileReader
{
    private readonly IMissileOffsets _missileOffsets;
    private readonly BatchReadContext _missileSpellInfoBatchReadContext;
    private readonly ILocalPlayer _localPlayer;
    private readonly SpellDataDictionary _spellDataDictionary;

    public MissileReader(
        IMemory memory,
        IMissileOffsets missileOffsets,
        ILocalPlayer localPlayer,
        SpellDataDictionary spellDataDictionary) : base(memory)
    {
        _missileOffsets = missileOffsets;
        _localPlayer = localPlayer;
        _spellDataDictionary = spellDataDictionary;

        _missileSpellInfoBatchReadContext = new BatchReadContext(GetSize(_missileOffsets.GetSpellInfoOffsets()));
    }

    public bool ReadMissile(IMissile? missile)
    {
        if (missile is null || missile.Pointer == IntPtr.Zero)
        {
            return false;
        }

        if (!StartRead(missile))
        {
            return false;
        }
        
        missile.NetworkId = ReadOffset<int>(_missileOffsets.NetworkId);
        missile.Name = ReadString(_missileOffsets.Name, Encoding.ASCII);
        missile.NameHash = missile.Name.GetHashCode();
        missile.Position = ReadOffset<Vector3>(_missileOffsets.Position);
        
        //propably should be read from spell data we dont want to do a lot of memory reads to with plenty of missiles
        //missile.Speed = ReadOffset<float>(_missileOffsets.Speed);
        
        var spellInfoPtr = ReadOffset<IntPtr>(_missileOffsets.SpellInfo);
        if (!ReadBuffer(spellInfoPtr, _missileSpellInfoBatchReadContext))
        {
            missile.IsValid = false;
            return false;
        }
        
        missile.StartPosition = ReadOffset<Vector3>(_missileOffsets.StartPosition);
        missile.EndPosition = ReadOffset<Vector3>(_missileOffsets.EndPosition);
        
        missile.SpellName = ReadString(_missileOffsets.SpellInfoSpellName, Encoding.ASCII, _missileSpellInfoBatchReadContext);
        missile.MissileName = ReadString(_missileOffsets.SpellInfoMissileName, Encoding.ASCII, _missileSpellInfoBatchReadContext);
        
        missile.SpellData = _spellDataDictionary[missile.SpellName.GetHashCode()];
        if (missile.SpellData?.MissileData != null)
        {
            missile.MissileData = missile.SpellData.MissileData;
            missile.Speed = missile.MissileData.Speed;
            missile.Width = missile.MissileData.Width;
        }
        else
        {
            missile.Speed = 650;
            missile.Width = 80;
        }
        
        missile.SourceIndex = ReadOffset<int>(_missileOffsets.SourceIndex);
        
        var destinationPtr = ReadOffset<IntPtr>(_missileOffsets.DestinationIndex);
        if (destinationPtr.ToInt64() > 0x1000 && Memory.Read<int>(destinationPtr, out var destinationIndex))
        {
            missile.DestinationIndex = destinationIndex;
        }
        else
        {
            missile.DestinationIndex = 0;
        }

        return true;
    }
    
    
    protected override BatchReadContext CreateBatchReadContext()
    {
        var size = GetSize(_missileOffsets.GetOffsets());
        return new BatchReadContext(size);
    }

    public override void Dispose()
    {
        base.Dispose();
        _missileSpellInfoBatchReadContext.Dispose();
    }
}