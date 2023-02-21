// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Drawing;
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

        var color1 = Color.White;
        var color2 = Color.FromArgb(3, Color.Black.R, Color.Black.G, Color.Black.B);
        var color3 = Color.FromArgb(255, Color.Black.R, Color.Black.G, Color.Black.B);
        var color4 = Color.White;

        data.Circle1 = circle1;
        data.Circle2 = circle2;
        data.Circle3 = circle3;
        data.Circle4 = circle4;
        data.Color1 = color1;
        data.Color2 = color2;
        data.Color3 = color3;
        data.Color4 = color4;

        return data;
    }

    public static void Draw(Graphics2D g, long x, long y, long r, long s, Matrix t, Color color)
    {
        var data = Instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.Color1.ToUInt32(), color.ToUInt32());
        var tinted2 = ColorMath.Tint(data.Color2.ToUInt32(), color.ToUInt32());
        var tinted3 = ColorMath.Tint(data.Color3.ToUInt32(), color.ToUInt32());
        var tinted4 = ColorMath.Tint(data.Color4.ToUInt32(), color.ToUInt32());

        Graphics2D.RenderWithTransform(g, data.Circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle2.Vertices().ToList(), tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle3.Vertices().ToList(), tinted3, matrix);
        Graphics2D.RenderWithTransform(g, data.Circle4.Vertices().ToList(), tinted4, matrix);
    }
    /*
        .st1{fill:#FFFFFF;}
        <circle class="st1" cx="300" cy="300" r="150"/>

        .st2{fill-opacity:0.01;}
        <circle class="st2" cx="300" cy="125" r="20"/>

       <circle cx="300" cy="125" r="5"/>

       .st1{fill:#FFFFFF;}
       <circle class="st1" cx="300" cy="80" r="10"/>
    */

    public class Data
    {
        public Ellipse Circle1;
        public Ellipse Circle2;
        public Ellipse Circle3;
        public Ellipse Circle4;
        public Color Color1;
        public Color Color2;
        public Color Color3;
        public Color Color4;
    }
}