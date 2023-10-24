using Api.Menus;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Api.Inputs;
using Api.Settings;

namespace Api.Internal.Menus
{
    internal abstract class MenuElement : IMenuElement
    {
        public string Id { get; set; } = string.Empty;
        public string Name {get; private set;}
        public string Description { get; private set; }
        public string ImGuiId => $"{Name}##{Id}";
        public string SaveId => $"{Id}.{Name}";

        public MenuElement(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void Render()
        {
            ImGui.BeginGroup();
            RenderElement();
            RenderTooltip();
            ImGui.EndGroup();
        }

        protected abstract void RenderElement();
    
        private void RenderTooltip()
        {
            if (!string.IsNullOrWhiteSpace(Description))
            {
                ImGui.SameLine();
                ImGui.TextDisabled("?");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.TextUnformatted(Description);
                    ImGui.EndTooltip();
                }
            }
        }
        
        public virtual void ProcessKey(VirtualKey virtualKey, KeyState keyState)
        {
            
        }
        
        public abstract void LoadSettings(ISettingsProvider settingsProvider);
        public abstract void SaveSettings(ISettingsProvider settingsProvider);
    }
}
