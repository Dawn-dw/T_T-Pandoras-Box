namespace Api.Game.Offsets;

public interface IBaseOffsets
{
    public int GameTime { get; }
    public int LocalPlayer { get; }
    int HeroList { get; }
    int MinionList { get; }
    int MissileList { get; }
    int TurretList { get; }
    int InhibitorList { get; set; }
    int UnderMouseObject { get; }
}