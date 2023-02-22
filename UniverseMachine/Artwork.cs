// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Kohi.Composer;

namespace UniverseMachine;

public sealed class Artwork
{
    public const long MasterScale = 2992558231; /* 0.6967592592592593 */
    private readonly int _seed;

    public readonly Parameters Parameters;

    public Artwork(int seed, int? whichMasterSet = null, int? colorOverride = null)
    {
        var xorShift = new XorShift(seed);
        _seed = seed;
        Parameters = new Parameters(xorShift, whichMasterSet, colorOverride);
    }

    public void Draw(Graphics2D g, Matrix canvasCorner, int scale, Layer layer = Layer.All)
    {
        var sw = Stopwatch.StartNew();

        if (layer.HasFlagFast(Layer.Background))
            RenderBackground(g, Parameters);
        if (layer.HasFlagFast(Layer.Grid))
            RenderGridDots(g, Parameters, canvasCorner, scale);
        if (layer.HasFlagFast(Layer.Skeleton))
            RenderSkeleton(g, Parameters, canvasCorner, scale);
        if (layer.HasFlagFast(Layer.Universe))
            RenderUniverse(g, Parameters, canvasCorner, _seed, scale);
        if (layer.HasFlagFast(Layer.Stars))
            RenderStars(g, Parameters, canvasCorner, scale);
        if (layer.HasFlagFast(Layer.Mats))
            RenderMats(g, scale);

        Trace.TraceInformation($"Composer completed in {sw.Elapsed}");
    }

    private static void RenderBackground(Graphics2D g, Parameters parameters)
    {
        Trace.TraceInformation("Composing background");

        var c = parameters.StarPositions[0].C % parameters.CLen;

        var background = ColorMath.ToColor(
            255,
            parameters.MyColorsR[c],
            parameters.MyColorsG[c],
            parameters.MyColorsB[c]
        );

        Graphics2D.Clear(g, background);
    }

    private void RenderMats(Graphics2D g, int scale)
    {
        Trace.TraceInformation("Composing mats");

        var edge = 85 * scale;

        RenderMat(g, edge--, GetMatColor(150));

        for (var i = 0; i < 8 * scale; i++) RenderMat(g, edge--, GetMatColor(50));

        for (var i = 0; i < 77 * scale; i++) RenderMat(g, edge--, GetMatColor(0));
    }

    private static void RenderMat(Graphics2D g, int edge, Color color)
    {
        var line = new CustomPath();

        line.MoveTo(edge * Fix64.One, (g.Height - edge) * Fix64.One);
        line.LineTo(edge * Fix64.One, edge * Fix64.One);

        line.MoveTo((edge + 1) * Fix64.One, (edge + 1) * Fix64.One);
        line.LineTo((g.Width - edge) * Fix64.One, (edge + 1) * Fix64.One);

        line.MoveTo((edge + 1) * Fix64.One, (g.Height - edge - 1) * Fix64.One);
        line.LineTo((g.Width - edge) * Fix64.One, (g.Height - edge - 1) * Fix64.One);

        line.MoveTo((g.Width - edge - 1) * Fix64.One, (g.Height - edge - 2) * Fix64.One);
        line.LineTo((g.Width - edge - 1) * Fix64.One, (edge + 2) * Fix64.One);

        var stroke = new Stroke(line.Vertices());
        var vertices = Stroke.Vertices(stroke).ToList();

        Graphics2D.Render(g, vertices, color.ToUInt32(), false);
    }

    private Color GetMatColor(int index)
    {
        var colorIndex = Parameters.StarPositions[0].C;

        var color = Color.FromArgb(
            255,
            Parameters.MyColorsR[(colorIndex + index) % Parameters.CLen],
            Parameters.MyColorsG[(colorIndex + index) % Parameters.CLen],
            Parameters.MyColorsB[(colorIndex + index) % Parameters.CLen]);

        return color;
    }

    private static void RenderGridDots(Graphics2D g, Parameters parameters, Matrix canvasCorner, int scale)
    {
        Trace.TraceInformation("Composing grid");

        var radius = 19327352832 * scale;

        var e = new Ellipse(0, 0, radius, radius);

        for (var i = 0; i < Parameters.GridSize; i++)
        {
            var black = Color.Black;
            e.OriginX = parameters.GridPoints[i].X * scale;
            e.OriginY = parameters.GridPoints[i].Y * scale;

            var origin = Matrix.NewTranslation(
                (long) (Parameters.StageW * scale / 2f * Fix64.One),
                (long) (Parameters.StageH * scale / 2f * Fix64.One)
            );

            Graphics2D.RenderWithTransform(g, e.Vertices().ToList(),
                black.ToUInt32(),
                Matrix.NewScale(2992558336) * origin * canvasCorner);
        }
    }

    private static void RenderSkeleton(Graphics2D g, Parameters parameters, Matrix canvasCorner, int scale)
    {
        Trace.TraceInformation("Composing skeleton");

        var radius = 6442450944 /* 3 / 2 */ * scale;

        for (var i = 0; i < parameters.PathSegments; i++)
        for (var j = 0; j < parameters.NumPaths; j++)
        {
            var x = parameters.Paths[j].Mx(Fix64.Div(i * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;
            var y = parameters.Paths[j].My(Fix64.Div(i * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;

            var color = Color.FromArgb(55, 0, 0, 0);

            var origin = Matrix.NewTranslation(
                (long) (Parameters.StageW * scale / 2f * Fix64.One),
                (long) (Parameters.StageH * scale / 2f * Fix64.One)
            );

            Graphics2D.RenderWithTransform(g, new Ellipse(
                    x,
                    y,
                    radius,
                    radius).Vertices().ToList(),
                color.ToUInt32(),
                Matrix.NewScale(MasterScale) * origin * canvasCorner
            );
        }
    }

    private static void RenderUniverse(Graphics2D g, Parameters parameters, Matrix canvasCorner, int seed, int scale)
    {
        Trace.TraceInformation("Composing universe");

        var counts = new Dictionary<int, int>();

        var sw = Stopwatch.StartNew();

        var debugCount = 0;

        const long reduceAmount = 2147483 /* (1 / pathSegments) = 0.0005 */;

        var origin = Matrix.NewTranslation(
            (long) (Parameters.StageW * scale / 2f * Fix64.One),
            (long) (Parameters.StageH * scale / 2f * Fix64.One)
        );

        for (var i = 0; i < parameters.PathSegments * parameters.NumPaths; i++)
        {
            var outer = i / parameters.NumPaths; // 0 .. 2000
            var inner = i % parameters.NumPaths; // 0 .. 28

            var x = parameters.Paths[inner].Mx(Fix64.Div(outer * Fix64.One, parameters.PathSegments * Fix64.One)) *
                    scale;
            var y = parameters.Paths[inner].My(Fix64.Div(outer * Fix64.One, parameters.PathSegments * Fix64.One)) *
                    scale;

            long angle;
            if (parameters.WhichRot[inner] == 0)
            {
                angle = parameters.WhichRotDir[inner] == 0
                    ? Radians(Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */))
                    : Radians(-Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */));
            }
            else if (parameters.WhichRot[inner] == 1)
            {
                if (parameters.WhichRotDir[inner] == 0)
                    angle = Radians(Fix64.Sub(360 * Fix64.One,
                        Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))));
                else
                    angle = Radians(-Fix64.Sub(360 * Fix64.One,
                        Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))));
            }
            else
            {
                angle = 0;
            }

            {
                var radius = 6442450944 /* 1.5 */ * scale;
                var color = Color.FromArgb(55, 0, 0, 0);

                Graphics2D.RenderWithTransform(g, new Ellipse(
                        x,
                        -y,
                        radius,
                        radius).Vertices().ToList(),
                    color.ToUInt32(),
                    Matrix.NewScale(MasterScale) *
                    origin * canvasCorner
                );
            }

            var t1 = Fix64.Mul(Fix64.Mul(parameters.CLen * Fix64.One, reduceAmount), outer * Fix64.One);
            var t2 = Fix64.Sub(parameters.CLen * Fix64.One, t1);
            var colorChoice = Fix64.Floor(t2 % (parameters.CLen * Fix64.One));

            if (parameters.WhichColorFlow[inner] != 0)
                colorChoice = parameters.WhichColorFlow[inner] switch
                {
                    1 => Fix64.Floor(Fix64.Add(Fix64.Mul(outer * Fix64.One, Fix64.Two), inner * Fix64.One) %
                                     (parameters.CLen * Fix64.One)),
                    2 => Fix64.Floor(
                        Fix64.Add(
                            Fix64.Mul(inner * Fix64.One,
                                Fix64.Div(parameters.CLen * Fix64.One, Parameters.GridSize * Fix64.One)),
                            Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One), 1288490240 /* 0.3 */)) %
                        (parameters.CLen * Fix64.One)),
                    3 => Fix64.Floor(
                        Fix64.Add(
                            Fix64.Mul(inner * Fix64.One,
                                Fix64.Div(parameters.CLen * Fix64.One,
                                    Fix64.Mul(Parameters.GridSize * Fix64.One, 429496736 /* 0.1 */))),
                            Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One), 429496736 /* 0.1 */)) %
                        (parameters.CLen * Fix64.One)),
                    _ => colorChoice
                };

            var tint = Color.FromArgb(
                255,
                parameters.MyColorsR[(int) (colorChoice / Fix64.One)],
                parameters.MyColorsG[(int) (colorChoice / Fix64.One)],
                parameters.MyColorsB[(int) (colorChoice / Fix64.One)]);

            var size = Fix64.Sub(Parameters.BaseSize * Fix64.One, Fix64.Mul(Parameters.BaseSize * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))) * scale;
            var s = Fix64.Mul(MasterScale, Fix64.Div(size, (long) (Parameters.BaseSize * Fix64.One)));
            var dx = Fix64.Mul(x, MasterScale);
            var dy = Fix64.Mul(-y, MasterScale);

            debugCount++;
            var textureIndex = parameters.WhichTex[inner];
            var transform = Matrix.Mul(origin, canvasCorner);

            switch (textureIndex)
            {
                case 0:
                {
                    Texture0.Draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(0, out var value))
                        counts.Add(0, 1);
                    else
                        counts[0] = value + 1;
                    break;
                }
                case 1:
                {
                    Texture1.Draw(g, dx, dy, -angle, s, transform, tint, debugCount);
                    if (!counts.TryGetValue(1, out var value))
                        counts.Add(1, 1);
                    else
                        counts[1] = value + 1;
                    break;
                }
                case 2:
                {
                    Texture2.Draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(2, out var value))
                        counts.Add(2, 1);
                    else
                        counts[2] = value + 1;
                    break;
                }
                case 3:
                {
                    Texture3.Draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(3, out var value))
                        counts.Add(3, 1);
                    else
                        counts[3] = value + 1;
                    break;
                }
                case 4:
                {
                    Texture4.Draw(g, dx, dy, -angle, s, transform, tint);
                    if (!counts.TryGetValue(4, out var value))
                        counts.Add(4, 1);
                    else
                        counts[4] = value + 1;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (debugCount > 0 && debugCount % 1000 == 0)
                Console.WriteLine($"TUM_{seed}_{debugCount}, elapsed time is {sw.Elapsed}");
        }
    }

    private static void RenderStars(Graphics2D g, Parameters p, Matrix canvasCorner, int scale)
    {
        Trace.TraceInformation("Composing stars...");

        for (var i = 0; i < p.StarMax; ++i)
        {
            var x = p.StarPositions[i].X * scale;
            var y = p.StarPositions[i].Y * scale;
            var colorIndex = p.StarPositions[i].C % p.CLen;

            var tint = 0;
            tint |= 255 << 24;
            tint |= p.MyColorsR[colorIndex] << 16;
            tint |= p.MyColorsG[colorIndex] << 8;
            tint |= p.MyColorsB[colorIndex] << 0;

            var size = (long) (p.StarPositions[i].S / 1000f * Fix64.One) * scale;

            var dx = Fix64.Mul(x * Fix64.One, MasterScale);
            var dy = Fix64.Mul(-y * Fix64.One, MasterScale);
            var s = Fix64.Div(Fix64.Mul(MasterScale, size), Fix64.Two);

            var origin = Matrix.NewTranslation(
                (long) (Parameters.StageW * scale / 2f * Fix64.One),
                (long) (Parameters.StageH * scale / 2f * Fix64.One)
            );

            Texture5.Draw(g, dx, dy, s, origin * canvasCorner, (uint) tint);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Radians(long degree)
    {
        return Fix64.Mul(degree, Fix64.Div(Fix64.Pi, 180 * Fix64.One));
    }
}