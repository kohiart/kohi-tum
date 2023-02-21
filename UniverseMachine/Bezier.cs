// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

public sealed class Bezier
{
    public Bezier(Vector2 t, Vector2 h, Vector2 s, Vector2 i)
    {
        A = t;
        B = h;
        C = s;
        D = i;
        Len = 100;
        ArcLengths = new List<long>(Len + 1);
        ArcLengths.Insert(0, 0);

        var n = XFunc(0);
        var r = YFunc(0);
        long e = 0;

        for (var ax = 1; ax <= Len; ax += 1)
        {
            var z = Fix64.Mul(42949672L /* 0.01 */, ax * Fix64.One);
            var c = XFunc(z);
            var u = YFunc(z);

            var y = Fix64.Sub(n, c);
            var o = Fix64.Sub(r, u);

            var t0 = Fix64.Mul(y, y);
            var t1 = Fix64.Mul(o, o);

            var sqrt = Fix64.Add(t0, t1);
            e = Fix64.Add(e, Trig256.Sqrt(sqrt));
            ArcLengths.Insert(ax, e);
            n = c;
            r = u;
        }
    }

    public Vector2 A { get; set; }
    public Vector2 B { get; set; }
    public Vector2 C { get; set; }
    public Vector2 D { get; set; }
    public int Len { get; set; }
    public List<long> ArcLengths { get; set; }

    public long XFunc(long t)
    {
        var t0 = Fix64.Sub(Fix64.One, t);
        var t1 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(t0, A.X)));
        var t2 = Fix64.Mul(Fix64.Mul(Fix64.Mul(Fix64.Mul(t0, t0), 3 * Fix64.One), t), B.X);
        var t3 = Fix64.Mul(3 * Fix64.One, Fix64.Mul(t0, Fix64.Mul(Fix64.Mul(t, t), C.X)));
        var t4 = Fix64.Mul(t, Fix64.Mul(t, Fix64.Mul(t, D.X)));

        return Fix64.Add(Fix64.Add(t1, t2), Fix64.Add(t3, t4));
    }

    public long YFunc(long t)
    {
        var t0 = Fix64.Sub(Fix64.One, t);
        var t1 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(t0, A.Y)));
        var t2 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(3 * Fix64.One, Fix64.Mul(t, B.Y))));
        var t3 = Fix64.Mul(3 * Fix64.One, Fix64.Mul(t0, Fix64.Mul(Fix64.Mul(t, t), C.Y)));
        var t4 = Fix64.Mul(t, Fix64.Mul(t, Fix64.Mul(t, D.Y)));

        return Fix64.Add(Fix64.Add(t1, t2), Fix64.Add(t3, t4));
    }

    public long Mx(long t)
    {
        return XFunc(Map(t));
    }

    public long My(long t)
    {
        return YFunc(Map(t));
    }

    private long Map(long t)
    {
        var h = Fix64.Mul(t, ArcLengths[Len]);
        var n = 0;
        var s = 0;
        for (var i = Len; s < i;)
        {
            n = s + (((i - s) / 2) | 0);
            if (ArcLengths[n] < h)
                s = n + 1;
            else
                i = n;
        }

        if (ArcLengths[n] > h) n--;
        var r = ArcLengths[n];
        return r == h
            ? Fix64.Div(n * Fix64.One, Len * Fix64.One)
            : Fix64.Div(
                Fix64.Add(n * Fix64.One, Fix64.Div(Fix64.Sub(h, r), Fix64.Sub(ArcLengths[n + 1], r))),
                Len * Fix64.One);
    }
}