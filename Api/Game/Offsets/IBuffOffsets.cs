namespace Api.Game.Offsets;


public interface IBuffOffsets
{
    public OffsetData BuffEntryBuffStartTime { get; }
    public OffsetData BuffEntryBuffEndTime { get; }
    public OffsetData BuffEntryBuffCount { get; }
    public OffsetData BuffEntryBuffCountAlt { get; }
    public OffsetData BuffInfo { get; }
    public OffsetData BuffInfoName { get; }
    IEnumerable<OffsetData> GetOffsets();
}