using Api.Game.Offsets;
using Microsoft.Extensions.Configuration;

namespace Api.Internal.Game.Offsets;

internal class BuffOffsets : IBuffOffsets
{
    public OffsetData BuffEntryBuffStartTime { get; }
    public OffsetData BuffEntryBuffEndTime { get; }
    public OffsetData BuffEntryBuffCount { get; }
    public OffsetData BuffEntryBuffCountAlt { get; }
    public OffsetData BuffInfo { get; }
    public OffsetData BuffInfoName { get; }

    public BuffOffsets(IConfiguration configuration)
    {
        var cs = configuration.GetSection(nameof(BuffOffsets));
        BuffEntryBuffStartTime = new OffsetData(nameof(BuffEntryBuffStartTime), Convert.ToInt32(cs[nameof(BuffEntryBuffStartTime)], 16), typeof(float));
        BuffEntryBuffEndTime = new OffsetData(nameof(BuffEntryBuffEndTime), Convert.ToInt32(cs[nameof(BuffEntryBuffEndTime)], 16), typeof(float));
        BuffEntryBuffCount = new OffsetData(nameof(BuffEntryBuffCount), Convert.ToInt32(cs[nameof(BuffEntryBuffCount)], 16), typeof(int));
        BuffEntryBuffCountAlt = new OffsetData(nameof(BuffEntryBuffCountAlt), Convert.ToInt32(cs[nameof(BuffEntryBuffCountAlt)], 16), typeof(int));
        BuffInfo = new OffsetData(nameof(BuffInfo), Convert.ToInt32(cs[nameof(BuffInfo)], 16), typeof(IntPtr));
        //TYPE IS WRONG BUT WE READ IT IN DIFFRENT WAY
        BuffInfoName = new OffsetData(nameof(BuffInfoName), Convert.ToInt32(cs[nameof(BuffInfoName)], 16), typeof(IntPtr));
    }
    
    public IEnumerable<OffsetData> GetOffsets()
    {
        yield return BuffEntryBuffStartTime;
        yield return BuffEntryBuffEndTime;
        yield return BuffEntryBuffCount;
        yield return BuffEntryBuffCountAlt;
        yield return BuffInfo;
    }
}