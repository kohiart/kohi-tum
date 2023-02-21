// Copyright Kohi Art Community, Inc.. All rights reserved.

using System.Drawing;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;


namespace UniverseMachine;

/// <summary>
/// Three circles. Used in universe rendering.
/// </summary>
public static class Texture3
{
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
        public Ellipse circle1;
        public Ellipse circle2;
        public Ellipse circle3;
        public Ellipse circle4;
        public Color color1;
        public Color color2;
        public Color color3;
        public Color color4;
    }

    private static readonly Data _instance;

    static Texture3()
    {
        _instance = createTexture();
    }

    public static Data createTexture()
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
        
        data.circle1 = circle1;
        data.circle2 = circle2;
        data.circle3 = circle3;
        data.circle4 = circle4;
        data.color1 = color1;
        data.color2 = color2;
        data.color3 = color3;
        data.color4 = color4;

        return data;
    }
    
    public static void draw(Graphics2D g, int64 x, int64 y, int64 r, int64 s, Matrix t, Color color)
    {
        var data = _instance;
        var matrix = Textures.Rectify(x, y, r, s, t);

        var tinted1 = ColorMath.Tint(data.color1.ToUInt32(), color.ToUInt32());
        var tinted2 = ColorMath.Tint(data.color2.ToUInt32(), color.ToUInt32());
        var tinted3 = ColorMath.Tint(data.color3.ToUInt32(), color.ToUInt32());
        var tinted4 = ColorMath.Tint(data.color4.ToUInt32(), color.ToUInt32());

        Graphics2D.RenderWithTransform(g, data.circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.circle2.Vertices().ToList(), tinted2, matrix);
        Graphics2D.RenderWithTransform(g, data.circle3.Vertices().ToList(), tinted3, matrix);
        Graphics2D.RenderWithTransform(g, data.circle4.Vertices().ToList(), tinted4, matrix);
    }
}