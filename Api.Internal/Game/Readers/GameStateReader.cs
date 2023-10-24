using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class GameStateReader : IGameStateReader
{
    private readonly IMemory _memory;
    private readonly IBaseOffsets _baseOffsets;
    
    public GameStateReader(IMemory memory, IBaseOffsets baseOffsets)
    {
        _memory = memory;
        _baseOffsets = baseOffsets;
    }
    public void ReadGameState(IGameState gameState)
    {
        if (_memory.ReadModule<float>(_baseOffsets.GameTime, out var time))
        {
            gameState.Time = time;
        }
        else
        {
            gameState.Time = 0;
        }
    }
}