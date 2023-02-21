// Copyright Kohi Art Community, Inc.. All rights reserved.

using Kohi.Composer;

namespace UniverseMachine;

using int64 = System.Int64;
using int32 = System.Int32;

public sealed class Bezier
{
    public Vector2 A { get; set; }
    public Vector2 B { get; set; }
    public Vector2 C { get; set; }
    public Vector2 D { get; set; }
    public int32 Len { get; set; }
    public List<int64> ArcLengths { get; set; }
    
    public Bezier(Vector2 t, Vector2 h, Vector2 s, Vector2 i)
    {
        A = t;
        B = h;
        C = s;
        D = i;
        Len = 100;
        ArcLengths = new List<int64>(Len + 1);
        ArcLengths.Insert(0, 0);

        int64 n = xFunc(0);
        int64 r = yFunc(0);
        int64 e = 0;

        for (var ax = 1; ax <= Len; ax += 1)
        {
            int64 z = Fix64.Mul(42949672L /* 0.01 */, ax * Fix64.One);
            int64 c = xFunc(z);
            int64 u = yFunc(z);

            int64 y = Fix64.Sub(n, c);
            int64 o = Fix64.Sub(r, u);

            int64 t0 = Fix64.Mul(y, y);
            int64 t1 = Fix64.Mul(o, o);

            int64 sqrt = Fix64.Add(t0, t1);
            e = Fix64.Add(e, Trig256.Sqrt(sqrt));
            ArcLengths.Insert(ax, e);
            n = c;
            r = u;
        }
    }

    public int64 xFunc(int64 t)
    {
        int64 t0 = Fix64.Sub(Fix64.One, t);
        int64 t1 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(t0, A.X)));
        int64 t2 = Fix64.Mul(Fix64.Mul(Fix64.Mul(Fix64.Mul(t0, t0), 3 * Fix64.One), t), B.X);
        int64 t3 = Fix64.Mul(3 * Fix64.One, Fix64.Mul(t0, Fix64.Mul(Fix64.Mul(t, t), C.X)));
        int64 t4 = Fix64.Mul(t, Fix64.Mul(t, Fix64.Mul(t, D.X)));

        return Fix64.Add(Fix64.Add(t1, t2), Fix64.Add(t3, t4));
    }

    public int64 yFunc(int64 t)
    {
        int64 t0 = Fix64.Sub(Fix64.One, t);
        int64 t1 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(t0, A.Y)));
        int64 t2 = Fix64.Mul(t0, Fix64.Mul(t0, Fix64.Mul(3 * Fix64.One, Fix64.Mul(t, B.Y))));
        int64 t3 = Fix64.Mul(3 * Fix64.One, Fix64.Mul(t0, Fix64.Mul(Fix64.Mul(t, t), C.Y)));
        int64 t4 = Fix64.Mul(t, Fix64.Mul(t, Fix64.Mul(t, D.Y)));

        return Fix64.Add(Fix64.Add(t1, t2), Fix64.Add(t3, t4));
    }
    
    public int64 mx(int64 t) => xFunc(map(t));
    public int64 my(int64 t) => yFunc(map(t));

    private int64 map(int64 t)
    {
        int64 h = Fix64.Mul(t, ArcLengths[Len]);
        int32 n = 0;
        int32 s = 0;
        for (int32 i = Len; s < i;)
        {
            n = s + ((i - s) / 2 | 0);
            if (ArcLengths[n] < h)
            {
                s = n + 1;
            }
            else
            {
                i = n;
            }
        }
        if (ArcLengths[n] > h)
        {
            n--;
        }
        int64 r = ArcLengths[n];
        return r == h ? Fix64.Div(n * Fix64.One, Len * Fix64.One) :
            Fix64.Div(
                Fix64.Add(n * Fix64.One, Fix64.Div(Fix64.Sub(h, r), Fix64.Sub(ArcLengths[n + 1], r))),
                Len * Fix64.One);
    }
}