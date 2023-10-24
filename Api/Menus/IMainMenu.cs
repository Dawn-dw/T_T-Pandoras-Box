using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Scripts;

namespace Api.Menus
{
    public interface IMainMenu
    {
        public event Action? MenuOpen;
        public event Action? MenuClose;
        void Render();
        void AddMenu(IMenu menu);
        IMenu CreateMenu(string name, ScriptType scriptType);
        void LoadSettings();
        void SaveSettings();
        void RemoveMenu(IMenu menu);
    }
}
