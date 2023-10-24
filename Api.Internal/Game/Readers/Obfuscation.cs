using System.Runtime.InteropServices;

namespace Api.Internal.Game.Readers;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ObfuscatedLong
{
    private const int Size = sizeof(long);
    private bool isInit;
    private byte xorCount64;
    private byte xorCount8;
    private long xorKey;
    private byte valueIndex;
    private fixed long valueTable[4];

    public long Deobfuscate()
    {
        var value = valueTable[valueIndex];

        var xor64 = xorCount64;
        var xor8 = xorCount8;

        if (xor64 > Size)
        {
            return 0x0;
        }
        
        if (xor8 > Size)
        {
            return 0x0;
        }
        
        fixed (long* pXorKey = &xorKey)
        {
            var xorValuePtr64 = (ulong*)pXorKey;
            for (var i = 0; i < xor64; i++)
                ((ulong*)&value)[i] ^= ~xorValuePtr64[i];

            var xorValuePtr8 = (byte*)pXorKey;
            for (var i = Size - xor8; i < Size; i++)
                ((byte*)&value)[i] ^= (byte)~xorValuePtr8[i];
        }

        return value;
    }
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ObfuscatedBool
{
    private const int Size = sizeof(bool);
    private bool isInit;
    private byte xorCount64;
    private byte xorCount8;
    private bool xorKey;
    private byte valueIndex;
    private fixed bool valueTable[4];

    public bool Deobfuscate()
    {
        var value = valueTable[valueIndex];

        var xor64 = xorCount64;
        var xor8 = xorCount8;

        if (xor64 > Size)
        {
            return false;
        }
        
        if (xor8 > Size)
        {
            return false;
        }
        
        fixed (bool* pXorKey = &xorKey)
        {
            var xorValuePtr64 = (ulong*)pXorKey;
            for (var i = 0; i < xor64; i++)
                ((ulong*)&value)[i] ^= ~xorValuePtr64[i];

            var xorValuePtr8 = (byte*)pXorKey;
            for (var i = Size - xor8; i < Size; i++)
                ((byte*)&value)[i] ^= (byte)~xorValuePtr8[i];
        }

        return value;
    }
}