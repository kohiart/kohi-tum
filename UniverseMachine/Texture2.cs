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
        var color = Color.White;
        var color1 = Color.FromArgb(255, color.R, color.G, color.B);
        var color2 = Color.FromArgb(0, color1.R, color1.G, color1.B);

        var c0 = color.ToUInt32();
        var c1 = color1.ToUInt32();
        var c2 = color2.ToUInt32();

        // int64 w = 4767413698560;
        var h = 4767413698560;
        var x = 4771708665856;
        long y = 0;

        var strokes = new List<Stroke>();
        var colors = new List<Color>();

        for (var i = y; i <= Fix64.Add(y, h); i += 2147483648)
        {
            var inter = Fix64.Map(i, y, Fix64.Add(y, h), 0, Fix64.One);
            var c = ColorMath.Lerp(color1.ToUInt32(), color2.ToUInt32(), inter).ToColor();
            var s = h - i;

            var line = new CustomPath();
            line.MoveTo(x, s);
            line.LineTo(Fix64.Add(x, i), Fix64.Add(s, i));

            var stroke = new Stroke(line.Vertices());
            strokes.Add(stroke);
            colors.Add(c);
        }

        var data = new Data();
        data.Strokes = strokes;
        data.Colors = colors;

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, Color color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        for (var i = 0; i < data.Strokes.Count; i++)
        {
            var stroke = data.Strokes[i];
            var tinted = ColorMath.Tint(data.Colors[i].ToUInt32(), color.ToUInt32());
            Graphics2D.RenderWithTransform(g, Stroke.Vertices(stroke).ToList(), tinted, matrix);
        }
    }
    /*
        var texture2 = so + `.st0{fill:url(#SVGID_1_);}
        ` + sc + `
        <linearGradient id="SVGID_1_" gradientUnits="userSpaceOnUse" x1="500" y1="500" x2="750" y2="750">
        <stop  offset="0" style="stop-color:#FFFFFF; stop-opacity:1.0"/>
        <stop  offset="1" style="stop-color:#FFFFFF; stop-opacity:0.0"/>
        </linearGradient>
        <polygon class="st0" points="500,1000 500,500 1000,500 "/>`;
    */

    public class Data
    {
        public List<Color> Colors;
        public List<Stroke> Strokes;
    }
}