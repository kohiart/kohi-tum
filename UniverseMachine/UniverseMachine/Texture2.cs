// Copyright Kohi Art Community, Inc.. All rights reserved.

using System.Drawing;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine;

/// <summary>
/// A gradient half-rectangle. Used in universe rendering.
/// </summary>
public static class Texture2
{
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
        public List<Stroke> strokes;
        public List<Color> colors;
    }

    private static readonly Data _instance;

    static Texture2()
    {
        _instance = createTexture();
    }

    
    public static Data createTexture()
    {
        var color = Color.White;
        var color1 = Color.FromArgb(255, color.R, color.G, color.B);
        var color2 = Color.FromArgb(0, color1.R, color1.G, color1.B);

        var c0 = color.ToUInt32();
        var c1 = color1.ToUInt32();
        var c2 = color2.ToUInt32();

        // int64 w = 4767413698560;
        int64 h = 4767413698560;
        int64 x = 4771708665856;
        int64 y = 0;

        var strokes = new List<Stroke>();
        var colors = new List<Color>();

        for (int64 i = y; i <= Fix64.Add(y, h); i += 2147483648)
        {
            int64 inter = Fix64.Map(i, y, Fix64.Add(y, h), 0, Fix64.One);
            var c = ColorMath.Lerp(color1.ToUInt32(), color2.ToUInt32(), inter).ToColor();
            int64 s = h - i;

            CustomPath line = new CustomPath();
            line.MoveTo(x, s);
            line.LineTo(Fix64.Add(x, i), Fix64.Add(s, i));

            Stroke stroke = new Stroke(line.Vertices(), Fix64.One);
            strokes.Add(stroke);
            colors.Add(c);
        }

        var data = new Data();
        data.strokes = strokes;
        data.colors = colors;
        
        return data;
    }
    
    public static void draw(Graphics2D g, int64 x, int64 y, int64 r, int64 s, Matrix t, Color color)
    {
        var data = _instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        for (var i = 0; i < data.strokes.Count; i++)
        {
            Stroke stroke = data.strokes[i];
            var tinted = ColorMath.Tint(data.colors[i].ToUInt32(), color.ToUInt32());
            Graphics2D.RenderWithTransform(g, Stroke.Vertices(stroke).ToList(), tinted, matrix);
        }
    }

}