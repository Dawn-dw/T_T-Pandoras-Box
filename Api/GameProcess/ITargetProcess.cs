namespace Api.GameProcess;

public interface ITargetProcess : IDisposable
{
    int Id { get; }
    string ProcessName { get; }
    IntPtr Handle { get; }
    IntPtr ModuleBase { get; }
    bool IsRunning { get; }
    bool Attached { get; }
    TargetProcessData TargetProcessData { get; }
    bool Attach();
    void Detach();
    void Update(float deltaTime);
}