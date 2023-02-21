// Copyright Kohi Art Community, Inc.. All rights reserved.

using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine;

/// <summary>
/// Small inset circles, used to draw stars.
/// </summary>
public static class Texture5
{
    /*
        .st0{fill:#FFFFFF; fill-opacity:0.1;}            
        <circle class="st0" cx="500" cy="500" r="7"/>
        .st1{fill:#FFFFFF; fill-opacity:0.8;}
        <circle class="st1" cx="500" cy="500" r="3"/>`;
    */

    public class Data
    {
        public Ellipse circle1;
        public uint color1;

        public Ellipse circle2;
        public uint color2;
    }

    private static readonly Data _instance;

    static Texture5()
    {
        _instance = createTexture();
    }

    //private static Ellipse ellipse(int originX, int originY, int radiusX, int radiusY)
    //{
    //    var ox = Fix64V1.mul(originX * Fix64V1.ONE, Textures.FactorF);
    //    var oy = Fix64V1.mul(originY * Fix64V1.ONE, Textures.FactorF);

    //    return new Ellipse(
    //        (long)((500 * Textures.Factor) * Fix64V1.ONE),
    //        (long)((2222 - 500 * Textures.Factor) * Fix64V1.ONE),
    //        (long)((7 * Textures.Factor) * Fix64V1.ONE),
    //        (long)(7 * Textures.Factor * Fix64V1.ONE));
    //}

    public static Data createTexture()
    {
        

        var circle1 = new Ellipse(4767413698560, 4776003633152, 66743791616, 66743791616);
        var circle2 = new Ellipse(4767413698560, 4776003633152, 28604481536, 28604481536);

        //var circle1 = new Ellipse(
        //    (long)((500 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((2222 - 500 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((7 * Textures.Factor) * Fix64V1.ONE),
        //    (long)(7 * Textures.Factor * Fix64V1.ONE));

        //var circle2 = new Ellipse(
        //    (long)((500 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((2222 - 500 * Textures.Factor) * Fix64V1.ONE),
        //    (long)((3 * Textures.Factor) * Fix64V1.ONE),
        //    (long)(3 * Textures.Factor * Fix64V1.ONE)
        //);

        var color1 = 436207615U; // Color.FromArgb(25 /* 0.10 */, Color.White.R, Color.White.G, Color.White.B);
        var color2 = 3439329279U; // Color.FromArgb(204 /* 0.8 */, Color.White.R, Color.White.G, Color.White.B);

        var data = new Data();
        data.circle1 = circle1;
        data.circle2 = circle2;
        data.color1 = color1;
        data.color2 = color2;

        return data;
    }
    
    public static void draw(Graphics2D g, int64 x, int64 y, int64 s, Matrix t, uint color)
    {
        var data = _instance;
        var matrix = Textures.Rectify(x, y, 0, s, t);

        var tinted1 = ColorMath.Tint(data.color1, color);
        var tinted2 = ColorMath.Tint(data.color2, color);

        Graphics2D.RenderWithTransform(g, data.circle1.Vertices().ToList(), tinted1, matrix);
        Graphics2D.RenderWithTransform(g, data.circle2.Vertices().ToList(), tinted2, matrix);
    }
}