using Api.Inputs;

namespace Api.Menus;

public interface ISubMenu : IMenuElement
{
    public void AddElement(IMenuElement menuElement);
    ISubMenu AddSubMenu(string name, string description);
    IToggle AddToggle(string name, string description, bool toggled);
    IValueSlider AddValueSlider(string name, string description, float value, float min, float max);
    IHotkey AddHotkey(string name, string description, VirtualKey virtualKey, HotkeyType hotkeyType);
    IComboBox AddComboBox(string name, string description, int selectedIndex, string[] values);
    IEnumComboBox<T> AddEnumComboBox<T>(string name, string description, T selectedItem) where T : Enum;
}