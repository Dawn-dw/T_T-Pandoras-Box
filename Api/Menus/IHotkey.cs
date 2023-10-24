using Api.Inputs;

namespace Api.Menus;

public enum HotkeyType
{
    Toggle = 0,
    Press = 1
}

public interface IHotkey : IMenuElement
{
    VirtualKey VirtualKey { get; set; }
    HotkeyType HotkeyType { get; set; } 
    bool Enabled { get; }
    void KeyDown(VirtualKey virtualKey);
    void KeyUp(VirtualKey virtualKey);
}