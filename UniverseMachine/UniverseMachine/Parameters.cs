// Copyright Kohi Art Community, Inc.. All rights reserved.

using Kohi.Composer;
using uint8 = System.Byte;
using int64 = System.Int64;
using int32 = System.Int32;
using uint32 = System.UInt32;

namespace UniverseMachine;

public sealed class Parameters
{
    public const int32 StageW = 1505;
    public const int32 StageH = 2228;
    public const int32 ColorSpread = 150;
    public const int32 GridSize = NumCols * NumRows;
    public const int32 GridMaxMargin = -335;
    public const int32 NumCols = 4;
    public const int32 NumRows = 7;
    public const int32 NumColors = 750;

    public int32 whichMasterSet { get; set; }
    public int32 whichColor { get; set; }
    public int32 endIdx { get; set; }
    public int32 cLen { get; set; }

    public List<uint8> myColorsR { get; set; }
    public List<uint8> myColorsG { get; set; }
    public List<uint8> myColorsB { get; set; }
    public List<int32> whichTex { get; set; }
    public List<int32> whichColorFlow { get; set; }
    public List<int32> whichRot { get; set; }
    public List<int32> whichRotDir { get; set; }
    public List<Vector2> gridPoints { get; set; }
    public Bezier[] paths { get; set; }
    public int32 numPaths { get; set; }
    public List<Star> starPositions { get; set; }
    
    private readonly List<int[]> masterSet = new()
    {
        new [] {1, 5, 4, 2},
        new [] {6, 5, 4, 3},
        new [] {4, 1, 4, 2},
        new [] {4, 1, 0, 2},
        new [] {4, 1, 2, 2},
        new [] {4, 5, 4, 1},
        new [] {3, 5, 3, 0},
        new [] {3, 0, 3, 0},
        new [] {3, 5, 2, 0},
        new [] {3, 2, 2, 0},
        new [] {3, 1, 2, 0},
        new [] {3, 0, 2, 0},
        new [] {2, 4, 4, 1},
        new [] {2, 4, 2, 1},
        new [] {2, 3, 4, 1},
        new [] {2, 3, 0, 1},
        new [] {2, 1, 4, 1},
        new [] {2, 1, 0, 1},
        new [] {2, 1, 4, 2},
        new [] {2, 1, 0, 2},
        new [] {2, 1, 2, 2},
        new [] {2, 0, 4, 1},
        new [] {2, 5, 4, 1},
        new [] {2, 5, 0, 1},
        new [] {2, 5, 4, 2},
        new [] {2, 5, 0, 2},
        new [] {2, 5, 2, 2},
        new [] {1, 4, 0, 1},
        new [] {1, 3, 4, 1},
        new [] {1, 3, 0, 1},
        new [] {1, 3, 2, 1},
        new [] {1, 1, 4, 1},
        new [] {1, 1, 4, 2},
        new [] {1, 1, 0, 2},
        new [] {1, 1, 2, 2},
        new [] {1, 0, 4, 0},
        new [] {1, 0, 4, 1},
        new [] {1, 5, 2, 1},
        new [] {1, 5, 4, 2},
        new [] {1, 5, 0, 2},
        new [] {1, 5, 2, 2},
        new [] {0, 1, 2, 2},
        new [] {0, 5, 4, 2},
        new [] {0, 5, 2, 2},
        new [] {6, 4, 2, 1},
        new [] {6, 3, 4, 1},
        new [] {6, 3, 2, 1},
        new [] {6, 1, 4, 2},
        new [] {6, 1, 0, 2},
        new [] {6, 1, 2, 2},
        new [] {6, 0, 4, 1},
        new [] {6, 0, 0, 1},
        new [] {6, 0, 2, 1},
        new [] {6, 5, 4, 2},
        new [] {6, 5, 0, 2},
        new [] {6, 5, 2, 2}
    };

    private uint[] whichClr;
    private List<uint[]> clrs;
    
    public int pathSegments = 2000;

    public int starMax = 150;
    public const int BaseSize = 2222;
    public const int64 BaseSizeF = 9543417331712 /* 2222 */;
    private int numTextures = 6;
    
    private class Universe
    {
        public List<int> whichBezierPattern = new List<int>();
        public List<int> whichGridPos = new List<int>();
        public List<int> whichBezierH1a = new List<int>();
        public List<int> whichBezierH1b = new List<int>();
        public List<int> whichBezierH2a = new List<int>();
        public List<int> whichBezierH2b = new List<int>();
    }

    public Parameters(XorShift prng, int? masterSetOverride = null, int? colorOverride = null)
    {
        clrs = new List<uint[]>
        {
            new[] { 0xFFA59081, 0xFFF26B8F, 0xFF3C7373, 0xFF7CC4B0, 0xFFF2F2F5 },
            new[] { 0xFFF2F2F5, 0xFF0C2F40, 0xFF335E71, 0xFF71AABF, 0xFFA59081 },
            new[] { 0xFFF35453, 0xFF007074, 0xFFD2D8BE, 0xFFEFCF89, 0xFFF49831 },
            new[] { 0xFF2B5D75, 0xFFF35453, 0xFFF2F2F5, 0xFF5E382C, 0xFFCB7570 },
            new[] { 0xFFF9C169, 0xFF56C4B5, 0xFF214B73, 0xFF16163F, 0xFF9A5E1F },
            new[] { 0xFFFBE5B6, 0xFFF9C169, 0xFF9C7447, 0xFF775D40, 0xFF4A5343 },
            new[] { 0xFFE2EBE1, 0xFFE7D9AD, 0xFF63AA62, 0xFF0C3A3C, 0xFF87C4C2 },
            new[] { 0xFFE8E8E8, 0xFFB9B9B9, 0xFF666666, 0xFF262626, 0xFF65D8E4 },
            new[] { 0xFF466E8B, 0xFFFEF5E7, 0xFFF1795E, 0xFF666073, 0xFF192348 },
            new[] { 0xFFFFFFFF, 0xFF8C8C8C, 0xFF404040, 0xFF8C8C8C, 0xFFF2F2F2 }
        };

        var realMasterSet = prng.nextInt(1 * Fix64.One, 55 * Fix64.One);
        whichMasterSet = masterSetOverride ?? realMasterSet;

        var realColor = prng.nextInt(0 * Fix64.One, 9 * Fix64.One);
        whichColor = colorOverride ?? realColor;

        endIdx = prng.nextInt(0, (GridSize - 1) * Fix64.One);

        buildColors();

        whichTex = new List<int>();
        whichColorFlow = new List<int>();
        whichRot = new List<int>();
        whichRotDir = new List<int>();
        
        var universe = buildUniverse(prng);

        buildGrid();

        buildPaths(universe);

        buildStars(prng, this);
    }

    private Universe buildUniverse(XorShift prng)
    {
        var universe = new Universe();

        for (var i = 0; i < GridSize; i++)
        {
            switch (masterSet[whichMasterSet][0])
            {
                case 0:
                    universe.whichBezierPattern.Add(0);
                    break;
                case 1:
                    universe.whichBezierPattern.Add(1);
                    break;
                case 2:
                    universe.whichBezierPattern.Add(2);
                    break;
                case 3:
                    universe.whichBezierPattern.Add(3);
                    break;
                case 4:
                    universe.whichBezierPattern.Add(4);
                    break;
                case 5:
                    universe.whichBezierPattern.Add(5);
                    break;
                case 6:
                    universe.whichBezierPattern.Add(prng.nextInt(0, 5 * Fix64.One));
                    break;
            }

            switch (masterSet[this.whichMasterSet][1])
            {
                case 0:
                    whichTex.Add(0);
                    break;
                case 1:
                    whichTex.Add(1);
                    break;
                case 2:
                    whichTex.Add(2);
                    break;
                case 3:
                    whichTex.Add(3);
                    break;
                case 4:
                    whichTex.Add(4);
                    break;
                case 5:
                    whichTex.Add(prng.nextInt(0, (numTextures - 2) * Fix64.One));
                    break;
            }

            switch (masterSet[this.whichMasterSet][2])
            {
                case 0:
                    whichColorFlow.Add(0);
                    break;
                case 1:
                    whichColorFlow.Add(1);
                    break;
                case 2:
                    whichColorFlow.Add(2);
                    break;
                case 3:
                    whichColorFlow.Add(3);
                    break;
                case 4:
                    whichColorFlow.Add(prng.nextInt(0, 3 * Fix64.One));
                    break;
            }

            switch (masterSet[this.whichMasterSet][3])
            {
                case 0:
                    whichRot.Add(0);
                    break;
                case 1:
                    whichRot.Add(1);
                    break;
                case 2:
                    whichRot.Add(2);
                    break; // NO ROT
                case 3:
                    whichRot.Add(prng.nextInt(0, 2 * Fix64.One));
                    break;
            }

            whichRotDir.Add(prng.nextInt(0, 1 * Fix64.One));
            universe.whichGridPos.Add(prng.nextInt(0, (GridSize - 1) * Fix64.One));
            universe.whichBezierH1a.Add(prng.nextInt(-(Fix64.Div(StageW * Fix64.One, Fix64.Two)), Fix64.Div(StageW * Fix64.One, Fix64.Two)));
            universe.whichBezierH1b.Add(prng.nextInt(-(StageH * Fix64.One), (StageH * Fix64.One)));
            universe.whichBezierH2a.Add(prng.nextInt(-(Fix64.Div(StageW * Fix64.One, Fix64.Two)), Fix64.Div(StageW * Fix64.One, Fix64.Two)));
            universe.whichBezierH2b.Add(prng.nextInt(-(StageH * Fix64.One), (StageH * Fix64.One)));
        }

        return universe;
    }

    private void buildColors()
    {
        whichClr = clrs[whichColor];

        int64 inter = Fix64.Div(Fix64.One, ColorSpread * Fix64.One);

        myColorsR = new List<uint8>(NumColors);
        myColorsG = new List<uint8>(NumColors);
        myColorsB = new List<uint8>(NumColors);

        for (uint32 i = 0; i < whichClr.Length; i++)
        {
            uint32 j = i == whichClr.Length - 1 ? 0 : i + 1;

            for (uint32 x = 0; x < ColorSpread; x++)
            {
                uint32 c = ColorMath.Lerp(whichClr[i], whichClr[j], Fix64.Mul(inter, x * Fix64.One));
                myColorsR.Add((uint8)(c >> 16));
                myColorsG.Add((uint8)(c >> 8));
                myColorsB.Add((uint8)(c >> 0));
            }
        }

        cLen = myColorsR.Count;

        myColorsR = myColorsR.ToList();
        myColorsG = myColorsG.ToList();
        myColorsB = myColorsB.ToList();
    }

    private void buildGrid()
    {
        gridPoints = new List<Vector2>();

        int64 stageW = StageW * Fix64.One;
        int64 stageH = StageH * Fix64.One;

        int64 ratio = Fix64.Div(NumCols * Fix64.One, NumRows * Fix64.One);
        int64 margin = Fix64.Min(GridMaxMargin * Fix64.One, Fix64.Div(stageW, Fix64.Two));
        
        int64 width = Fix64.Sub(stageW, Fix64.Mul(margin, Fix64.Two));
        int64 height = Fix64.Div(width, ratio);

        if (height > Fix64.Sub(stageH, Fix64.Mul(margin, Fix64.Two)))
        {
            height = Fix64.Sub(stageH, Fix64.Mul(margin, Fix64.Two));
            width = Fix64.Mul(height, ratio);
        }

        int64 gridStartX = Fix64.Div(-width, Fix64.Two);
        int64 gridStartY = Fix64.Div(-height, Fix64.Two);
        int64 gridSpaceX = Fix64.Div(width, Fix64.Sub(NumCols * Fix64.One, Fix64.One));
        int64 gridSpaceY = Fix64.Div(height, Fix64.Sub(NumRows * Fix64.One, Fix64.One));

        for (var i = 0; i < GridSize; i++)
        {
            int32 col = i % NumCols;
            int64 row = Fix64.Floor(Fix64.Div(i * Fix64.One, NumCols * Fix64.One));
            int64 x = Fix64.Add(gridStartX, Fix64.Mul(col * Fix64.One, gridSpaceX));
            int64 y = Fix64.Add(gridStartY, Fix64.Mul(row, gridSpaceY));

            gridPoints.Add(new Vector2(x, y));
        }
    }


    private void buildPaths(Universe universe)
    {
        paths = new Bezier[GridSize];

        numPaths = 0;

        for (var i = 0; i < GridSize; i++)
        {
            var p1 = new Vector2 { X = gridPoints[i].X, Y = gridPoints[i].Y };
            var p2 = p1;
            var p3 = new Vector2 { X = gridPoints[endIdx].X, Y = gridPoints[endIdx].Y };
            var p4 = p3;

            switch (universe.whichBezierPattern[i])
            {
                case 0: break;
                case 1:
                    p3 = p4 = new Vector2 { X = gridPoints[universe.whichGridPos[i]].X, Y = gridPoints[universe.whichGridPos[i]].Y};
                    break;
                case 2:
                    p3 = new Vector2 { X = universe.whichBezierH1a[i] * Fix64.One, Y = universe.whichBezierH1b[i] * Fix64.One };
                    p4 = new Vector2 { X = gridPoints[universe.whichGridPos[i]].X, Y = gridPoints[universe.whichGridPos[i]].Y};
                    break;
                case 3:
                    p3 = p4 = new Vector2 { X = gridPoints[i].X, Y = gridPoints[i].Y };
                    break;
                case 4:
                    p2 = new Vector2 { X = universe.whichBezierH1a[i] * Fix64.One, Y = universe.whichBezierH1b[i] * Fix64.One };
                    p3 = new Vector2 { X = universe.whichBezierH2a[i] * Fix64.One, Y = universe.whichBezierH2b[i] * Fix64.One };
                    p4 = new Vector2 { X = gridPoints[universe.whichGridPos[i]].X, Y = gridPoints[universe.whichGridPos[i]].Y};
                    break;
                case 5:
                    p2 = new Vector2 { X = universe.whichBezierH1a[i] * Fix64.One, Y = universe.whichBezierH1b[i] * Fix64.One };
                    p3 = new Vector2 { X = universe.whichBezierH2a[i] * Fix64.One, Y = universe.whichBezierH2b[i] * Fix64.One };
                    break;
            }

            paths[numPaths++] = new Bezier(p1, p2, p3, p4);
        }
    }

    public static void buildStars(XorShift prng, Parameters parameters)
    {
        parameters.starPositions = new List<Star>();

        for (var i = 0; i < parameters.starMax; ++i)
        {
            var x = prng.nextInt(
                Fix64.Mul(parameters.gridPoints[0].X, 5368709120L /* 1.25 */),
                Fix64.Mul(parameters.gridPoints[GridSize - 1].X, 5368709120L)
            );

            var y = prng.nextInt(
                Fix64.Mul(parameters.gridPoints[0].Y, 4724464128L /* 1.1 */),
                Fix64.Mul(parameters.gridPoints[GridSize - 1].Y, 4724464128L)
            );

            var sTemp = prng.nextInt(1 * Fix64.One, 3 * Fix64.One);
            var s = (sTemp == 1) ? 1000 : (sTemp == 2) ? 2000 : 3000;
            var c = prng.nextInt(0, (parameters.cLen - 1) * Fix64.One);
            parameters.starPositions.Add(new Star { X = x, Y = y, S = s, c = c });
        }
    }
}