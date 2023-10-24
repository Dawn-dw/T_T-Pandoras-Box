using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Inputs;
using Api.Settings;

namespace Api.Menus
{
    public interface IMenuElement
    {
        string Id { get; set; }
        string ImGuiId { get; }
        string SaveId { get; }
        string Name { get; }
        string Description { get; }
        void Render();
        void ProcessKey(VirtualKey virtualKey, KeyState keyState);
        void LoadSettings(ISettingsProvider settingsProvider);
        void SaveSettings(ISettingsProvider settingsProvider);
    }
}
