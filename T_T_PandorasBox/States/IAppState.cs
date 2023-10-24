using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace T_T_PandorasBox.States;

internal interface IAppState : IDisposable
{
    void EnterState();
    void LeaveState();
    void WindowOnLoad(IWindow window, GL gl);
    void Update(float deltaTime);
    void Render(float deltaTime);
    void ImGuiRender(float deltaTime);
    WindowOptions GetWindowOptions();
}