// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

public static class Textures
{
    public static Matrix Rectify(long x, long y, long r, long s, Matrix t)
    {
        var dx = x;
        var dy = y;

        if (!t.IsIdentity()) {
            t.Transform(ref dx, ref dy);
        }

        dx = Fix64.Add(dx, Fix64.One);
        dy = Fix64.Sub(dy, Fix64.One);

        var transform = Matrix.NewIdentity();
        transform = Matrix.Mul(transform, Matrix.NewTranslation(-4771708665856 /* -1111 */, -4771708665856 /* -1111 */));
        transform = Matrix.Mul(transform, Matrix.NewScale(s, s));

        if (r != 0) {
            transform = Matrix.Mul(transform, Matrix.NewRotation(r));
        }

        transform = Matrix.Mul(transform, Matrix.NewTranslation(dx, dy));
        return transform;
    }
}