// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Drawing;
using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// A gradient half-rectangle. Used in universe rendering.
/// </summary>
public static class Texture2
{
    private static readonly Data Instance;

    static Texture2()
    {
        Instance = CreateTexture();
    }
    
    public static Data CreateTexture()
    {
        const uint color1 = 4294967295;
        const uint color2 = 16777215;

        const long h = 4767413698560;
        const long x = 4771708665856;
        const long y = 0;

        var strokes = new List<IList<VertexData>>();
        var colors = new List<uint>();

        for (var i = y; i <= Fix64.Add(y, h); i += 2147483648)
        {
            var inter = Fix64.Map(i, y, Fix64.Add(y, h), 0, Fix64.One);
            var c = ColorMath.Lerp(color1, color2, inter);
            var s = h - i;

            var line = new CustomPath();
            line.MoveTo(x, s);
            line.LineTo(Fix64.Add(x, i), Fix64.Add(s, i));

            var stroke = new Stroke(line.Vertices());
            strokes.Add(Stroke.Vertices(stroke).ToList());
            colors.Add(c);
        }

        var data = new Data
        {
            Strokes = strokes,
            Colors = colors
        };

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, uint color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        for (var i = 0; i < data.Strokes.Count; i++)
        {
            var stroke = data.Strokes[i];
            var tinted = ColorMath.Tint(data.Colors[i], color);
            Graphics2D.RenderWithTransform(g, stroke, tinted, matrix);
        }
    }

    public class Data
    {
        public List<uint> Colors = null!;
        public List<IList<VertexData>> Strokes = null!;
    }
}