using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Inputs;
using Api.Scripts;
using Api.Settings;

namespace Api.Menus
{
    public interface IMenu
    {
        string Name { get; }
        ScriptType ScriptType { get; }
        void AddElement(IMenuElement menuElement);
        void Render();
        void ProcessKey(VirtualKey virtualKey, KeyState keyState);
        void LoadSettings(ISettingsProvider settingsProvider);
        void SaveSettings(ISettingsProvider settingsProvider);

        ISubMenu AddSubMenu(string name, string description);
        IToggle AddToggle(string name, string description, bool toggled);
        IValueSlider AddValueSlider(string name, string description, float value, float min, float max);
        IHotkey AddHotkey(string name, string description, VirtualKey virtualKey, HotkeyType hotkeyType);
        IComboBox AddComboBox(string name, string description, int selectedIndex, string[] values);
        IEnumComboBox<T> AddEnumComboBox<T>(string name, string description, T selectedItem) where T : Enum;
    }
}
