using System.Numerics;
using System.Text;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class SpellReader : BaseReader, ISpellReader
{
    private readonly ISpellOffsets _spellOffsets;
    private readonly IGameState _gameState;
    private readonly BatchReadContext _spellInputBatchReadContext;
    
    public SpellReader(IMemory memory, ISpellOffsets spellOffsets, IGameState gameState) : base(memory)
    {
        _spellOffsets = spellOffsets;
        _gameState = gameState;
        _spellInputBatchReadContext = new BatchReadContext(GetSize(_spellOffsets.GetSpellInputOffsets()));
    }
    
    public bool ReadSpell(ISpell spell, IntPtr spellPointer)
    {
        spell.Pointer = spellPointer;
        if (!StartRead(spellPointer))
        {
            return false;
        }

        spell.Level = ReadOffset<int>(_spellOffsets.SpellSlotLevel);
        spell.Cooldown = ReadOffset<float>(_spellOffsets.SpellSlotReadyAt) - _gameState.Time;
        spell.SmiteCooldown = ReadOffset<float>(_spellOffsets.SpellSlotSmiteReadyAt) - _gameState.Time;
        spell.Damage = ReadOffset<float>(_spellOffsets.SpellSlotDamage);
        spell.Stacks = ReadOffset<int>(_spellOffsets.SpellSlotSmiteCharges);

        if (Memory.ReadPointer(ReadOffset<IntPtr>(_spellOffsets.SpellSlotSpellInput), out var spellInputPointer))
        {
            spell.SpellInput.Pointer = spellInputPointer;
            if (ReadBuffer(spellInputPointer, _spellInputBatchReadContext))
            {
                spell.SpellInput.SpellInputTargetId =
                    ReadOffset<int>(_spellOffsets.SpellInputTargetId, _spellInputBatchReadContext);
                spell.SpellInput.SpellInputStartPosition =
                    ReadOffset<Vector3>(_spellOffsets.SpellInputStartPosition, _spellInputBatchReadContext);
                spell.SpellInput.SpellInputEndPosition =
                    ReadOffset<Vector3>(_spellOffsets.SpellInputEndPosition, _spellInputBatchReadContext);
            }
        }
        
        if (spell.Cooldown <= 0)
        {
            spell.Cooldown = 0;
        }
        if (spell.SmiteCooldown <= 0)
        {
            spell.SmiteCooldown = 0;
        }

        spell.IsReady = spell is { Cooldown: <= 0, Level: > 0 };
        spell.SmiteIsReady = spell.SmiteCooldown <= 0 || spell.Stacks >= 1;

        if (Memory.ReadPointer(ReadOffset<IntPtr>(_spellOffsets.SpellSlotSpellInfo) + _spellOffsets.SpellInfoSpellData.Offset, out var spellDataPointer))
        {
            if (Memory.ReadPointer(spellDataPointer + _spellOffsets.SpellDataSpellName.Offset,
                    out var spellNamePointer))
            {
                spell.Name = ReadCharArray(spellNamePointer, Encoding.ASCII);
                spell.NameHash = spell.Name.GetHashCode();
            }
            
            //Load
        }
        
        return true;
    }

    protected override BatchReadContext CreateBatchReadContext()
    {
        var size = GetSize(_spellOffsets.GetOffsets());
        return new BatchReadContext(size);
    }

    public override void Dispose()
    {
        base.Dispose();
        _spellInputBatchReadContext.Dispose();
    }
}