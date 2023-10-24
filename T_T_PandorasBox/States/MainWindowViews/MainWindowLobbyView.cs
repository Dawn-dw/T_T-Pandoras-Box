using System.Diagnostics;
using System.Numerics;
using ImGuiNET;
using LCU;

namespace T_T_PandorasBox.States.MainWindowViews;

public class MainWindowLobbyView : IMainWindowView
{
    public string Name => "Lobby";
    private LcuClient? _lcuClient;
    private string _region = string.Empty;

    private bool sendInChat;
    private float _timer = 0;
    private List<LcuPlayer> _lobbyPlayers = new List<LcuPlayer>();
    
    public void Render(float deltaTime)
    {
        if (!LcuClient.IsSupported())
        {
            ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f),"Platform not supported.");
            return;
        }
        
        _lcuClient ??= LcuClient.Create();

        if (_lcuClient is null)
        {
            ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f),"Failed to get LCU");
            return;
        }

        if (ImGui.Button("Get players"))
        {
            _lobbyPlayers.Clear();
            _region = _lcuClient.GetRegion();
            var lobbyPlayers = _lcuClient.GetLobbyPlayers();
            foreach (var player in lobbyPlayers.Participants)
            {
                _lobbyPlayers.Add(player);
            }
        }

        if (!string.IsNullOrWhiteSpace(_region))
        {
            ImGui.Text($"Region: {_region}");
        }

        for (var i = 0; i < _lobbyPlayers.Count; i++)
        {
            ImGui.Text($"Player{i+1}");
            ImGui.SameLine();
            var name = _lobbyPlayers[i].Name;
            ImGui.SetNextItemWidth(200);
            ImGui.InputText($"##Player" + i, ref name, (uint)name.Length);
            ImGui.Spacing();
        }

        if (_lobbyPlayers.Any())
        {
            ImGui.Checkbox("Send chat", ref sendInChat);
            if (ImGui.Button("op.gg"))
            {
                OpenUrl($"https://www.op.gg/multisearch/{_region.ToLower()}?summoners={GetSearchNames()}");
            }
            ImGui.SameLine();
            if (ImGui.Button("u.gg"))
            {
                OpenUrl($"https://u.gg/multisearch?summoners={GetSearchNames()}&region={MapRegion(_region)}");
            }
            ImGui.SameLine();
            if (ImGui.Button("poro.gg"))
            {
                OpenUrl($"https://poro.gg/multi?region={_region}&q={GetSearchNames()}");
            }
        }
    }

    private string MapRegion(string region)
    {
        region = region.ToLower();
        return region switch
        {
            "KR" => "kr",
            "RU" => "ru",
            _ => region + "1"
        };
    }

    private void OpenUrl(string url)
    {
        if (!_lobbyPlayers.Any())
        {
            return;
        }
        if (sendInChat)
        {
            _lcuClient?.SendChat(_lobbyPlayers.First().Cid, url);
        }
        
        Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
    }

    private string GetSearchNames()
    {
        return string.Join(", ", _lobbyPlayers.Select(x => x.Name));
    }
}