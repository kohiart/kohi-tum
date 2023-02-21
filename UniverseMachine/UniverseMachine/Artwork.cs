using System.Diagnostics;
using System.Runtime.CompilerServices;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;
using Color = System.Drawing.Color;

namespace UniverseMachine
{
    public sealed class Artwork
    {
        public const float MasterScale = 0.6967592592592593f;
        public const int64 MasterScaleF = 2992558231; /* 0.6967592592592593 */

        public const float BaseSize = 2222f;

        public static readonly Matrix MasterOrigin = Matrix.NewTranslation(
            (long) ((Parameters.StageW / 2f) * Fix64.One), 
            (long) ((Parameters.StageH / 2f) * Fix64.One)
            );

        public static readonly Matrix MasterOriginF = Matrix.NewTranslation(
            3231962890240,
            4784593567744
        );

        public readonly Parameters parameters;
        private readonly int seed;

        public int Width => Parameters.StageW;
        public int Height => Parameters.StageH;

        public Artwork(int seed, int? whichMasterSet = null, int? colorOverride = null)
        {
            var xorShift = new XorShift(seed);
            this.seed = seed;
            parameters = new Parameters(xorShift, whichMasterSet, colorOverride);
        }

        public void RenderCanvas(Graphics2D g, Matrix canvasCorner, int scale, Layer layer = Layer.All)
        {
            var sw = Stopwatch.StartNew();

            if(layer.HasFlagFast(Layer.Background))
                renderBackground(g, parameters);
            if(layer.HasFlagFast(Layer.Grid))
                renderGridDots(g, parameters, canvasCorner, scale);
            if(layer.HasFlagFast(Layer.Skeleton))
                renderSkeleton(g, parameters, canvasCorner, scale);
            if(layer.HasFlagFast(Layer.Universe))
                renderUniverse(g, parameters, canvasCorner, seed, scale);
            if(layer.HasFlagFast(Layer.Stars))
                renderStars(g, parameters, canvasCorner, scale);
            if(layer.HasFlagFast(Layer.Mats))
                renderMats(g, scale);

            Trace.TraceInformation($"Composer completed in {sw.Elapsed}");
        }

        private static void renderBackground(Graphics2D g, Parameters parameters)
        {
            Trace.TraceInformation("Composing background");

            var c = parameters.starPositions[0].c % parameters.cLen;
            
            var background = ColorMath.ToColor(
                255,
                parameters.myColorsR[c],
                parameters.myColorsG[c],
                parameters.myColorsB[c]
            );

            Graphics2D.Clear(g, background);
        }

        private void renderMats(Graphics2D g, int scale)
        {
            Trace.TraceInformation("Composing mats");

            var edge = 85 * scale;

            var count = 0;
            renderMat(g, edge--, getMatColor(150));
            count++;
                        
            for (var i = 0; i < 8 * scale; i++)
            {
                renderMat(g, edge--, getMatColor(50));
                count++;
            }

            for (var i = 0; i < 77 * scale; i++)
            {
                renderMat(g, edge--, getMatColor(0));
                count++;
            }


        }

        private static void renderMat(Graphics2D g, int edge, Color color)
        {
            var line = new CustomPath();

            // TL to BL
            line.MoveTo(edge * Fix64.One, (g.Height - edge) * Fix64.One);
            line.LineTo(edge * Fix64.One, edge * Fix64.One);

            // BL to BR
            line.MoveTo((edge + 1) * Fix64.One, (edge + 1) * Fix64.One);
            line.LineTo((g.Width - edge) * Fix64.One, (edge + 1) * Fix64.One);

            // TL to TR
            line.MoveTo((edge + 1) * Fix64.One, (g.Height - edge - 1) * Fix64.One);
            line.LineTo((g.Width - edge) * Fix64.One, (g.Height - edge - 1) * Fix64.One);

            // TR to BR
            line.MoveTo((g.Width - edge - 1) * Fix64.One, (g.Height - edge - 2) * Fix64.One);
            line.LineTo((g.Width - edge - 1) * Fix64.One, (edge + 2) * Fix64.One);

            var stroke = new Stroke(line.Vertices(), Fix64.One);
            var vertices = Stroke.Vertices(stroke).ToList();

            Graphics2D.Render(g, vertices, color.ToUInt32(), blend: false);
        }

        private Color getMatColor(int index)
        {
            var colorIndex = parameters.starPositions[0].c;

            var color = Color.FromArgb(
                255,
                parameters.myColorsR[(colorIndex + index) % parameters.cLen],
                parameters.myColorsG[(colorIndex + index) % parameters.cLen],
                parameters.myColorsB[(colorIndex + index) % parameters.cLen]);
            
            return color;
        }

        private static void renderGridDots(Graphics2D g, Parameters parameters, Matrix canvasCorner, int scale)
        {
            Trace.TraceInformation("Composing grid");

            int64 radius = 19327352832 * scale;

            var e = new Ellipse(0, 0, radius, radius);

            for (var i = 0; i < Parameters.GridSize; i++)
            {
                var black = Color.Black;
                e.OriginX = parameters.gridPoints[i].X * scale;
                e.OriginY = parameters.gridPoints[i].Y * scale;

                var origin = Matrix.NewTranslation(
                    (long)((Parameters.StageW * scale / 2f) * Fix64.One),
                    (long)((Parameters.StageH * scale  / 2f) * Fix64.One)
                );

                Graphics2D.RenderWithTransform(g, e.Vertices().ToList(),
                    black.ToUInt32(),
                    Matrix.NewScale(2992558336) * origin * canvasCorner);
            }
        }

        private static void renderSkeleton(Graphics2D g, Parameters parameters, Matrix canvasCorner, int scale)
		{
            Trace.TraceInformation("Composing skeleton");

            // const int64 radius = (long) ((3 / 2f) * Fix64V1.ONE);
            int64 radius = 6442450944 * scale;

            for (var i = 0; i < parameters.pathSegments; i++)
			{
				for (var j = 0; j < parameters.numPaths; j++)
                {
                    int64 x = parameters.paths[j].mx(Fix64.Div(i * Fix64.One, parameters.pathSegments * Fix64.One)) * scale;
                    int64 y = parameters.paths[j].my(Fix64.Div(i * Fix64.One, parameters.pathSegments * Fix64.One)) * scale;

                    var color = Color.FromArgb(55, 0, 0, 0);

                    var origin = Matrix.NewTranslation(
                        (long)((Parameters.StageW * scale / 2f) * Fix64.One),
                        (long)((Parameters.StageH * scale / 2f) * Fix64.One)
                    );

                    Graphics2D.RenderWithTransform(g, new Ellipse(
                            x,
                            y,
                            radius, 
                            radius).Vertices().ToList(),
                        color.ToUInt32(),
                        Matrix.NewScale((long) (MasterScale * Fix64.One)) * origin * canvasCorner
                        );
                }
			}
		}

        private static void renderUniverse(Graphics2D g, Parameters parameters, Matrix canvasCorner, int seed, int scale)
        {
            Trace.TraceInformation("Composing universe");

            var counts = new Dictionary<int, int>();

            var sw = Stopwatch.StartNew();

            int debugCount = 0;

            // int64 reduceAmount = Fix64V1.div(Fix64V1.ONE, parameters.pathSegments * Fix64V1.ONE);
            int64 reduceAmount = 2147483 /* 0.0005 */;
            
            var origin = Matrix.NewTranslation(
                (long)((Parameters.StageW * scale / 2f) * Fix64.One),
                (long)((Parameters.StageH * scale / 2f) * Fix64.One)
            );

            for (var i = 0; i < parameters.pathSegments * parameters.numPaths; i++)
            {
                var outer = i / parameters.numPaths; // 0 .. 2000
                var inner = i % parameters.numPaths; // 0 .. 28

                int64 x = parameters.paths[inner].mx(Fix64.Div(outer * Fix64.One, parameters.pathSegments * Fix64.One)) * scale;
                int64 y = parameters.paths[inner].my(Fix64.Div(outer * Fix64.One, parameters.pathSegments * Fix64.One)) * scale;

                int64 angle;
                if (parameters.whichRot[inner] == 0)
                {
                    if (parameters.whichRotDir[inner] == 0)
                    {
                        angle = radians(Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */));
                    }
                    else
                    {
                        angle = radians(-Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */));
                    }
                }
                else if (parameters.whichRot[inner] == 1)
                {
                    if (parameters.whichRotDir[inner] == 0)
                    {
                        angle = radians(Fix64.Sub(360 * Fix64.One,
                            Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))));
                    }
                    else
                    {
                        angle = radians(-Fix64.Sub(360 * Fix64.One, Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))));
                    }
                }
                else
                {
                    angle = 0;
                }

                {
                    int64 radius = 6442450944 /* 1.5 */ * scale;
                    var color = Color.FromArgb(55, 0, 0, 0);

                    Graphics2D.RenderWithTransform(g, new Ellipse(
                            x,
                            -y,
                            radius,
                            radius).Vertices().ToList(),

                        color.ToUInt32(),
                        Matrix.NewScale(MasterScaleF) *
                        origin * canvasCorner
                    );
                }

                int64 t1 = Fix64.Mul(Fix64.Mul(parameters.cLen * Fix64.One, reduceAmount), outer * Fix64.One);
                int64 t2 = Fix64.Sub(parameters.cLen * Fix64.One, t1);
                int64 colorChoice = Fix64.Floor(t2 % (parameters.cLen * Fix64.One));

                if (parameters.whichColorFlow[inner] != 0)
                {
                    if (parameters.whichColorFlow[inner] == 1)
                    {
                        colorChoice = Fix64.Floor(
                            Fix64.Add(Fix64.Mul(outer * Fix64.One, Fix64.Two), inner * Fix64.One)
                            % (parameters.cLen * Fix64.One)
                            );
                    }
                    else if (parameters.whichColorFlow[inner] == 2)
                    {
                        colorChoice = Fix64.Floor(Fix64.Add(Fix64.Mul(inner * Fix64.One, Fix64.Div(parameters.cLen * Fix64.One, Parameters.GridSize * Fix64.One))
                                                     ,
                                                     Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One), 1288490240 /* 0.3 */))
                                                    % (parameters.cLen * Fix64.One)
                                                    );
                    }
                    else if (parameters.whichColorFlow[inner] == 3)
                    {
                        colorChoice =
                            Fix64.Floor(Fix64.Add(Fix64.Mul(inner * Fix64.One, Fix64.Div(parameters.cLen * Fix64.One, Fix64.Mul(Parameters.GridSize * Fix64.One, 429496736 /* 0.1 */)))
                                           ,
                                           Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One), 429496736 /* 0.1 */))
                                % (parameters.cLen * Fix64.One));
                    }
                }

                var tint = Color.FromArgb(
                    255,
                    parameters.myColorsR[(int)(colorChoice / Fix64.One)],
                    parameters.myColorsG[(int)(colorChoice / Fix64.One)],
                    parameters.myColorsB[(int)(colorChoice / Fix64.One)]);

                int64 size = Fix64.Sub(Parameters.BaseSize * Fix64.One, Fix64.Mul((long)(Parameters.BaseSize * Fix64.One), Fix64.Mul(reduceAmount, outer * Fix64.One))) * scale;
                int64 s = Fix64.Mul(MasterScaleF, Fix64.Div(size, (long)(BaseSize * Fix64.One)));
                int64 dx = Fix64.Mul(x, MasterScaleF);
                int64 dy = Fix64.Mul(-y, MasterScaleF);

                debugCount++;
                var textureIndex = parameters.whichTex[inner];
                var transform = Matrix.Mul(origin, canvasCorner);

                if (textureIndex == 0)
                {
                    Texture0.draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(0, out var value))
                    {
                        counts.Add(0, 1);
                    }
                    else
                    {
                        counts[0] = value + 1;
                    }
                }
                else if (textureIndex == 1)
                {
                    Texture1.draw(g, dx, dy, -angle, s, transform, tint, debugCount);
                    if (!counts.TryGetValue(1, out var value))
                    {
                        counts.Add(1, 1);
                    }
                    else
                    {
                        counts[1] = value + 1;
                    }
                }
                else if (textureIndex == 2)
                {
                    Texture2.draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(2, out var value))
                    {
                        counts.Add(2, 1);
                    }
                    else
                    {
                        counts[2] = value + 1;
                    }
                }
                else if (textureIndex == 3)
                {
                    Texture3.draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(3, out var value))
                    {
                        counts.Add(3, 1);
                    }
                    else
                    {
                        counts[3] = value + 1;
                    }
                }
                else if (textureIndex == 4)
                {
                    Texture4.draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(4, out var value))
                    {
                        counts.Add(4, 1);
                    }
                    else
                    {
                        counts[4] = value + 1;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (debugCount > 0 && debugCount % 1000 == 0)
                {
                    Console.WriteLine($"TUM_{seed}_{debugCount}, elapsed time is {sw.Elapsed}");
                }
            }

            foreach (var (key, value) in counts)
            {
                Console.WriteLine($"texture #{key}: {value}");
            }
        }

        private static void renderStars(Graphics2D g, Parameters p, Matrix canvasCorner, int scale)
        {
            Trace.TraceInformation("Composing stars...");

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < p.starMax; ++i)
            {
                int32 x = p.starPositions[i].X * scale;
                int32 y = p.starPositions[i].Y * scale;
                int32 colorIndex = p.starPositions[i].c % p.cLen;

                int32 tint = 0;
                tint |= 255 << 24;
                tint |= p.myColorsR[colorIndex] << 16;
                tint |= p.myColorsG[colorIndex] << 8;
                tint |= p.myColorsB[colorIndex] << 0;

                int64 size = (long) (p.starPositions[i].S / 1000f * Fix64.One) * scale;
                
                int64 dx = Fix64.Mul(x * Fix64.One, MasterScaleF);
                int64 dy = Fix64.Mul(-y * Fix64.One, MasterScaleF);
                int64 s = Fix64.Div(Fix64.Mul(MasterScaleF, size), Fix64.Two);

                var origin = Matrix.NewTranslation(
                    (long)((Parameters.StageW * scale / 2f) * Fix64.One),
                    (long)((Parameters.StageH * scale / 2f) * Fix64.One)
                );

                Texture5.draw(g, dx, dy, s, origin * canvasCorner, (uint) tint);

                if (i % 10 == 0)
                {
                    Console.WriteLine($"{i} stars drawn, elapsed time is " + sw.Elapsed);
                }
            }

            Console.WriteLine("Drawing stars finished, took " + sw.Elapsed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int64 radians(int64 degree)
        {
            return Fix64.Mul(degree, Fix64.Div(Fix64.Pi, 180 * Fix64.One));
        }
    }
}
