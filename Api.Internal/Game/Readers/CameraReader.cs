using System.Numerics;
using System.Runtime.InteropServices;
using Api.Game.Objects;
using Api.Game.Offsets;
using Api.Game.Readers;
using Api.GameProcess;

namespace Api.Internal.Game.Readers;

internal class GameCameraReader : IGameCameraReader
{
	private readonly IMemory _memory;
	private readonly IGameCameraOffsets _gameCameraOffsets;
	private readonly BatchReadContext _matrixBatchReadContext;
	private readonly BatchReadContext _doubleIntBatchReadContext;
	private readonly int _matrixSize;
	private readonly int _intSize;
	
	public GameCameraReader(IMemory memory, IGameCameraOffsets gameCameraOffsets)
	{
		_memory = memory;
		_gameCameraOffsets = gameCameraOffsets;
		_matrixSize = Marshal.SizeOf<Matrix4x4>();
		_intSize = Marshal.SizeOf<int>();
		_matrixBatchReadContext = new BatchReadContext(_matrixSize * 2);
		_doubleIntBatchReadContext = new BatchReadContext(_intSize * 2);
	}
	
    public bool ReadCamera(IGameCamera? gameCamera)
    {
        if (gameCamera is null)
        {
            return false;
        }

        if (!ReadMatrices(gameCamera))
        {
	        gameCamera.IsValid = false;
	        return false;
        }

        if (!ReadSize(gameCamera))
        {
	        gameCamera.IsValid = false;
	        return false;
        }
        
        return true;
    }

    private bool ReadSize(IGameCamera gameCamera)
    {
	    if (!gameCamera.RequireFullUpdate)
	    {
		    return true;
	    }
	    
	    if(!_memory.ReadModulePointer(_gameCameraOffsets.Renderer.Offset, out var rendererPtr))
	    {
		    return false;
	    }
	    
	    if (!_memory.ReadCachedBuffer(rendererPtr + _gameCameraOffsets.RendererWidth.Offset, _doubleIntBatchReadContext))
	    {
		    return false;
	    }

	    gameCamera.RendererWidth = _doubleIntBatchReadContext.Read<int>(0);
	    gameCamera.RendererHeight = _doubleIntBatchReadContext.Read<int>(_intSize);
	    gameCamera.RequireFullUpdate = false;
	    
	    return true;
    }
    
    private bool ReadMatrices(IGameCamera gameCamera)
    {
	    if (!_memory.ReadModuleCachedBuffer(_gameCameraOffsets.ViewProjMatrix.Offset, _matrixBatchReadContext))
	    {
		    return false;
	    }

	    var viewMatrix = _matrixBatchReadContext.Read<Matrix4x4>(0);
	    var projMatrix = _matrixBatchReadContext.Read<Matrix4x4>(_matrixSize);

	    gameCamera.ViewProjMatrix = viewMatrix * projMatrix;

	    return true;
    }
}