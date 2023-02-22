// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Kohi.Composer;
using int64 = System.Int64;
using int32 = System.Int32;

namespace UniverseMachine;

public sealed class Artwork
{
    public const long MasterScale = 2992558336; /* 0.6967592592592593 */

    /// <summary>
    /// This is a quirk in the history of the project.
    /// The Solidity rendering used a different precision when plotting bezier curves, but scaled with the correct value.
    /// </summary>
    public const long MasterScaleBezier = 2992558231; /* 0.6967592592592593 */

    public const float BaseSize = 2222f;
    private readonly int _seed;

    public readonly Parameters Parameters;

    public Artwork(int seed, int? whichMasterSet = null, int? colorOverride = null)
    {
        var xorShift = new XorShift(seed);
        _seed = seed;
        Parameters = new Parameters(xorShift, whichMasterSet, colorOverride);
    }

    public void Draw(Graphics2D g, int scale)
    {
        RenderBackground(g, Parameters);
        RenderGridDots(g, Parameters, scale);
        RenderSkeleton(g, Parameters, scale);
        RenderUniverse(g, Parameters,  _seed, scale);
        RenderStars(g, Parameters, scale);
        RenderMats(g, scale);
    }

    private static void RenderBackground(Graphics2D g, Parameters parameters)
    {
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

    private static void RenderGridDots(Graphics2D g, Parameters parameters, int scale)
    {
        var radius = 19327352832 /* 4.5 */ * scale;

        var e = new Ellipse(0, 0, radius, radius);

        for (var i = 0; i < Parameters.GridSize; i++)
        {
            var black = Color.Black;
            e.OriginX = parameters.GridPoints[i].X * scale;
            e.OriginY = parameters.GridPoints[i].Y * scale;

            Graphics2D.RenderWithTransform(g, e.Vertices().ToList(),
                black.ToUInt32(),
                CreateScaledTranslation(scale)
                );
        }
    }

    private static void RenderSkeleton(Graphics2D g, Parameters parameters, int scale)
    {
        var radius = 6442450944 /* 1.5 */ * scale;

        for (var i = 0; i < parameters.PathSegments; i++)
        for (var j = 0; j < parameters.NumPaths; j++)
        {
            var x = parameters.Paths[j].Mx(Fix64.Div(i * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;
            var y = parameters.Paths[j].My(Fix64.Div(i * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;

            var color = Color.FromArgb(55, 0, 0, 0);

            Graphics2D.RenderWithTransform(g, new Ellipse(
                    x,
                    y,
                    radius,
                    radius).Vertices().ToList(),
                color.ToUInt32(),
                CreateScaledTranslation(scale)
            );
        }
    }

    private static void RenderUniverse(Graphics2D g, Parameters parameters, int seed, int scale)
    {
        Console.WriteLine("Rendering universe...");

        var counts = new Dictionary<int, int>();
        var sw = Stopwatch.StartNew();
        var debugCount = 0;

        const long reduceAmount = 2147483 /* 0.0005 */;

        for (var i = 0; i < parameters.PathSegments * parameters.NumPaths; i++)
        {
            var outer = i / parameters.NumPaths; // 0 .. 2000
            var inner = i % parameters.NumPaths; // 0 .. 28

            var x = parameters.Paths[inner].Mx(Fix64.Div(outer * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;
            var y = parameters.Paths[inner].My(Fix64.Div(outer * Fix64.One, parameters.PathSegments * Fix64.One)) * scale;

            long angle;
            switch (parameters.WhichRot[inner])
            {
                case 0:
                    angle = parameters.WhichRotDir[inner] == 0 ? Radians(Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */)) : Radians(-Fix64.Mul(outer * Fix64.One, 2147483648 /* 0.5 */));
                    break;
                case 1:
                    angle = parameters.WhichRotDir[inner] == 0
                        ? Radians(Fix64.Sub(360 * Fix64.One,
                            Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))))
                        : Radians(-Fix64.Sub(360 * Fix64.One,
                            Fix64.Mul(360 * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))));
                    break;
                default:
                    angle = 0;
                    break;
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
                    CreateScaledTranslation(scale)
                );
            }

            var t1 = Fix64.Mul(Fix64.Mul(parameters.CLen * Fix64.One, reduceAmount), outer * Fix64.One);
            var t2 = Fix64.Sub(parameters.CLen * Fix64.One, t1);
            var colorChoice = Fix64.Floor(t2 % (parameters.CLen * Fix64.One));

            if (parameters.WhichColorFlow[inner] != 0)
            {
                if (parameters.WhichColorFlow[inner] == 1)
                    colorChoice = Fix64.Floor(
                        Fix64.Add(Fix64.Mul(outer * Fix64.One, Fix64.Two), inner * Fix64.One)
                        % (parameters.CLen * Fix64.One)
                    );
                else if (parameters.WhichColorFlow[inner] == 2)
                    colorChoice = Fix64.Floor(Fix64.Add(Fix64.Mul(inner * Fix64.One,
                                                      Fix64.Div(parameters.CLen * Fix64.One,
                                                          Parameters.GridSize * Fix64.One))
                                                  ,
                                                  Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One),
                                                      1288490240 /* 0.3 */))
                                              % (parameters.CLen * Fix64.One)
                    );
                else if (parameters.WhichColorFlow[inner] == 3)
                    colorChoice =
                        Fix64.Floor(Fix64.Add(Fix64.Mul(inner * Fix64.One,
                                            Fix64.Div(parameters.CLen * Fix64.One,
                                                Fix64.Mul(Parameters.GridSize * Fix64.One, 429496736 /* 0.1 */)))
                                        ,
                                        Fix64.Mul(Fix64.Add(outer * Fix64.One, inner * Fix64.One), 429496736 /* 0.1 */))
                                    % (parameters.CLen * Fix64.One));
            }

            var tint = Color.FromArgb(
                255,
                parameters.MyColorsR[(int) (colorChoice / Fix64.One)],
                parameters.MyColorsG[(int) (colorChoice / Fix64.One)],
                parameters.MyColorsB[(int) (colorChoice / Fix64.One)]);

            var size = Fix64.Sub(Parameters.BaseSize * Fix64.One, Fix64.Mul(Parameters.BaseSize * Fix64.One, Fix64.Mul(reduceAmount, outer * Fix64.One))) * scale;
            var s = Fix64.Mul(MasterScaleBezier, Fix64.Div(size, (long) (BaseSize * Fix64.One)));
            var dx = Fix64.Mul(x, MasterScaleBezier);
            var dy = Fix64.Mul(-y, MasterScaleBezier);

            debugCount++;
            var textureIndex = parameters.WhichTex[inner];

            var transform = CreateTranslation(scale);

            if (textureIndex == 0)
            {
                Texture0.Draw(g, dx, dy, -angle, s, transform, tint);
                if (!counts.TryGetValue(0, out var value))
                    counts.Add(0, 1);
                else
                    counts[0] = value + 1;
            }
            else if (textureIndex == 1)
            {
                Texture1.Draw(g, dx, dy, -angle, s, transform, tint, debugCount);
                if (!counts.TryGetValue(1, out var value))
                    counts.Add(1, 1);
                else
                    counts[1] = value + 1;
            }
            else if (textureIndex == 2)
            {
                Texture2.Draw(g, dx, dy, -angle, s, transform, tint);
                if (!counts.TryGetValue(2, out var value))
                    counts.Add(2, 1);
                else
                    counts[2] = value + 1;
            }
            else if (textureIndex == 3)
            {
                Texture3.Draw(g, dx, dy, -angle, s, transform, tint);
                if (!counts.TryGetValue(3, out var value))
                    counts.Add(3, 1);
                else
                    counts[3] = value + 1;
            }
            else if (textureIndex == 4)
            {
                Texture4.Draw(g, dx, dy, -angle, s, transform, tint);
                if (!counts.TryGetValue(4, out var value))
                    counts.Add(4, 1);
                else
                    counts[4] = value + 1;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            if (debugCount > 0 && debugCount % 1000 == 0)
                Console.WriteLine($"TUM_{seed}_{debugCount}, elapsed time is {sw.Elapsed}");
        }

        foreach (var (key, value) in counts) Console.WriteLine($"texture #{key}: {value}");
    }

    private static Matrix CreateTranslation(int scale) => Matrix.Mul(GetOriginScaled(scale), GetCanvasCorner(scale));
    private static Matrix CreateScaledTranslation(int scale) => Matrix.Mul(Matrix.Mul(Matrix.NewScale(MasterScale), GetOriginScaled(scale)), GetCanvasCorner(scale));
    private static Matrix GetCanvasCorner(int scale) => Matrix.NewTranslation(365072220160 * scale, 365072220160 * scale);
    private static Matrix GetOriginScaled(int scale) => Matrix.NewTranslation((long) (Parameters.StageW * scale / 2f * Fix64.One), (long) (Parameters.StageH * scale / 2f * Fix64.One));

    private static void RenderStars(Graphics2D g, Parameters p, int scale)
    {
        Console.WriteLine("Drawing stars...");

        var sw = Stopwatch.StartNew();

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

            Texture5.Draw(g, dx, dy, s, CreateTranslation(scale), (uint) tint);

            if (i % 10 == 0) Console.WriteLine($"{i} stars drawn, elapsed time is {sw.Elapsed}");
        }

        Console.WriteLine($"Drawing stars finished, took {sw.Elapsed}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Radians(long degree)
    {
        return Fix64.Mul(degree, Fix64.Div(Fix64.Pi, 180 * Fix64.One));
    }
}