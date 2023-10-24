using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class TrapReader : GameObjectReader, ITrapReader
{
    public TrapReader(IMemory memory, IGameObjectOffsets gameObjectOffsets) : base(memory, gameObjectOffsets)
    {
    }

    public bool ReadTrap(ITrap? trap)
    {
        if (trap is null || !ReadObject(trap))
        {
            return false;
        }

        return true;
    }

    public bool ReadTrap(ITrap? trap, BatchReadContext batchReadContext)
    {
        if (trap is null || !ReadObject(trap, batchReadContext))
        {
            return false;
        }

        return true;
    }
}