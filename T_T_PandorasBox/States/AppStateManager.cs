using System.Runtime.InteropServices;
using Api.GameProcess;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace T_T_PandorasBox.States;

internal class AppStateManager
{
    /// <summary>
    /// IDK BUT WINDOWS BETWEEN CREATION AND CLOSE NEED TIME OTHERWHISE SILK.NET GETS FUCKED
    /// </summary>
    private const int DelayBetweenWindows = 5;

    private IWindow? _window;
    private IInputContext? _inputContext;
    private ImGuiController? _imGuiController;
    private GL? _gl;
    
    private IAppState? _currentState;
    private bool _isChangingState = false;
    private DateTime _startChange = DateTime.Now;
    private readonly ITargetProcess _targetProcess;
    private readonly MainAppState _mainAppState;
    private readonly InGameAppState _inGameAppState;
    
    public bool ShouldExit { get; private set; } = false;
    
    public AppStateManager(ITargetProcess targetProcess,
        MainAppState mainAppState,
        InGameAppState inGameAppState)
    {
        _targetProcess = targetProcess;
        _mainAppState = mainAppState;
        _inGameAppState = inGameAppState;
    }

    private void CreateWindow()
    {
        if(_currentState is null) return;
        
        _window = Window.Create(_currentState.GetWindowOptions());
        _window.Load += WindowOnLoad;
        _window.Update += OnUpdate;
        _window.Render += WindowOnRender;
        _window.Closing += WindowClosing;
        _window.Run();
    }

    private void WindowClosing()
    {
        if (_currentState == _mainAppState && !_targetProcess.IsRunning)
        {
            ShouldExit = true;
        }
    }

    private void WindowOnLoad()
    {
        if(_window == null) return;
        _inputContext = _window.CreateInput();
        _gl = _window.CreateOpenGL();
        _gl.ClearColor(0, 0, 0, 0);

        _imGuiController = new ImGuiController(_gl, _window, _inputContext);
        _currentState?.WindowOnLoad(_window, _gl);
        _currentState?.EnterState();
    }
    
    public void Update()
    {
        if (_isChangingState)
        {
            var timeDifference = DateTime.Now - _startChange;
            if (timeDifference.Seconds >= DelayBetweenWindows)
            {
                _isChangingState = false;
                CreateWindow();
            }
            return;
        }
        
        if (_targetProcess.IsRunning)
        {
            SetState(_inGameAppState);
        }
        else
        {
            SetState(_mainAppState);
        }
    }
    
    private void OnUpdate(double dt)
    {
        if (_isChangingState)
        {
            return;
        }
        
        var deltaTime = Convert.ToSingle(dt);
        _targetProcess.Update(deltaTime);
        
        if (_targetProcess.IsRunning)
        {
            SetState(_inGameAppState);
        }
        else
        {
            SetState(_mainAppState);
        }
        
        if (_isChangingState)
        {
            return;
        }
        
        _imGuiController?.Update(deltaTime);
        _currentState?.Update(deltaTime);
    }
    
    private void WindowOnRender(double dt)
    {
        if(_currentState is null || _isChangingState) return;
        
        var deltaTime = (float)dt;
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (_imGuiController != null)
        {
            _currentState.ImGuiRender(deltaTime);
            _imGuiController.Render();
        }

        _gl.Enable(EnableCap.Blend);
        _gl.Enable(EnableCap.Multisample);
        //_gl.Disable(EnableCap.CullFace);
            
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _currentState.Render(deltaTime);
            
        //_gl.Enable(EnableCap.CullFace);
        _gl.Disable(EnableCap.Multisample);
        _gl.Disable(EnableCap.Blend);
        
        //Render(Convert.ToSingle(deltaTime));
    }

    public void Run()
    {
        _targetProcess.Update(0);
        Update();
    }

    private void SetState(IAppState appState)
    {
        if (appState == _currentState) return;
        if (_currentState != null)
        {
            _currentState?.LeaveState();
            _window?.Close();
            _isChangingState = true;
        }
        
        _currentState = appState;
        _startChange = DateTime.Now;
        
        if (!_isChangingState)
        {
            CreateWindow();
        }
    }
}