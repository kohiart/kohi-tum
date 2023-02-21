// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// Small inset circles, used to draw stars.
/// </summary>
public static class Texture5
{
    private static readonly Data Instance;

    static Texture5()
    {
        Instance = CreateTexture();
    }

    public static Data CreateTexture()
    {
        var circle1 = new Ellipse(4767413698560, 4776003633152, 66743791616, 66743791616);
        var circle2 = new Ellipse(4767413698560, 4776003633152, 28604481536, 28604481536);

        const uint color1 = 436207615U;
        const uint color2 = 3439329279U;

        var data = new Data
        {
            Circle1 = circle1,
            Circle2 = circle2,
            Color1 = color1,
            Color2 = color2
        };

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long s, Matrix t, uint color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, 0, s, t);

        var tinted1 = ColorMath.Tint(data.Color1, color);
        var tinted2 = ColorMath.Tint(data.Color2, color);

        Graphics2D.RenderWithTransform(g, data.Circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle2.Vertices().ToList(), tinted2, matrix);
    }

    public class Data
    {
        public Ellipse Circle1 = null!;
        public Ellipse Circle2 = null!;
        public uint Color1;
        public uint Color2;
    }
}