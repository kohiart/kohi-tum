using UniverseMachine.Renderer;

const string outputDir = "renders";

await CommandLine.MastheadAsync();

var token = int.Parse(args[0]);
var seeds = Manifest.GetSeeds("manifest-tum.txt");
var seed = seeds[token];

await RenderService.RenderAsync(outputDir, token, seed);