// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Drawing;
using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// Mysterious contraption. Used in universe rendering.
/// </summary>
public static class Texture4
{
    private static readonly Data Instance;

    static Texture4()
    {
        Instance = CreateTexture();
    }

    public static Data CreateTexture()
    {
        var data = new Data();

        var poly1 = new CustomPath();
        poly1.MoveTo(7620234051584, 4179123765248);
        poly1.LineTo(5570246475776, 3122664046592);
        poly1.LineTo(7915813994496, 3204664262656);
        var polyStroke1 = new Stroke(poly1.Vertices(), 2147483648 /* 0.5 */);
        polyStroke1.LineCap = LineCap.Round;
        polyStroke1.LineJoin = LineJoin.Round;

        data.Poly1 = poly1;
        data.PolyStroke1 = polyStroke1;

        var poly2 = new CustomPath();
        poly2.MoveTo(8940808044544, 4338354749440);
        poly2.LineTo(7620234051584, 4179123765248);
        poly2.LineTo(7915813994496, 3204664262656);
        poly2.LineTo(8940808044544, 4338354749440);
        var polyStroke2 = new Stroke(poly2.Vertices(), 2147483648 /* 0.5 */);
        polyStroke2.LineCap = LineCap.Round;
        polyStroke2.LineJoin = LineJoin.Round;

        data.Poly2 = poly2;
        data.PolyStroke2 = polyStroke2;

        data.Lines = new List<Stroke>();

        var line1 = new CustomPath();
        line1.MoveTo(0, 3816800387072);
        line1.LineTo(5570246475776, 3122664046592);
        var stroke1 = new Stroke(line1.Vertices(), 1073741824 /* 0.25 */);
        stroke1.LineCap = LineCap.Round;
        stroke1.LineJoin = LineJoin.Round;

        var line2 = new CustomPath();
        line2.MoveTo(9534827397120, 4708305993728);
        line2.LineTo(8940808044544, 4338354749440);

        var stroke2 = new Stroke(line2.Vertices(), 1073741824 /* 0.25 */);
        stroke2.LineCap = LineCap.Round;
        stroke2.LineJoin = LineJoin.Round;

        data.Lines.Add(stroke1);
        data.Lines.Add(stroke2);

        data.Circles = new List<Ellipse>
        {
            new(4335486107648, 5130699145216, 22883586048, 22883586048),
            new(4801739358208, 5074443567104, 22883586048, 22883586048),
            new(5267992346624, 5019141668864, 22883586048, 22883586048),
            new(5734245335040, 4962886090752, 22883586048, 22883586048),
            new(6200498323456, 4906631036928, 22883586048, 22883586048),
            new(6666751311872, 4851328614400, 22883586048, 22883586048),
            new(7133004300288, 4795073036288, 22883586048, 22883586048),
            new(7599257288704, 4739771138048, 22883586048, 22883586048),
            new(8065510801408, 4683515559936, 22883586048, 22883586048),
            new(8531763789824, 4628213661696, 22883586048, 22883586048),
            new(4569089703936, 4348843655168, 31464931328, 31464931328),
            new(5966894989312, 4181030076416, 31464931328, 31464931328),
            new(4335486107648, 3678545117184, 39092793344, 39092793344),
            new(4102836191232, 2952944680960, 46720655360, 39092793344)
        };

        var poly1Color = Color.FromArgb((byte) (255 * 0.75f), Color.White.R, Color.White.G, Color.White.B);
        var strokeColor = Color.FromArgb(255, Color.Black.R, Color.Black.G, Color.Black.B);
        var poly2Color = Color.FromArgb((byte) (255 * 0.50f), Color.White.R, Color.White.G, Color.White.B);
        
        data.Poly1Color = poly1Color;
        data.PolyStroke1Color = strokeColor;
        data.Poly2Color = poly2Color;
        data.PolyStroke2Color = strokeColor;

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, Color color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        Graphics2D.RenderWithTransform(g, data.Poly1.Vertices().ToList(),
            ColorMath.Tint(data.Poly1Color.ToUInt32(), color.ToUInt32()), matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.PolyStroke1).ToList(),
            ColorMath.Tint(data.PolyStroke1Color.ToUInt32(), color.ToUInt32()), matrix);
        Graphics2D.RenderWithTransform(g, data.Poly2.Vertices().ToList(),
            ColorMath.Tint(data.Poly2Color.ToUInt32(), color.ToUInt32()), matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.PolyStroke2).ToList(),
            ColorMath.Tint(data.PolyStroke2Color.ToUInt32(), color.ToUInt32()), matrix);

        foreach (var line in data.Lines)
        {
            var tinted = ColorMath.Tint(
                Color.FromArgb((byte) (255 * 0.5f), Color.Black.R, Color.Black.G, Color.Black.B).ToUInt32(),
                color.ToUInt32()
            );

            Graphics2D.RenderWithTransform(g, Stroke.Vertices(line).ToList(), tinted, matrix);
        }

        foreach (var circle in data.Circles)
        {
            var tinted = ColorMath.Tint(Color.White.ToUInt32(), color.ToUInt32());
            Graphics2D.RenderWithTransform(g, circle.Vertices().ToList(), tinted, matrix);
        }
    }

    public class Data
    {
        public List<Ellipse> Circles;
        public List<Stroke> Lines;

        public CustomPath Poly1;
        public Color Poly1Color;

        public CustomPath Poly2;
        public Color Poly2Color;
        public Stroke PolyStroke1;
        public Color PolyStroke1Color;
        public Stroke PolyStroke2;
        public Color PolyStroke2Color;
    }
}