using Kohi.Composer;
using UniverseMachine;

int scale = 1;
int seed = 13371337;
int? masterSetOverride = null;
var canvasCorner = Matrix.NewTranslation(365072220160 * scale, 365072220160 * scale);
var artwork = new Artwork(seed, masterSetOverride);
var canvas = new Graphics2D(1674, 2400, scale);
artwork.RenderCanvas(canvas, canvasCorner, scale);
ImageData.Save($"TUM_{masterSetOverride}_{seed}_{scale}X.png", canvas);