using System.Numerics;
using Api.Inputs;
using Api.Menus;
using Api.Settings;
using ImGuiNET;

namespace Api.Internal.Menus;

internal class Hotkey : MenuElement, IHotkey
{
    private bool _isChangingHotKey;
    public VirtualKey VirtualKey { get; set; }
    public HotkeyType HotkeyType { get; set; }
    public bool Enabled { get; private set; }
    
    private static readonly string[] HotkeyTypeNames = Enum.GetNames(typeof(HotkeyType));
    
    public Hotkey(string name, string description, VirtualKey virtualKey, HotkeyType hotkeyType) : base(name, description)
    {
        VirtualKey = virtualKey;
        HotkeyType = hotkeyType;
    }

    public override void ProcessKey(VirtualKey virtualKey, KeyState keyState)
    {
        switch (keyState)
        {
            case KeyState.KeyDown:
                KeyDown(virtualKey);
                break;
            case KeyState.KeyUp:
                KeyUp(virtualKey);
                break;
            case KeyState.Unknown:
            default:
                break;
        }
    }
    
    public void KeyDown(VirtualKey virtualKey)
    {
        if (_isChangingHotKey)
        {
            return;
        }
        
        switch (HotkeyType)
        {
            case HotkeyType.Toggle:
            {
                if (virtualKey == VirtualKey)
                {
                    Enabled = !Enabled;
                }

                break;
            }
            case HotkeyType.Press:
            {
                if (virtualKey == VirtualKey)
                {
                    Enabled = true;
                }

                break;
            }
        }
    }

    public void KeyUp(VirtualKey virtualKey)
    {
        if (_isChangingHotKey)
        {
            VirtualKey = virtualKey;
            _isChangingHotKey = false;
            return;
        }
        
        if (HotkeyType == HotkeyType.Press)
        {
            if (virtualKey == VirtualKey)
            {
                Enabled = false;
            }
        }
    }

    protected override void RenderElement()
    {
        ImGui.AlignTextToFramePadding(); 
        ImGui.Text("Name");
        ImGui.SameLine();
        var buttonWidth = 100.0f;
        var buttonHeight = 20.0f;
        
        if (ImGui.Button($"{VirtualKey}##{Name}{Id}", new Vector2(buttonWidth, buttonHeight)))
        {
            VirtualKey = VirtualKey.SelectKey;
            _isChangingHotKey = true;
        }
        
        ImGui.SameLine();
        
        ImGui.SetNextItemWidth(buttonWidth);
        var selectedIndex = (int)HotkeyType;
        if(ImGui.Combo($"Type##{Name}{Id}", ref selectedIndex, HotkeyTypeNames, HotkeyTypeNames.Length))
        {
            HotkeyType = (HotkeyType)selectedIndex;
        }
    }
    
    public override void LoadSettings(ISettingsProvider settingsProvider)
    {
        if (settingsProvider.ReadValue<VirtualKey>($"{SaveId}.{VirtualKey}", out var virtualKey))
        {
            VirtualKey = virtualKey;
        }
        if (settingsProvider.ReadValue<HotkeyType>($"{SaveId}.{HotkeyType}", out var hotkeyType))
        {
            HotkeyType = hotkeyType;
        }
        if (settingsProvider.ReadValue<bool>($"{SaveId}.{Enabled}", out var enabled))
        {
            Enabled = enabled;
        }
    }

    public override void SaveSettings(ISettingsProvider settingsProvider)
    {
        settingsProvider.SetValue($"{SaveId}.{nameof(VirtualKey)}", VirtualKey);
        settingsProvider.SetValue($"{SaveId}.{nameof(HotkeyType)}", HotkeyType);
        settingsProvider.SetValue($"{SaveId}.{nameof(Enabled)}", Enabled);
    }
}