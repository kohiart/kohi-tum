// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using System.Diagnostics;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace UniverseMachine.Renderer
{
    internal static class RenderService
    {
        public static async Task RenderAsync(string folder, int token, int seed)
        {
            Directory.CreateDirectory(folder);
            await Console.Out.WriteLineAsync($"Rendering token #{token} with seed {seed}...");
            var expected = await RenderCSharpAsync(folder, token, seed);
            var actual = await RenderEthereumAsync(folder, token, seed);
            await ValidateAsync(expected, actual, token, seed);
            await Console.Out.WriteLineAsync("Render complete");
        }

        public static async Task<string> RenderCSharpAsync(string folder, BigInteger token, int seed)
        {
            Stopwatch sw;
            string outputPath;
            await Console.Out.WriteLineAsync("Starting C# render...");
            {
                sw = Stopwatch.StartNew();
                outputPath = await RenderCSharp.ExecuteAsync(folder, token, seed);
            }
            await Console.Out.WriteLineAsync($"C# render complete, took {sw.Elapsed}");
            return outputPath;
        }

        public static async Task<string> RenderEthereumAsync(string folder, BigInteger token, int seed)
        {
            Stopwatch stopwatch;
            string outputPath;
            await Console.Out.WriteLineAsync("Starting Ethereum render...");
            {
                stopwatch = Stopwatch.StartNew();
                outputPath = await RenderEthereum.ExecuteAsync("manifest-mochi.json", folder, token, seed);
            }
            await Console.Out.WriteLineAsync($"Ethereum render complete, took {stopwatch.Elapsed}");
            return outputPath;
        }

        public static async Task ValidateAsync(string expected, string actual, int token, int seed)
        {
            await Console.Out.WriteLineAsync("Comparing images...");

            var leftImage = Image.Load<Rgba32>(expected);
            var rightImage = Image.Load<Rgba32>(actual);

            var diffImage = rightImage.Clone();
            diffImage.Mutate(x => x.Lightness(1.7f).Grayscale());

            var diffs = 0;
            for (var y = 0; y < leftImage.Height; y++)
                for (var x = 0; x < leftImage.Width; x++)
                {
                    if (leftImage[x, y] == rightImage[x, y])
                        continue;

                    diffImage[x, y] = Color.Red;
                    diffs++;
                    await Console.Error.WriteLineAsync($"Found difference at ({x}, {y})");
                }

            var diffPath = $"TUM_Diff_{token}_{seed}.png";
            if (diffs > 0)
            {
                await diffImage.SaveAsync(diffPath);
                await Console.Error.WriteLineAsync($"Images don't match, {diffs} differences found. Check delta image.");
            }
            else
            {
                await Console.Out.WriteLineAsync("Images are identical!");
                if (File.Exists(diffPath))
                    File.Delete(diffPath);
            }
        }
    }
}
