// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

namespace UniverseMachine;

[Flags]
public enum Layer : byte
{
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