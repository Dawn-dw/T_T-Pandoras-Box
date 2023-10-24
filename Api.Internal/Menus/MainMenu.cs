using Api.Inputs;
using Api.Internal.Inputs;
using Api.Menus;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Api.Scripts;
using Api.Settings;
using WinApi;

namespace Api.Internal.Menus
{
    internal class MainMenu : IMainMenu
    {
        private readonly ILogger<MainMenu> _logger;
        private readonly IInputManager _inputManager;
        private readonly List<IMenu> _menuItems = new();
        private bool _isOpen;
        public bool IsOpen => _isOpen;

        public event Action? MenuOpen;
        public event Action? MenuClose;
        private readonly ISettingsProvider _settingsProvider;

        public MainMenu(ILogger<MainMenu> logger, IInputManager inputManager, ISettingsProvider settingsProvider)
        {
            _logger = logger;
            _inputManager = inputManager;
            _settingsProvider = settingsProvider;
            _inputManager.KeyDown += InputManagerKeyDown;
            _inputManager.KeyUp += InputManagerOnKeyUp;
          
            _settingsProvider = settingsProvider;
            LoadSettings();
        }

        private void InputManagerOnKeyUp(VirtualKey virtualKey)
        {
            foreach (var menuItem in _menuItems)
            {
                menuItem.ProcessKey(virtualKey, KeyState.KeyUp);
            }
        }

        private void InputManagerKeyDown(VirtualKey virtualKey)
        {
            if (virtualKey == VirtualKey.LeftShift)
            {
                _isOpen = !_isOpen;
                _logger.LogInformation("Menu state changed: {_isOpen}", _isOpen);

                if(_isOpen)
                {
                    MenuOpen?.Invoke();
                }
                else
                {
                    OnClose();
                }
                return;
            }
            
            foreach (var menuItem in _menuItems)
            {
                menuItem.ProcessKey(virtualKey, KeyState.KeyDown);
            }
        }

        private void OnClose()
        {
            SaveSettings();
            MenuClose?.Invoke();
        }

        public void Render()
        {
            if(!_isOpen)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(800, 600), ImGuiCond.FirstUseEver);

            if (!ImGui.Begin("T_T_PandorasBox", ImGuiWindowFlags.NoCollapse))
            {
                ImGui.End();
                return;
            }

            if (ImGui.BeginTabBar("MainMenuTabs", ImGuiTabBarFlags.None))
            {
                foreach (var menu in _menuItems)
                {
                    if (ImGui.BeginTabItem(menu.Name))
                    {
                        menu.Render();
                        ImGui.EndTabItem();
                    }
                }

                ImGui.EndTabBar();
            }


            ImGui.End();
        }

        public void AddMenu(IMenu menu)
        {
            _menuItems.Add(menu);
        }
        
        public IMenu CreateMenu(string name, ScriptType scriptType)
        {
            var menu = new Menu(name, scriptType);
            AddMenu(menu);
            return menu;
        }

        public void LoadSettings()
        {
            _settingsProvider.Load();
            foreach (var menuItem in _menuItems)
            {
                menuItem.LoadSettings(_settingsProvider);
            }
        }

        public void SaveSettings()
        {
            foreach (var menuItem in _menuItems)
            {
                menuItem.SaveSettings(_settingsProvider);
            }
            
            _settingsProvider.Save();
        }

        public void RemoveMenu(IMenu menu)
        {
            _menuItems.Remove(menu);
        }
    }
}
