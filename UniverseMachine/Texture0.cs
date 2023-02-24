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
        
        var stroke1 = new Stroke(circle1.Vertices(), 0)
        {
            LineCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        var circle2 = new Ellipse(2860448219136, 4776003633152, 95348277248, 95348277248);
        var stroke2 = new Stroke(circle2.Vertices(), 1073741824 /* 0.25 */);

        var data = new Data
        {
            Circle1 = circle1.Vertices().ToList(),
            Stroke1 = Stroke.Vertices(stroke1).ToList(),
            Circle2 = circle2.Vertices().ToList(),
            Stroke2 = Stroke.Vertices(stroke2).ToList(),
            CircleColor1 = 4294967295,
            StrokeColor1 = 16777215,
            CircleColor2 = 4278190080,
            StrokeColor2 = 1056964608
        };

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, uint color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.CircleColor1, color);
        var tinted2 = ColorMath.Tint(data.StrokeColor1, color);
        var tinted3 = ColorMath.Tint(data.CircleColor2, color);
        var tinted4 = ColorMath.Tint(data.StrokeColor2, color);

        Graphics2D.RenderWithTransform(g, data.Circle1, tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.Stroke1, tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle2, tinted3, matrix);
        Graphics2D.RenderWithTransform(g, data.Stroke2, tinted4, matrix);
    }

    public class Data
    {
        public IList<VertexData> Circle1 = null!;
        public IList<VertexData> Circle2 = null!;
        public uint CircleColor1;
        public uint CircleColor2;
        public IList<VertexData> Stroke1 = null!;
        public IList<VertexData> Stroke2 = null!;
        public uint StrokeColor1;
        public uint StrokeColor2;
    }
}