using Api.Game.Offsets;
using Microsoft.Extensions.Configuration;

namespace Api.Internal.Game.Offsets;

internal class BaseOffsets : IBaseOffsets
{
    public int GameTime { get; }
    public int LocalPlayer { get; }
    public int HeroList { get; }
    public int MinionList { get; }
    public int MissileList { get; }
    public int TurretList { get; }
    public int InhibitorList { get; set; }
    public int UnderMouseObject { get; }

    public BaseOffsets(IConfiguration configuration)
    {
        var cs = configuration.GetSection(nameof(BaseOffsets));
        GameTime = Convert.ToInt32(cs[nameof(GameTime)], 16);
        LocalPlayer = Convert.ToInt32(cs[nameof(LocalPlayer)], 16);
        MinionList = Convert.ToInt32(cs[nameof(MinionList)], 16);
        HeroList = Convert.ToInt32(cs[nameof(HeroList)], 16);
        MissileList = Convert.ToInt32(cs[nameof(MissileList)], 16);
        TurretList = Convert.ToInt32(cs[nameof(TurretList)], 16);
        InhibitorList = Convert.ToInt32(cs[nameof(InhibitorList)], 16);
    }
}