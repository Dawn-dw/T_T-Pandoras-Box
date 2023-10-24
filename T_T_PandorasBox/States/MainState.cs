using System.Numerics;
using Api.Utils;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using T_T_PandorasBox.States.MainWindowViews;
using WinApi;

namespace T_T_PandorasBox.States;

internal sealed class MainAppState : IAppState
{
    private readonly Vector2D<int> _windowSize = new Vector2D<int>(800, 600);
    private readonly IRandomGenerator _randomGenerator;
    private readonly IEnumerable<IMainWindowView> _mainWindowViews;

    public MainAppState(IRandomGenerator randomGenerator, IEnumerable<IMainWindowView> mainWindowViews)
    {
        _randomGenerator = randomGenerator;
        _mainWindowViews = mainWindowViews;
    }

    public void EnterState()
    {
    }

    public void LeaveState()
    {
    }

    public void WindowOnLoad(IWindow window, GL gl)
    {
    }

    public void Update(float deltaTime)
    {
    }

    public void Render(float deltaTime)
    {
    }

    public void ImGuiRender(float deltaTime)
    {
        ImGui.SetNextWindowSize(new Vector2(_windowSize.X, _windowSize.Y));
        ImGui.SetNextWindowPos(new Vector2(0, 0));
        
        if (!ImGui.Begin("T_T Pandora's Box Launcher", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar))
        {
            ImGui.End();
            return;
        }

        ImGui.TextColored(new Vector4(0f, 1.0f, 0.0f, 1.0f), "Status: Waiting for game.");

        if (ImGui.BeginTabBar($"MainWindowTabBar", ImGuiTabBarFlags.None))
        {
            foreach (var mainWindowView in _mainWindowViews)
            {
                if (ImGui.BeginTabItem(mainWindowView.Name))
                {
                    mainWindowView.Render(deltaTime);
                    ImGui.EndTabItem();
                }
            }
            
            ImGui.EndTabBar();
        }
        
        ImGui.End();
    }

    public void Dispose()
    {
    }

    public WindowOptions GetWindowOptions()
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.TransparentFramebuffer = false;
        windowOptions.FramesPerSecond = 60;
        windowOptions.API = new GraphicsAPI
        {
            API = ContextAPI.OpenGL,
            Profile = ContextProfile.Core,
            Version = new APIVersion(4, 6),
            Flags = ContextFlags.ForwardCompatible
        };
        windowOptions.Title = _randomGenerator.GetRandomString(8, 12);
            
        windowOptions.Size = new Vector2D<int>(800, 600);

        windowOptions.TopMost = false;
        windowOptions.WindowBorder = WindowBorder.Fixed;
        windowOptions.VSync = true;
        windowOptions.WindowState = WindowState.Normal;
            
        windowOptions.PreferredStencilBufferBits = 8;
        windowOptions.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);
        windowOptions.Samples = 4;
        windowOptions.PreferredDepthBufferBits = 24;
            
        return windowOptions;
    }
}