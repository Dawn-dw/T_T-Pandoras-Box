using Api.Game.Objects;
using Api.GameProcess;

namespace Api.Game.Readers;

public interface ITrapReader : IGameObjectReader
{
    bool ReadTrap(ITrap? trap);
    bool ReadTrap(ITrap? trap, BatchReadContext batchReadContext);
}