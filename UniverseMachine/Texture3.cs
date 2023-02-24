// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

/// <summary>
/// Three circles. Used in universe rendering.
/// </summary>
public static class Texture3
{
    private static readonly Data Instance;

    static Texture3()
    {
        Instance = CreateTexture();
    }

    public static Data CreateTexture()
    {
        var data = new Data();

        var circle1 = new Ellipse(2860448219136, 6682969112576, 1430224109568, 1430224109568);
        var circle2 = new Ellipse(2860448219136, 8351563907072, 190696554496, 190696554496);
        var circle3 = new Ellipse(2860448219136, 8351563907072, 47674138624, 47674138624);
        var circle4 = new Ellipse(2860448219136, 8780631244800, 95348277248, 95348277248);
        
        data.Circle1 = circle1.Vertices().ToList();
        data.Circle2 = circle2.Vertices().ToList();
        data.Circle3 = circle3.Vertices().ToList();
        data.Circle4 = circle4.Vertices().ToList();
        data.Color1 = 4294967295;
        data.Color2 = 50331648;
        data.Color3 = 4278190080;
        data.Color4 = 4294967295;

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, uint color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.Color1, color);
        var tinted2 = ColorMath.Tint(data.Color2, color);
        var tinted3 = ColorMath.Tint(data.Color3, color);
        var tinted4 = ColorMath.Tint(data.Color4, color);

        Graphics2D.RenderWithTransform(g, data.Circle1, tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle2, tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle3, tinted3, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle4, tinted4, matrix);
    }

    public class Data
    {
        public IList<VertexData> Circle1 = null!;
        public IList<VertexData> Circle2 = null!;
        public IList<VertexData> Circle3 = null!;
        public IList<VertexData> Circle4 = null!;
        public uint Color1;
        public uint Color2;
        public uint Color3;
        public uint Color4;
    }
}