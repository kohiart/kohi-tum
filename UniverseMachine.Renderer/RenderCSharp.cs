// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Numerics;
using Kohi.Composer;

namespace UniverseMachine.Renderer;

internal class RenderCSharp
{
    public static Task<string> ExecuteAsync(string folder, BigInteger token, int seed, int scale = 1)
    {
        var canvasCorner = Matrix.NewTranslation(365072220160 * scale, 365072220160 * scale);
        var artwork = new Artwork(seed);
        var canvas = new Graphics2D(1674, 2400, scale);
        artwork.Draw(canvas, canvasCorner, scale);
        var filename = Path.Combine(folder, $"TUM_CSharp_{token}_{seed}_{scale}X.png");
        ImageData.Save(filename, canvas);
        return Task.FromResult(filename);
    }
}