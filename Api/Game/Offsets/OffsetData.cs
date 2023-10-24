using System.Runtime.InteropServices;

namespace Api.Game.Offsets;

public struct OffsetData
{
    public string Name { get; }
    public int Offset { get; }
    public int TargetSize { get; }

    public OffsetData(string name, int offset, Type type)
    {
        Name = name;
        Offset = offset;
        TargetSize = Marshal.SizeOf(type);
    }
}