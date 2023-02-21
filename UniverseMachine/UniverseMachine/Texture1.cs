// Copyright Kohi Art Community, Inc.. All rights reserved.

using System.Drawing;
using Kohi.Composer;

namespace UniverseMachine;

using int64 = System.Int64;
using int32 = System.Int32;

/// <summary>
/// A rectangle with a shadow. Used in universe rendering.
/// </summary>
public static class Texture1
{
    /*
        .st1{fill:#000000; fill-opacity:0.025; stroke:none;}
        <rect x="310" y="310" class="st1" width="400" height="400"/>

        .st0{fill:#FFFFFF; fill-opacity:1.0; stroke:#000000; stroke-width:0.0; stroke-opacity:0.0; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10;
        <rect x="300" y="300" class="st0" width="400" height="400"/>
     */

    public class Data
    {
        public CustomPath rect1;
        public CustomPath rect2;
        public Color color1;
        public Color color2;
        public Color color3;
    }

    private static CustomPath rect(int32 x, int32 y, int32 width, int32 height)
    {
        var line = new CustomPath();

        // TL to BL
        line.MoveTo(x * Fix64.One, y * Fix64.One);
        line.LineTo((x + width) * Fix64.One, y * Fix64.One);
        line.LineTo((x + width) * Fix64.One, (y + height) * Fix64.One);
        line.LineTo(x * Fix64.One, (y + height) * Fix64.One);
        line.LineTo(x * Fix64.One, y * Fix64.One);

        return line;
    }

    private static readonly Data _instance;
    
    static Texture1()
    {
        _instance = createTexture();
    }

    public static Data createTexture()
    {
        var rect1 = rect(688, 644, 890, 890);
        var rect2 = rect(666, 666, 890, 890);

        var color1 = Color.FromArgb(6, Color.Black.R, Color.Black.G, Color.Black.B);
        var color2 = Color.FromArgb(255, Color.White.R, Color.White.G, Color.White.B);
        var color3 = Color.FromArgb(0, 0, 0, 0);

        var data = new Data
        {
            rect1 = rect1,
            color1 = color1,
            rect2 = rect2,
            color2 = color2,
            color3 = color3
        };

        return data;
    }
    
    public static Matrix draw(Graphics2D g, int64 x, int64 y, int64 r, int64 s, Matrix t, Color color, int count)
    {
        var data = _instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.color1.ToUInt32(), color.ToUInt32());
        var v1 = data.rect1.Vertices().ToList();
        Graphics2D.RenderWithTransform(g, v1, tinted1, matrix);

        var tinted2 = ColorMath.Tint(data.color2.ToUInt32(), color.ToUInt32());
        var v2 = data.rect2.Vertices().ToList();
        Graphics2D.RenderWithTransform(g, v2, tinted2, matrix);

        return matrix;
    }
}