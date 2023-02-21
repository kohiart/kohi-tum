// Copyright Kohi Art Community, Inc.. All rights reserved.

using Kohi.Composer;

namespace UniverseMachine;

public sealed class XorShift
{
    public int seed;

    public XorShift(int seed)
    {
        this.seed = seed;
    }

    public long nextFloat()
    {
        seed ^= seed << 13;
        seed ^= seed >> 17;
        seed ^= seed << 5;

        int t0;
        if (seed < 0)
        {
            t0 = ~seed + 1;
        }
        else
        {
            t0 = seed;
        }

        var dec = Fix64.Div((t0 % 1000) * Fix64.One, 1000 * Fix64.One);
        return dec;
    }

    public long nextFloatRange(long a, long b)
    {
        return Fix64.Add(a, Fix64.Mul(Fix64.Sub(b, a), nextFloat()));
    }

    public int nextInt(long a, long b)
    {
        var nextFloatRange = this.nextFloatRange(a, Fix64.Add(b, Fix64.One));
        var floor = Fix64.Floor(nextFloatRange);
        return (int) (floor / Fix64.One);
    }
}