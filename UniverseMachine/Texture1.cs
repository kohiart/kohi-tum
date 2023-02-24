// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

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

        var data = new Data
        {
            Rect1 = rect1.Vertices().ToList(),
            Color1 = 100663296,
            Rect2 = rect2.Vertices().ToList(),
            Color2 = 4294967295
        };

        return data;
    }

    public static Matrix Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, uint color, int count)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.Color1, color);
        Graphics2D.RenderWithTransform(g, data.Rect1, tinted1, matrix);

        var tinted2 = ColorMath.Tint(data.Color2, color);
        Graphics2D.RenderWithTransform(g, data.Rect2, tinted2, matrix);

        return matrix;
    }
    
    public class Data
    {
        public uint Color1;
        public uint Color2;
        public IList<VertexData> Rect1 = null!;
        public IList<VertexData> Rect2 = null!;
    }
}