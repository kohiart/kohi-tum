// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Drawing;
using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// A white circle and a smaller black circle. Used in universe rendering.
/// </summary>
public static class Texture0
{
    private static readonly Data Instance;

    static Texture0()
    {
        Instance = CreateTexture();
    }

    public static Data CreateTexture()
    {
        var circle1 = new Ellipse(2860448219136, 6682969112576, 1906965479424, 1906965479424);
        var stroke1 = new Stroke(circle1.Vertices(), 0);
        stroke1.LineCap = LineCap.Round;
        stroke1.LineJoin = LineJoin.Round;

        var circle2 = new Ellipse(2860448219136, 4776003633152, 95348277248, 95348277248);
        var stroke2 = new Stroke(circle2.Vertices(), 1073741824 /* 0.25 */);

        var circleColor1 = Color.White;
        var strokeColor1 = Color.FromArgb(0, Color.White.R, Color.White.G, Color.White.B);
        var circleColor2 = Color.Black;
        var strokeColor2 = Color.FromArgb(63, Color.Black.R, Color.Black.G, Color.Black.B);

        var data = new Data
        {
            Circle1 = circle1,
            Stroke1 = stroke1,
            Circle2 = circle2,
            Stroke2 = stroke2,
            CircleColor1 = circleColor1,
            StrokeColor1 = strokeColor1,
            CircleColor2 = circleColor2,
            StrokeColor2 = strokeColor2
        };

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, Color color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.CircleColor1.ToUInt32(), color.ToUInt32());
        var tinted2 = ColorMath.Tint(data.StrokeColor1.ToUInt32(), color.ToUInt32());
        var tinted3 = ColorMath.Tint(data.CircleColor2.ToUInt32(), color.ToUInt32());
        var tinted4 = ColorMath.Tint(data.StrokeColor2.ToUInt32(), color.ToUInt32());

        Graphics2D.RenderWithTransform(g, data.Circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.Stroke1).ToList(), tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle2.Vertices().ToList(), tinted3, matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.Stroke2).ToList(), tinted4, matrix);
    }

    public class Data
    {
        public Ellipse Circle1 = null!;
        public Ellipse Circle2 = null!;
        public Color CircleColor1;
        public Color CircleColor2;
        public Stroke Stroke1 = null!;
        public Stroke Stroke2 = null!;
        public Color StrokeColor1;
        public Color StrokeColor2;
    }
}