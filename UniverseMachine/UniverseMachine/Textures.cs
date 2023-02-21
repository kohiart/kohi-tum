// Copyright Kohi Art Community, Inc.. All rights reserved.

using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine;

public static class Textures
{
    public static Matrix Rectify(int64 x, int64 y, int64 r, int64 s, Matrix t)
    {
        int64 dx = x;
        int64 dy = y;

        if (!t.IsIdentity())
            t.Transform(ref dx, ref dy);

        dx = Fix64.Add(dx, Fix64.One);
        dy = Fix64.Sub(dy, Fix64.One);

        var transform = Matrix.NewIdentity();
        transform *= Matrix.NewTranslation(-4771708665856 /* -1111 */, -4771708665856 /* -1111 */);
        transform *= Matrix.NewScale(s, s);

        if (r != 0)
        {
            transform *= Matrix.NewRotation(r);
        }

        transform *= Matrix.NewTranslation(dx, dy);
        return transform;
    }
}