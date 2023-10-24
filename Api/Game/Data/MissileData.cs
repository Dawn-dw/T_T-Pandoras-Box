namespace Api.Game.Data;

public class MissileData
{
    public string Name { get; set; } = string.Empty;
    public float Speed { get; set; }
    public float Height { get; set; }
    public float TravelTime { get; set; }
    public int NameHash { get; set; }
}