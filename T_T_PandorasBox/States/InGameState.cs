using Api;
using Api.Game.Managers;
using Api.Menus;
using Api.Scripts;
using Api.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using T_T_PandorasBox.Extensions;
using WinApi;

namespace T_T_PandorasBox.States;

internal sealed class InGameAppState : IAppState
{
    private bool _isActive;
    private IWindow _window;
    private readonly IRandomGenerator _randomGenerator;
    private readonly IRenderer _renderer;
    private readonly IMainMenu _mainMenu;
    private readonly IGameManager _gameManager;
    private readonly IScriptManager _scriptManager;
    
    public InGameAppState(
        IRandomGenerator randomGenerator,
        IRenderer renderer,
        IMainMenu mainMenu,
        IGameManager gameManager,
        IScriptManager scriptManager)
    {
        _randomGenerator = randomGenerator;
        _renderer = renderer;
        _mainMenu = mainMenu;
        _gameManager = gameManager;
        _scriptManager = scriptManager;

        _mainMenu.MenuOpen += () => _window?.SetWindowExNotTransparent();
        _mainMenu.MenuClose += () => _window?.SetWindowExTransparent();
    }

    public void EnterState()
    {
        _isActive = true;
    }

    public void LeaveState()
    {
        _isActive = false;
        _renderer.Unload();
        _scriptManager.Unload();
        _gameManager.Unload();
    }

    public void WindowOnLoad(IWindow window, GL gl)
    {
        _window = window;
        _renderer.Init(gl);
        _window.SetWindowExTransparent();
        _scriptManager.LoadScripts();
    }

    public void Update(float deltaTime)
    {
        if(!_isActive) return;
        _gameManager.Update(deltaTime);
        _scriptManager.Update(deltaTime);
    }

    public void Render(float deltaTime)
    {
        if(!_isActive) return;
        _scriptManager.Render(deltaTime);
    }

    public void ImGuiRender(float deltaTime)
    {
        if(!_isActive) return;
        _mainMenu.Render();
    }

    public WindowOptions GetWindowOptions()
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.TransparentFramebuffer = true;
        windowOptions.FramesPerSecond = 60;
        
        windowOptions.API = new GraphicsAPI
        {
            API = ContextAPI.OpenGL,
            Profile = ContextProfile.Core,
            Version = new APIVersion(4, 6),
            Flags = ContextFlags.ForwardCompatible
        };

        windowOptions.Title = _randomGenerator.GetRandomString(8, 12);
            
        var size = WindowsApi.GetVirtualDisplaySize();
        windowOptions.Size = new Vector2D<int>(size.Width-1, size.Height-1);

        windowOptions.TopMost = true;
        windowOptions.WindowBorder = WindowBorder.Hidden;
        windowOptions.VSync = true;
        windowOptions.Position = new Vector2D<int>(0, 0);
        windowOptions.WindowState = WindowState.Normal;
            
        windowOptions.PreferredStencilBufferBits = 8;
        windowOptions.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);
        windowOptions.Samples = 4;
        windowOptions.PreferredDepthBufferBits = 24;
            
        return windowOptions;
    }

    public void Dispose()
    {
        _renderer.Unload();
        _scriptManager.Unload();
        _gameManager.Unload();
    }

    ~InGameAppState() => Dispose();
}