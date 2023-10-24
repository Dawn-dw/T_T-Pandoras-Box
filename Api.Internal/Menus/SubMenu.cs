using Api.Inputs;
using Api.Menus;
using Api.Settings;
using ImGuiNET;

namespace Api.Internal.Menus;

internal class SubMenu : MenuElement, ISubMenu
{
    private readonly List<IMenuElement> _elements = new();
    public SubMenu(string name, string description) : base(name, description)
    {
    }

    protected override void RenderElement()
    {
        if (ImGui.CollapsingHeader(ImGuiId))
        {
            foreach (var element in _elements)
            {
                element.Render();
            }
        }
    }

    public void AddElement(IMenuElement menuElement)
    {
        menuElement.Id = $"{Id}.{Name}";
        _elements.Add(menuElement);
    }

    public ISubMenu AddSubMenu(string name, string description)
    {
        var item = new SubMenu(name, description);
        AddElement(item);
        return item;
    }

    public IToggle AddToggle(string name, string description, bool toggled)
    {
        var item = new Toggle(name, description, toggled);
        AddElement(item);
        return item;
    }

    public IValueSlider AddValueSlider(string name, string description, float value, float min, float max)
    {
        var item = new ValueSlider(name, description, value, min, max);
        AddElement(item);
        return item;
    }

    public IHotkey AddHotkey(string name, string description, VirtualKey virtualKey, HotkeyType hotkeyType)
    {
        var item = new Hotkey(name, description, virtualKey, hotkeyType);
        AddElement(item);
        return item;
    }

    public IComboBox AddComboBox(string name, string description, int selectedIndex, string[] values)
    {
        var item = new ComboBox(name, description, selectedIndex, values);
        AddElement(item);
        return item;
    }
        
    public IEnumComboBox<T> AddEnumComboBox<T>(string name, string description, T selectedItem) where T : Enum
    {
        var item = new EnumComboBox<T>(name, description, selectedItem);
        AddElement(item);
        return item;
    }
    
    public override void ProcessKey(VirtualKey virtualKey, KeyState keyState)
    {
        foreach (var menuElement in _elements)
        {
            menuElement.ProcessKey(virtualKey, keyState);
        }
    }

    public override void LoadSettings(ISettingsProvider settingsProvider)
    {
        foreach (var element in _elements)
        {
            element.LoadSettings(settingsProvider);
        }
    }

    public override void SaveSettings(ISettingsProvider settingsProvider)
    {
        foreach (var element in _elements)
        {
            element.SaveSettings(settingsProvider);
        }
    }
}