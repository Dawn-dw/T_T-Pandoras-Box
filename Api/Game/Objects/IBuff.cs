namespace Api.Game.Objects;


public interface IBuff
{
    public IntPtr Pointer { get; set; }
    public string Name { get; set; }
    public float StartTime { get; set; }
    public float EndTime { get; set; }
    public int Count { get; set; }
    public int CountAlt { get; set; }

    void CloneFrom(IBuff buff);
}