using Api.Menus;
using Api.Settings;
using ImGuiNET;

namespace Api.Internal.Menus;

internal class ValueSlider : MenuElement, IValueSlider
{
    private readonly float _min;
    private readonly float _max;
    private float _value;
    public float Value
    {
        get => _value;
        set => _value = value;
    }

    public ValueSlider(
        string name,
        string description,
        float value,
        float min,
        float max) : base(name, description)
    {
        _value = value;
        _min = min;
        _max = max;
    }

    protected override void RenderElement()
    {
        ImGui.SliderFloat(ImGuiId, ref _value, _min, _max);
    }

    public override void LoadSettings(ISettingsProvider settingsProvider)
    {
        if (settingsProvider.ReadValue<float>($"{SaveId}.{nameof(Value)}", out var value))
        {
            _value = value;
        }
    }

    public override void SaveSettings(ISettingsProvider settingsProvider)
    {
        settingsProvider.SetValue($"{SaveId}.{nameof(Value)}", _value);
    }
}