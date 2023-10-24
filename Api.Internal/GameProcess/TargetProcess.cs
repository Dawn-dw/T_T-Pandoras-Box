using System.Diagnostics;
using Api.GameProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WinApi;

namespace Api.Internal.GameProcess;

public class TargetProcess : ITargetProcess
{
    private float _lastCheckTimer = 0;
    private readonly ILogger<TargetProcess> _logger;

    public int Id { get; private set; }
    public string ProcessName { get; private set; } = string.Empty;
    public IntPtr Handle { get; private set; }
    public IntPtr ModuleBase { get; private set; }
    public IntPtr ModuleEndAddress { get; private set; }
    public bool IsRunning { get; private set; }
    public bool Attached => Handle != IntPtr.Zero;
    public TargetProcessData TargetProcessData { get; }

    public TargetProcess(ILogger<TargetProcess> logger, IOptions<TargetProcessData> targetProcessData)
    {
        _logger = logger;
        TargetProcessData = targetProcessData.Value;
    }
    
    public bool Attach()
    {
        var processName = TargetProcessData.Name;
        var module = TargetProcessData.Module;
        
        var targetProcess = Process.GetProcessesByName(processName).FirstOrDefault();
        if (targetProcess is null) return false;
        
        Id = targetProcess.Id;
        Handle = WindowsApi.OpenProcess(WindowsApi.ProcessAccessRights.ProcessVmRead | WindowsApi.ProcessAccessRights.ProcessQueryInformation, false, Id);

        var mainModule = GetModule(targetProcess, module);
        if (mainModule != null)
        {
            ModuleBase = mainModule.BaseAddress;
            ModuleEndAddress = ModuleBase + mainModule.ModuleMemorySize;
        }

        _logger.LogInformation("Attached to process {processName}, id: {Id}, moduleBase: {ModuleBase}", processName, Id, ModuleBase);
        
        IsRunning = Handle != IntPtr.Zero;
        
        return IsRunning;
    }

    private ProcessModule? GetModule(Process process, string moduleName)
    {
        return process.Modules.Cast<ProcessModule>().FirstOrDefault(module => module.ModuleName == moduleName);
    }
    
    public void Detach()
    {
        WindowsApi.CloseHandle(Handle);
        Handle = IntPtr.Zero;
        Id = 0;
        IsRunning = false;
    }

    public void Update(float deltaTime)
    {
        if (_lastCheckTimer > 0.5f)
        {
            var isRunning = Process.GetProcesses().Any(p => p.Id == Id);

            switch (Attached)
            {
                case true when !isRunning:
                    Detach();
                    break;
                case false when isRunning:
                    Attach();
                    break;
            }
            
            _lastCheckTimer = 0;
        }

        _lastCheckTimer += deltaTime;
    }

    public void Dispose()
    {
        if (Attached)
        {
            Detach();
        }
    }
}