using Api.Inputs;
using Api.Menus;
using Api.Scripts;

namespace Scripts;

public class ScriptingState : IScriptingState
{
    public VirtualKey ComboKey { get; set; } = VirtualKey.Spacebar;
    public VirtualKey HarasKey { get; set; } = VirtualKey.C;
    public VirtualKey FarmKey { get; set; } = VirtualKey.X;
    public VirtualKey ClearKey { get; set; } = VirtualKey.V;
    public ActionType ActionType { get; private set; } = ActionType.None;
    
    public bool IsCombo => (ActionType & ActionType.Combo) == ActionType.Combo;
    public bool IsHaras => (ActionType & ActionType.Haras) == ActionType.Haras;
    public bool IsFarm => (ActionType & ActionType.Farm) == ActionType.Farm;
    public bool IsClear => (ActionType & ActionType.Clear) == ActionType.Clear;

    public ScriptingState(IInputManager inputManager)
    {
        inputManager.KeyDown += InputManagerOnKeyDown;
        inputManager.KeyUp += InputManagerOnKeyUp;
    }

    private void InputManagerOnKeyDown(VirtualKey virtualKey)
    {
        if (virtualKey == ComboKey)
        {
            ActionType |= ActionType.Combo;
        }
        else if (virtualKey == HarasKey)
        {
            ActionType |= ActionType.Haras;
        }
        else if (virtualKey == FarmKey)
        {
            ActionType |= ActionType.Farm;
        }
        else if (virtualKey == ClearKey)
        {
            ActionType |= ActionType.Clear;
        }
    }

    private void InputManagerOnKeyUp(VirtualKey virtualKey)
    {
        if (virtualKey == ComboKey)
        {
            ActionType &= ~ActionType.Combo;
        }
        else if (virtualKey == HarasKey)
        {
            ActionType &= ~ActionType.Haras;
        }
        else if (virtualKey == FarmKey)
        {
            ActionType &= ~ActionType.Farm;
        }
        else if (virtualKey == ClearKey)
        {
            ActionType &= ~ActionType.Clear;
        }
    }
}