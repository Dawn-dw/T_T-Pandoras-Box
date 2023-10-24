using System.Numerics;
using Api.Game.Offsets;
using Microsoft.Extensions.Configuration;

namespace Api.Internal.Game.Offsets;

internal class AiManagerOffsets : IAiManagerOffsets
{
    public OffsetData TargetPosition { get; }
    public OffsetData PathStart { get; }
    public OffsetData PathEnd { get; }
    public OffsetData CurrentPathSegment { get; }
    public OffsetData PathSegments { get; }
    public OffsetData PathSegmentsCount { get; }
    public OffsetData CurrentPosition { get; }
    public OffsetData IsDashing { get; }
    public OffsetData DashSpeed { get; }
    public OffsetData IsMoving { get; }
    public OffsetData MovementSpeed { get; }

    public AiManagerOffsets(IConfiguration configuration)
    {
        var cs = configuration.GetSection(nameof(AiManagerOffsets));
	    
        TargetPosition = new OffsetData(nameof(TargetPosition), Convert.ToInt32(cs[nameof(TargetPosition)], 16), typeof(Vector3));
        PathStart = new OffsetData(nameof(PathStart), Convert.ToInt32(cs[nameof(PathStart)], 16), typeof(Vector3));
        PathEnd = new OffsetData(nameof(PathEnd), Convert.ToInt32(cs[nameof(PathEnd)], 16), typeof(Vector3));
        CurrentPathSegment = new OffsetData(nameof(CurrentPathSegment), Convert.ToInt32(cs[nameof(CurrentPathSegment)], 16), typeof(int));
        PathSegments = new OffsetData(nameof(PathSegments), Convert.ToInt32(cs[nameof(PathSegments)], 16), typeof(IntPtr));
        PathSegmentsCount = new OffsetData(nameof(PathSegmentsCount), Convert.ToInt32(cs[nameof(PathSegmentsCount)], 16), typeof(int));
        CurrentPosition = new OffsetData(nameof(CurrentPosition), Convert.ToInt32(cs[nameof(CurrentPosition)], 16), typeof(Vector3));
        IsDashing = new OffsetData(nameof(IsDashing), Convert.ToInt32(cs[nameof(IsDashing)], 16), typeof(bool));
        DashSpeed = new OffsetData(nameof(DashSpeed), Convert.ToInt32(cs[nameof(DashSpeed)], 16), typeof(float));
        IsMoving = new OffsetData(nameof(IsMoving), Convert.ToInt32(cs[nameof(IsMoving)], 16), typeof(bool));
        MovementSpeed = new OffsetData(nameof(MovementSpeed), Convert.ToInt32(cs[nameof(MovementSpeed)], 16), typeof(float));
    }
    
    public IEnumerable<OffsetData> GetOffsets()
    {
        yield return TargetPosition;
        yield return PathStart;
        yield return PathEnd;
        yield return CurrentPathSegment;
        yield return PathSegments;
        yield return PathSegmentsCount;
        yield return CurrentPosition;
        yield return IsDashing;
        yield return DashSpeed;
        yield return IsMoving;
        yield return MovementSpeed;
    }
}