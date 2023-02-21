// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

public sealed class XorShift
{
    public int Seed;

    public XorShift(int seed)
    {
        Seed = seed;
    }

    public long NextFloat()
    {
        Seed ^= Seed << 13;
        Seed ^= Seed >> 17;
        Seed ^= Seed << 5;

        int t0;
        if (Seed < 0)
            t0 = ~Seed + 1;
        else
            t0 = Seed;

        var dec = Fix64.Div(t0 % 1000 * Fix64.One, 1000 * Fix64.One);
        return dec;
    }

    public long NextFloatRange(long a, long b)
    {
        return Fix64.Add(a, Fix64.Mul(Fix64.Sub(b, a), NextFloat()));
    }

    public int NextInt(long a, long b)
    {
        var nextFloatRange = NextFloatRange(a, Fix64.Add(b, Fix64.One));
        var floor = Fix64.Floor(nextFloatRange);
        return (int) (floor / Fix64.One);
    }
}