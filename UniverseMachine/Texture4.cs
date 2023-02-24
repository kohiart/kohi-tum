// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

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

        var polyStroke1 = new Stroke(poly1.Vertices(), 2147483648 /* 0.5 */)
        {
            LineCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        data.Poly1 = poly1.Vertices().ToList();
        data.PolyStroke1 = Stroke.Vertices(polyStroke1).ToList();

        var poly2 = new CustomPath();
        poly2.MoveTo(8940808044544, 4338354749440);
        poly2.LineTo(7620234051584, 4179123765248);
        poly2.LineTo(7915813994496, 3204664262656);
        poly2.LineTo(8940808044544, 4338354749440);

        var polyStroke2 = new Stroke(poly2.Vertices(), 2147483648 /* 0.5 */)
        {
            LineCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };

        data.Poly2 = poly2.Vertices().ToList();
        data.PolyStroke2 = Stroke.Vertices(polyStroke2).ToList();

        data.Lines = new List<IList<VertexData>>();

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

        data.Lines.Add(Stroke.Vertices(stroke1).ToList());
        data.Lines.Add(Stroke.Vertices(stroke2).ToList());

        data.Circles = new List<IList<VertexData>>
        {
            new Ellipse(4335486107648, 5130699145216, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(4801739358208, 5074443567104, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(5267992346624, 5019141668864, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(5734245335040, 4962886090752, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(6200498323456, 4906631036928, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(6666751311872, 4851328614400, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(7133004300288, 4795073036288, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(7599257288704, 4739771138048, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(8065510801408, 4683515559936, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(8531763789824, 4628213661696, 22883586048, 22883586048).Vertices().ToList(),
            new Ellipse(4569089703936, 4348843655168, 31464931328, 31464931328).Vertices().ToList(),
            new Ellipse(5966894989312, 4181030076416, 31464931328, 31464931328).Vertices().ToList(),
            new Ellipse(4335486107648, 3678545117184, 39092793344, 39092793344).Vertices().ToList(),
            new Ellipse(4102836191232, 2952944680960, 46720655360, 39092793344).Vertices().ToList()
        };

        data.Poly1Color = 3221225471;
        data.PolyStroke1Color = 4278190080;
        data.Poly2Color = 2147483647;
        data.PolyStroke2Color = 4278190080;

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, uint color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        Graphics2D.RenderWithTransform(g, data.Poly1, ColorMath.Tint(data.Poly1Color, color), matrix);
        Graphics2D.RenderWithTransform(g, data.PolyStroke1, ColorMath.Tint(data.PolyStroke1Color, color), matrix);
        Graphics2D.RenderWithTransform(g, data.Poly2, ColorMath.Tint(data.Poly2Color, color), matrix);
        Graphics2D.RenderWithTransform(g, data.PolyStroke2, ColorMath.Tint(data.PolyStroke2Color, color), matrix);

        foreach (var line in data.Lines)
        {
            var tinted = ColorMath.Tint(2130706432, color);
            Graphics2D.RenderWithTransform(g, line, tinted, matrix);
        }

        foreach (var circle in data.Circles)
        {
            var tinted = ColorMath.Tint(4294967295, color);
            Graphics2D.RenderWithTransform(g, circle, tinted, matrix);
        }
    }

    public class Data
    {
        public List<IList<VertexData>> Circles = null!;
        public List<IList<VertexData>> Lines = null!;

        public IList<VertexData> Poly1 = null!;
        public uint Poly1Color;

        public IList<VertexData> Poly2 = null!;
        public uint Poly2Color;

        public IList<VertexData> PolyStroke1 = null!;
        public uint PolyStroke1Color;

        public IList<VertexData> PolyStroke2 = null!;
        public uint PolyStroke2Color;
    }
}