using System.Numerics;
using System.Text;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

public class ActiveCastSpellReader : BaseReader, IActiveCastSpellReader
{
    private readonly IActiveCastSpellOffsets _activeCastSpellOffsets;
    
    public ActiveCastSpellReader(
        IMemory memory,
        IActiveCastSpellOffsets activeCastSpellOffsets) : base(memory)
    {
        _activeCastSpellOffsets = activeCastSpellOffsets;
    }
    
    public bool ReadSpell(IActiveCastSpell spell, IntPtr spellPointer)
    {
        spell.Pointer = spellPointer;
        if (spellPointer.ToInt64() <= 0x1000)
        {
            spell.IsActive = false;
            return false;
        }
        if (!StartRead(spellPointer))
        {
            spell.IsActive = false;
            return false;
        }
        
        spell.IsActive = true;

        var type = ReadOffset<sbyte>(_activeCastSpellOffsets.Type);
        if (type is >= -1 and <= 3)
        {
            spell.Type = (ActiveSpellType)type;
        }
        else
        {
            spell.IsActive = false;
            spell.Type = ActiveSpellType.Unknown;
            return false;
        }

        spell.SourceId = ReadOffset<int>(_activeCastSpellOffsets.SourceId);

        if (Memory.Read<int>(ReadOffset<IntPtr>(_activeCastSpellOffsets.TargetId), out var targetId))
        {
            spell.TargetId = targetId;
        }
        else
        {
            spell.TargetId = 0;
        }

        spell.StartPosition = ReadOffset<Vector3>(_activeCastSpellOffsets.StartPosition);
        spell.EndPosition = ReadOffset<Vector3>(_activeCastSpellOffsets.EndPosition);

        spell.StartTime = ReadOffset<float>(_activeCastSpellOffsets.StartTime);
        spell.EndTime = ReadOffset<float>(_activeCastSpellOffsets.EndTime);

        var spellInfo = ReadOffset<IntPtr>(_activeCastSpellOffsets.SpellInfo);
        spell.Name = ReadString(spellInfo + _activeCastSpellOffsets.SpellInfoName.Offset, Encoding.ASCII);
        
        return true;
    }
    
    protected override BatchReadContext CreateBatchReadContext()
    {
        var size = GetSize(_activeCastSpellOffsets.GetOffsets());
        return new BatchReadContext(size);
    }
}