// Copyright Kohi Art Community, Inc.. All rights reserved.

using System.Drawing;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine;

/// <summary>
/// A white circle and a smaller black circle. Used in universe rendering.
/// </summary>
public static class Texture0
{
    /*          
        .st0{fill:#FFFFFF; fill-opacity:1.0; stroke:#000000; stroke-width:0.0; stroke-opacity:0.0; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10; }
        <circle class="st0" cx="300" cy="300" r="200"/>

        .st1{fill:#000000; fill-opacity:1.0; stroke:#000000; stroke-width:0.25; stroke-opacity:0.25; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10; }
        <circle class="st1" cx="300" cy="500" r="10"/>`;
    */

    public class Data
    {
        public Ellipse circle1;
        public Ellipse circle2;
        public Stroke stroke1;
        public Stroke stroke2;

        public Color circleColor1;
        public Color circleColor2;
        public Color strokeColor1;
        public Color strokeColor2;
    }

    private static readonly Data _instance;

    static Texture0()
    {
        _instance = createTexture();
    }

    public static Data createTexture()
    {
        //var circle1 = new Ellipse(
        //    (long)((300 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((2222 - (300 * Textures.Factor)) * Fix64V1.ONE),
        //    (long)((200 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((200 * Textures.Factor) * Fix64V1.ONE));

        //var stroke1 = new Stroke(circle1.vertices(), 0);
        //stroke1.lineCap = LineCap.Round;
        //stroke1.lineJoin = LineJoin.Round;

        //var circleColor1 = Color.White;
        //var strokeColor1 = Color.FromArgb((byte)(255 * 0.0f), Color.White.R, Color.White.G, Color.White.B);

        //var circle2 = new Ellipse(
        //    (long)((300 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((2222 - (500 * Textures.Factor)) * Fix64V1.ONE),
        //    (long)((10 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((10 * Textures.Factor) * Fix64V1.ONE)
        //);

        //var stroke2 = new Stroke(circle2.vertices(), 0.25f);

        var circle1 = new Ellipse(2860448219136, 6682969112576,1906965479424, 1906965479424);
        var stroke1 = new Stroke(circle1.Vertices(), 0);
        stroke1.LineCap = LineCap.Round;
        stroke1.LineJoin = LineJoin.Round;

        var circle2 = new Ellipse(2860448219136, 4776003633152, 95348277248, 95348277248);

        // long widthFixed = 1073741824 /* 0.25 */;
        var stroke2 = new Stroke(circle2.Vertices(), 1073741824 /* 0.25 */);


        var circleColor1 = Color.White;
        var strokeColor1 = Color.FromArgb(0, Color.White.R, Color.White.G, Color.White.B);
        var circleColor2 = Color.Black;
        var strokeColor2 = Color.FromArgb(63, Color.Black.R, Color.Black.G, Color.Black.B);

        //var cc1 = circleColor1.ToUInt32();
        //var sc1 = strokeColor1.ToUInt32();
        //var cc2 = circleColor2.ToUInt32();
        //var sc2 = strokeColor2.ToUInt32();

        var data = new Data
        {
            circle1 = circle1,
            stroke1 = stroke1,
            circle2 = circle2,
            stroke2 = stroke2,
            circleColor1 = circleColor1,
            strokeColor1 = strokeColor1,
            circleColor2 = circleColor2,
            strokeColor2 = strokeColor2
        };

        return data;
    }
    
    public static void draw(Graphics2D g, int64 x, int64 y, int64 r, int64 s, Matrix t, Color color)
    {
        var data = _instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.circleColor1.ToUInt32(), color.ToUInt32());
        var tinted2 = ColorMath.Tint(data.strokeColor1.ToUInt32(), color.ToUInt32());
        var tinted3 = ColorMath.Tint(data.circleColor2.ToUInt32(), color.ToUInt32());
        var tinted4 = ColorMath.Tint(data.strokeColor2.ToUInt32(), color.ToUInt32());

        Graphics2D.RenderWithTransform(g, data.circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.stroke1).ToList(), tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.circle2.Vertices().ToList(), tinted3, matrix);
        Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.stroke2).ToList(), tinted4, matrix);
    }
}