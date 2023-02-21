// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Drawing;
using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// A rectangle with a shadow. Used in universe rendering.
/// </summary>
public static class Texture1
{
    private static readonly Data Instance;

    static Texture1()
    {
        Instance = CreateTexture();
    }

    private static CustomPath Rect(int x, int y, int width, int height)
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

    public static Data CreateTexture()
    {
        var rect1 = Rect(688, 644, 890, 890);
        var rect2 = Rect(666, 666, 890, 890);

        var color1 = Color.FromArgb(6, Color.Black.R, Color.Black.G, Color.Black.B);
        var color2 = Color.FromArgb(255, Color.White.R, Color.White.G, Color.White.B);
        var color3 = Color.FromArgb(0, 0, 0, 0);

        var data = new Data
        {
            Rect1 = rect1,
            Color1 = color1,
            Rect2 = rect2,
            Color2 = color2,
            Color3 = color3
        };

        return data;
    }

    public static Matrix Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, Color color, int count)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.Color1.ToUInt32(), color.ToUInt32());
        var v1 = data.Rect1.Vertices().ToList();
        Graphics2D.RenderWithTransform(g, v1, tinted1, matrix);

        var tinted2 = ColorMath.Tint(data.Color2.ToUInt32(), color.ToUInt32());
        var v2 = data.Rect2.Vertices().ToList();
        Graphics2D.RenderWithTransform(g, v2, tinted2, matrix);

        return matrix;
    }
    /*
        .st1{fill:#000000; fill-opacity:0.025; stroke:none;}
        <rect x="310" y="310" class="st1" width="400" height="400"/>

        .st0{fill:#FFFFFF; fill-opacity:1.0; stroke:#000000; stroke-width:0.0; stroke-opacity:0.0; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10;
        <rect x="300" y="300" class="st0" width="400" height="400"/>
     */

    public class Data
    {
        public Color Color1;
        public Color Color2;
        public Color Color3;
        public CustomPath Rect1;
        public CustomPath Rect2;
    }
}