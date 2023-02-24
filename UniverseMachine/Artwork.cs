// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Kohi.Composer;

namespace UniverseMachine;

public sealed class Artwork
{
    public const long MasterScale = 2992558336; /* 0.6967592592592593 */

    /// <summary>
    /// This is a quirk in the history of the project.
    /// The Solidity rendering used a different precision when plotting bezier curves, but scaled with the correct value.
    /// </summary>
    public const long MasterScaleBezier = 2992558231; /* 0.6967592592592593 */    
    public const long ReduceAmount = 2147483 /* 0.0005 */;
    public const long BaseSize = 9543417331712 /* 2222 */;
    
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

            Graphics2D.RenderWithTransform(g, new Ellipse(
                    x,
                    y,
                    radius,
                    radius).Vertices().ToList(),
                922746880,
                CreateScaledTranslation(scale)
            );
        }
    }

    private static void RenderUniverse(Graphics2D g, Parameters p, int seed, int scale)
    {
        Console.WriteLine("Rendering universe...");

        var counts = new Dictionary<int, int>();
        var sw = Stopwatch.StartNew();
        var debugCount = 0;

        for (var i = 0; i < p.PathSegments * p.NumPaths; i++)
        {
            var outer = i / p.NumPaths; // 0 .. 2000
            var inner = i % p.NumPaths; // 0 .. 28

            var step = Fix64.Div(outer * Fix64.One, 2000 * Fix64.One);
            var x = p.Paths[inner].Mx(step) * scale;
            var y = p.Paths[inner].My(step) * scale;
            var angle = GetAngle(p, outer, inner);

            {
                var radius = 6442450944 /* 1.5 */ * scale;
                                
                Graphics2D.RenderWithTransform(g, new Ellipse(
                        x,
                        -y,
                        radius,
                        radius).Vertices().ToList(),
                    922746880,
                    CreateScaledTranslation(scale)
                );
            }

            var colorChoice = GetColorChoice(p, outer, inner);

            var tint = ColorMath.ToColor(
                    255,
                    p.MyColorsR[(int)(colorChoice / Fix64.One)],
                    p.MyColorsG[(int)(colorChoice / Fix64.One)],
                    p.MyColorsB[(int)(colorChoice / Fix64.One)]
                );

            var size = Fix64.Sub(BaseSize, Fix64.Mul(BaseSize, Fix64.Mul(ReduceAmount, outer * Fix64.One))) * scale;
            var s = Fix64.Mul(MasterScaleBezier, Fix64.Div(size, BaseSize));
            var dx = Fix64.Mul(x, MasterScaleBezier);
            var dy = Fix64.Mul(-y, MasterScaleBezier);

            debugCount++;
            var textureIndex = p.WhichTex[inner];

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

    private static long GetAngle(Parameters p, int i, int j)
    {
        long angle;
        
        if (p.WhichRot[j] == 0) {
            if (p.WhichRotDir[j] == 0) {
                angle = Radians(
                    Fix64.Mul(
                        i * Fix64.One,
                        2147483648 /* 0.5 */
                    )
                );
            } else {
                angle = Radians(
                    -Fix64.Mul(
                        i * Fix64.One,
                        2147483648 /* 0.5 */
                    )
                );
            }
        } else if (p.WhichRot[j] == 1) {
            if (p.WhichRotDir[j] == 0) {
                angle = Radians(
                    Fix64.Sub(
                        360 * Fix64.One,
                        Fix64.Mul(
                            360 * Fix64.One,
                            Fix64.Mul(
                                ReduceAmount,
                                i * Fix64.One
                            )
                        )
                    )
                );
            } else {
                angle = Radians(
                    -Fix64.Sub(
                        360 * Fix64.One,
                        Fix64.Mul(
                            360 * Fix64.One,
                            Fix64.Mul(
                                2147483, /* 0.0005 */
                                i * Fix64.One
                            )
                        )
                    )
                );
            }
        } else {
            angle = 0;
        }

        return angle;
    }

    private static long GetColorChoice(Parameters p, int i, int j)
    {
        var colorChoice = Fix64.Floor(
            Fix64.Sub(
                p.CLen * Fix64.One,
                Fix64.Mul(
                    Fix64.Mul(
                        p.CLen * Fix64.One,
                        ReduceAmount
                    ),
                    i * Fix64.One
                )
            ) % (p.CLen * Fix64.One)
        );
        if (p.WhichColorFlow[j] != 0) {
            if (p.WhichColorFlow[j] == 1) {
                colorChoice = Fix64.Floor(
                    Fix64.Add(
                        Fix64.Mul((i) * Fix64.One, Fix64.Two),
                        (j) * Fix64.One
                    ) % (p.CLen * Fix64.One)
                );
            } else if (p.WhichColorFlow[j] == 2) {
                colorChoice = Fix64.Floor(
                    Fix64.Add(
                        Fix64.Mul(
                            (j) * Fix64.One,
                            Fix64.Div(
                                p.CLen * Fix64.One,
                                (28) * Fix64.One
                            )
                        ),
                        Fix64.Mul(
                            Fix64.Add(
                                (i) * Fix64.One,
                                (j) * Fix64.One
                            ),
                            1288490240 /* 0.3 */
                        )
                    ) % (p.CLen * Fix64.One)
                );
            } else if (p.WhichColorFlow[j] == 3) {
                colorChoice = Fix64.Floor(
                    Fix64.Add(
                        Fix64.Mul(
                            (j) * Fix64.One,
                            Fix64.Div(
                                p.CLen * Fix64.One,
                                Fix64.Mul(
                                    28 * Fix64.One,
                                    429496736 /* 0.1 */
                                )
                            )
                        ),
                        Fix64.Mul(
                            Fix64.Add(
                                (i) * Fix64.One,
                                (j) * Fix64.One
                            ),
                            429496736 /* 0.1 */
                        )
                    ) % (p.CLen * Fix64.One)
                );
            }
        }
        return colorChoice;
    }

    private static Matrix CreateTranslation(int scale) => Matrix.Mul(GetOriginScaled(scale), GetCanvasCorner(scale));
    private static Matrix CreateScaledTranslation(int scale) => Matrix.Mul(Matrix.Mul(Matrix.NewScale(MasterScale), GetOriginScaled(scale)), GetCanvasCorner(scale));
    private static Matrix GetCanvasCorner(int scale) => Matrix.NewTranslation(Fix64.Mul(365072220160, scale * Fix64.One), Fix64.Mul(365072220160, scale * Fix64.One));
    private static Matrix GetOriginScaled(int scale) => Matrix.NewTranslation(Fix64.Mul(3231962890240, scale * Fix64.One), Fix64.Mul(4784593567744, scale * Fix64.One));

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