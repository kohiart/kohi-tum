// Copyright Kohi Art Community, Inc.. All rights reserved.

namespace UniverseMachine;

[Flags]
public enum Layer : byte
{
    None = 0x0,
    Background = 1 << 0,
    Grid = 1 << 1,
    Skeleton = 1 << 2,
    Universe = 1 << 3,
    Stars = 1 << 4,
    Mats = 1 << 5,
    All = Background | Grid | Skeleton | Universe | Stars | Mats
}

public static class LayerExtensions
{
    public static bool HasFlagFast(this Layer value, Layer flag)
    {
        return (value & flag) != 0;
    }
}