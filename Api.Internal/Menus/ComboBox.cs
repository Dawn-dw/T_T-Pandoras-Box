using Api.Menus;
using Api.Settings;
using ImGuiNET;

namespace Api.Internal.Menus;

internal class ComboBox : MenuElement, IComboBox
{
    public int SelectedIndex { get; set; }
    public string[] Items { get; set; }
    public event Action<int>? SelectionChanged;

    public ComboBox(string name, string description, int selectedIndex, string[] items) : base(name, description)
    {
        SelectedIndex = selectedIndex;
        Items = items;
    }

    protected override void RenderElement()
    {
        var selectedIndex = SelectedIndex;

        if (ImGui.Combo(ImGuiId, ref selectedIndex, Items, Items.Length))
        {
            if (selectedIndex != SelectedIndex)
            {
                SelectionChanged?.Invoke(selectedIndex);
            }
            SelectedIndex = selectedIndex;
        }
    }


    public override void LoadSettings(ISettingsProvider settingsProvider)
    {
        if (settingsProvider.ReadValue<int>($"{SaveId}.{nameof(SelectedIndex)}", out var selectedIndex))
        {
            SelectedIndex = selectedIndex;
        }
    }

    public override void SaveSettings(ISettingsProvider settingsProvider)
    {
        settingsProvider.SetValue($"{SaveId}.{nameof(SelectedIndex)}", SelectedIndex);
    }
}

internal class EnumComboBox<T> : MenuElement, IEnumComboBox<T> where T : Enum
{
    public T SelectedItem { get; set; }
    public event Action<T>? SelectionChanged;

    private int _selectedIndex;
    private readonly string[] _comboItems = Enum.GetNames(typeof(T));
    private readonly T[] _enumValues = (T[])Enum.GetValues(typeof(T));
    
    public EnumComboBox(string name, string description, T selectedItem) : base(name, description)
    {
        SelectedItem = selectedItem;
        _selectedIndex = Array.IndexOf(_enumValues, SelectedItem);
    }

    protected override void RenderElement()
    {
        var selectedIndex = _selectedIndex;

        if (ImGui.Combo(ImGuiId, ref selectedIndex, _comboItems, _comboItems.Length))
        {
            if (selectedIndex != _selectedIndex)
            {
                SelectionChanged?.Invoke(_enumValues[_selectedIndex]);
            }
            
            _selectedIndex = selectedIndex;
            SelectedItem = _enumValues[_selectedIndex];
        }
    }


    public override void LoadSettings(ISettingsProvider settingsProvider)
    {
        if (settingsProvider.ReadValue<T>($"{SaveId}.{nameof(SelectedItem)}", out var selectedItem))
        {
            SelectedItem = selectedItem;
        }
    }

    public override void SaveSettings(ISettingsProvider settingsProvider)
    {
        settingsProvider.SetValue($"{SaveId}.{nameof(SelectedItem)}", SelectedItem);
    }
}
