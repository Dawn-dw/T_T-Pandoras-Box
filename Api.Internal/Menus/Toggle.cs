using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Menus;
using Api.Settings;
using ImGuiNET;

namespace Api.Internal.Menus
{
    internal class Toggle : MenuElement, IToggle
    {
        private bool _toggled;
        public bool Toggled => _toggled;

        public Toggle(string name, string description, bool toggled) : base(name, description)
        {
            _toggled = toggled;
        }

        protected override void RenderElement()
        {
            ImGui.Checkbox(ImGuiId, ref _toggled);
        }


        public override void LoadSettings(ISettingsProvider settingsProvider)
        {
            if (settingsProvider.ReadValue<bool>($"{SaveId}.{nameof(Toggled)}", out var toggled))
            {
                _toggled = toggled;
            }
        }

        public override void SaveSettings(ISettingsProvider settingsProvider)
        {
            settingsProvider.SetValue($"{SaveId}.{nameof(Toggled)}", _toggled);
        }
    }
}
