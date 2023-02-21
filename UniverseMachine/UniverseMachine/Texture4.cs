using System.Drawing;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine
{
    /// <summary>
    /// Weird contraption. Used in universe rendering.
    /// </summary>
    public static class Texture4
    {
        /*
            .st0{ stroke:#000000; stroke-width:0.5; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10; stroke-opacity:0.1; fill:#FFFFFF; fill-opacity:0.75; }
            .st1{ stroke:#000000; stroke-width:0.5; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10; stroke-opacity:0.1; fill:#FFFFFF; fill-opacity:0.5;  }
            .st2{ stroke:#000000; stroke-width:0.25; stroke-linecap:round; stroke-linejoin:round; stroke-miterlimit:10; stroke-opacity:0.5; fill:none;}
            .st3{ fill:#FFFFFF; }

            <polygon class="st0" points="799.2,562.6 584.2,673.4 830.2,664.8 "/>
            <polygon class="st1" points="937.7,545.9 799.2,562.6 830.2,664.8 "/>

            <line class="st2" x1="0" y1="600.6" x2="584.2" y2="673.4"/>
            <line class="st2" x1="1000" y1="507.1" x2="937.7" y2="545.9"/>

            <circle class="st3" cx="454.7" cy="462.8" r="2.4"/>
            <circle class="st3" cx="503.6" cy="468.7" r="2.4"/>
            <circle class="st3" cx="552.5" cy="474.5" r="2.4"/>
            <circle class="st3" cx="601.4" cy="480.4" r="2.4"/>
            <circle class="st3" cx="650.3" cy="486.3" r="2.4"/>
            <circle class="st3" cx="699.2" cy="492.1" r="2.4"/>
            <circle class="st3" cx="748.1" cy="498" r="2.4"/>
            <circle class="st3" cx="797" cy="503.8" r="2.4"/>
            <circle class="st3" cx="845.9" cy="509.7" r="2.4"/>
            <circle class="st3" cx="894.8" cy="515.5" r="2.4"/>
            <circle class="st3" cx="479.2" cy="544.8" r="3.3"/>
            <circle class="st3" cx="625.8" cy="562.4" r="3.3"/>
            <circle class="st3" cx="454.7" cy="615.1" r="4.1"/>
            <circle class="st3" cx="430.3" cy="691.2" r="4.9"/>`;
         */

        public class Data
        {
            public List<Ellipse> circles;
            public List<Stroke> lines;

            public CustomPath poly1;
            public Color poly1Color;
            public Stroke polyStroke1;
            public Color polyStroke1Color;

            public CustomPath poly2;
            public Color poly2Color;
            public Stroke polyStroke2;
            public Color polyStroke2Color;
        }

        private static readonly Data _instance;

        static Texture4()
        {
            _instance = createTexture();
        }

        public static Data createTexture()
        {
            var data = new Data();

            //var poly1 = new CustomPath();
            //poly1.moveTo((long)((799.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 562.6f * Textures.Factor) * Fix64V1.ONE));
            //poly1.lineTo((long)((584.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 673.4f * Textures.Factor) * Fix64V1.ONE));
            //poly1.lineTo((long)((830.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 664.8f * Textures.Factor) * Fix64V1.ONE));
            //var polyStroke1 = new Stroke(poly1.vertices(), 0.5f);
            //polyStroke1.lineCap = LineCap.Round;
            //polyStroke1.lineJoin = LineJoin.Round;

            //data.poly1 = poly1;
            //data.polyStroke1 = polyStroke1;

            //var poly2 = new CustomPath();
            //poly2.moveTo((long)((937.7f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 545.9f * Textures.Factor) * Fix64V1.ONE));
            //poly2.lineTo((long)((799.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 562.6f * Textures.Factor) * Fix64V1.ONE));
            //poly2.lineTo((long)((830.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 664.8f * Textures.Factor) * Fix64V1.ONE));
            //poly2.lineTo((long)((937.7f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 545.9f * Textures.Factor) * Fix64V1.ONE));
            //var polyStroke2 = new Stroke(poly2.vertices(), 0.5f);
            //polyStroke2.lineCap = LineCap.Round;
            //polyStroke2.lineJoin = LineJoin.Round;

            long foo1 = 2147483648 /* 0.5 */;

            CustomPath poly1 = new CustomPath();
            poly1.MoveTo(7620234051584, 4179123765248);
            poly1.LineTo(5570246475776, 3122664046592);
            poly1.LineTo(7915813994496, 3204664262656);
            Stroke polyStroke1 = new Stroke(poly1.Vertices(), 2147483648 /* 0.5 */);
            polyStroke1.LineCap = LineCap.Round;
            polyStroke1.LineJoin = LineJoin.Round;

            data.poly1 = poly1;
            data.polyStroke1 = polyStroke1;

            CustomPath poly2 = new CustomPath();
            poly2.MoveTo(8940808044544, 4338354749440);
            poly2.LineTo(7620234051584, 4179123765248);
            poly2.LineTo(7915813994496, 3204664262656);
            poly2.LineTo(8940808044544, 4338354749440);
            Stroke polyStroke2 = new Stroke(poly2.Vertices(), 2147483648 /* 0.5 */);
            polyStroke2.LineCap = LineCap.Round;
            polyStroke2.LineJoin = LineJoin.Round;

            data.poly2 = poly2;
            data.polyStroke2 = polyStroke2;

            //var line1 = new CustomPath();
            //line1.moveTo((long)((0 * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 600.6f * Textures.Factor) * Fix64V1.ONE));
            //line1.lineTo((long)((584.2f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 673.4f * Textures.Factor) * Fix64V1.ONE));
            //var stroke1 = new Stroke(line1.vertices(), 0.25f);
            //stroke1.lineCap = LineCap.Round;
            //stroke1.lineJoin = LineJoin.Round;

            //var line2 = new CustomPath();
            //line2.moveTo((long)((1000f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 507.1f * Textures.Factor) * Fix64V1.ONE));
            //line2.lineTo((long)((937.7f * Textures.Factor) * Fix64V1.ONE), (long)((2222 - 545.9f * Textures.Factor) * Fix64V1.ONE));

            //var stroke2 = new Stroke(line2.vertices(), 0.25f);
            //stroke2.lineCap = LineCap.Round;
            //stroke2.lineJoin = LineJoin.Round;

            data.lines = new List<Stroke>();

            CustomPath line1 = new CustomPath();
            line1.MoveTo(0, 3816800387072);
            line1.LineTo(5570246475776, 3122664046592);
            var stroke1 = new Stroke(line1.Vertices(), 1073741824 /* 0.25 */);
            stroke1.LineCap = LineCap.Round;
            stroke1.LineJoin = LineJoin.Round;

            CustomPath line2 = new CustomPath();
            line2.MoveTo(9534827397120, 4708305993728);
            line2.LineTo(8940808044544, 4338354749440);

            Stroke stroke2 = new Stroke(line2.Vertices(), 1073741824 /* 0.25 */);
            stroke2.LineCap = LineCap.Round;
            stroke2.LineJoin = LineJoin.Round;

            data.lines.Add(stroke1);
            data.lines.Add(stroke2);
            
            //coords.Add(new Circle(454.7f * Textures.Factor * Fix64V1.ONE, (2222 - 462.8f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(503.6f * Textures.Factor * Fix64V1.ONE, (2222 - 468.7f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(552.5f * Textures.Factor * Fix64V1.ONE, (2222 - 474.5f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(601.4f * Textures.Factor * Fix64V1.ONE, (2222 - 480.4f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(650.3f * Textures.Factor * Fix64V1.ONE, (2222 - 486.3f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(699.2f * Textures.Factor * Fix64V1.ONE, (2222 - 492.1f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(748.1f * Textures.Factor * Fix64V1.ONE, (2222 - 498.0f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(797.0f * Textures.Factor * Fix64V1.ONE, (2222 - 503.8f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(845.9f * Textures.Factor * Fix64V1.ONE, (2222 - 509.7f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(894.8f * Textures.Factor * Fix64V1.ONE, (2222 - 515.5f * Textures.Factor) * Fix64V1.ONE, 2.4f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(479.2f * Textures.Factor * Fix64V1.ONE, (2222 - 544.8f * Textures.Factor) * Fix64V1.ONE, 3.3f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(625.8f * Textures.Factor * Fix64V1.ONE, (2222 - 562.4f * Textures.Factor) * Fix64V1.ONE, 3.3f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(454.7f * Textures.Factor * Fix64V1.ONE, (2222 - 615.1f * Textures.Factor) * Fix64V1.ONE, 4.1f * Textures.Factor * Fix64V1.ONE));
            //coords.Add(new Circle(430.3f * Textures.Factor * Fix64V1.ONE, (2222 - 691.2f * Textures.Factor) * Fix64V1.ONE, 4.9f * Textures.Factor * Fix64V1.ONE));

            data.circles = new List<Ellipse>();
            data.circles.Add(new Ellipse(4335486107648, 5130699145216, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(4801739358208, 5074443567104, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(5267992346624, 5019141668864, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(5734245335040, 4962886090752, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(6200498323456, 4906631036928, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(6666751311872, 4851328614400, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(7133004300288, 4795073036288, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(7599257288704, 4739771138048, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(8065510801408, 4683515559936, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(8531763789824, 4628213661696, 22883586048, 22883586048));
            data.circles.Add(new Ellipse(4569089703936, 4348843655168, 31464931328, 31464931328));
            data.circles.Add(new Ellipse(5966894989312, 4181030076416, 31464931328, 31464931328));
            data.circles.Add(new Ellipse(4335486107648, 3678545117184, 39092793344, 39092793344));
            data.circles.Add(new Ellipse(4102836191232, 2952944680960, 46720655360, 39092793344));

            var poly1Color = Color.FromArgb((byte)(255 * 0.75f), Color.White.R, Color.White.G, Color.White.B);
            var strokeColor = Color.FromArgb(255, Color.Black.R, Color.Black.G, Color.Black.B);
            var poly2Color = Color.FromArgb((byte)(255 * 0.50f), Color.White.R, Color.White.G, Color.White.B);

            //var p1c = poly1Color.ToUInt32();
            //var sc = strokeColor.ToUInt32();
            //var p2c = poly2Color.ToUInt32();

            data.poly1Color = poly1Color;
            data.polyStroke1Color = strokeColor;
            data.poly2Color = poly2Color;
            data.polyStroke2Color = strokeColor;

            return data;
        }
        
        public static void draw(Graphics2D g, int64 x, int64 y, int64 r, int64 s, Matrix t, Color color)
        {
            var data = _instance;
            var matrix = Textures.Rectify(x, y, r, s, t);

            Graphics2D.RenderWithTransform(g, data.poly1.Vertices().ToList(), ColorMath.Tint(data.poly1Color.ToUInt32(), color.ToUInt32()), matrix);
            Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.polyStroke1).ToList(), ColorMath.Tint(data.polyStroke1Color.ToUInt32(), color.ToUInt32()), matrix);
            Graphics2D.RenderWithTransform(g, data.poly2.Vertices().ToList(), ColorMath.Tint(data.poly2Color.ToUInt32(), color.ToUInt32()), matrix);
            Graphics2D.RenderWithTransform(g, Stroke.Vertices(data.polyStroke2).ToList(), ColorMath.Tint(data.polyStroke2Color.ToUInt32(), color.ToUInt32()), matrix);

            foreach (var line in data.lines)
            {
                var tinted = ColorMath.Tint(
                    Color.FromArgb((byte) (255 * 0.5f), Color.Black.R, Color.Black.G, Color.Black.B).ToUInt32(),
                    color.ToUInt32()
                    );

                Graphics2D.RenderWithTransform(g, Stroke.Vertices(line).ToList(), tinted, matrix);
            }

            foreach (var circle in data.circles)
            {
                var tinted = ColorMath.Tint(Color.White.ToUInt32(), color.ToUInt32());
                Graphics2D.RenderWithTransform(g, circle.Vertices().ToList(), tinted, matrix);
            }
        }
    }
}
