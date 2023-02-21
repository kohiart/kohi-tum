// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;

namespace UniverseMachine;

public sealed class Parameters
{
    public const int StageW = 1505;
    public const int StageH = 2228;
    public const int ColorSpread = 150;
    public const int GridSize = NumCols * NumRows;
    public const int GridMaxMargin = -335;
    public const int NumCols = 4;
    public const int NumRows = 7;
    public const int NumColors = 750;
    public const int BaseSize = 2222;
    public const long BaseSizeF = 9543417331712 /* 2222 */;

    private readonly List<uint[]> _clrs;

    private readonly List<int[]> _masterSet = new()
    {
        new[] {1, 5, 4, 2},
        new[] {6, 5, 4, 3},
        new[] {4, 1, 4, 2},
        new[] {4, 1, 0, 2},
        new[] {4, 1, 2, 2},
        new[] {4, 5, 4, 1},
        new[] {3, 5, 3, 0},
        new[] {3, 0, 3, 0},
        new[] {3, 5, 2, 0},
        new[] {3, 2, 2, 0},
        new[] {3, 1, 2, 0},
        new[] {3, 0, 2, 0},
        new[] {2, 4, 4, 1},
        new[] {2, 4, 2, 1},
        new[] {2, 3, 4, 1},
        new[] {2, 3, 0, 1},
        new[] {2, 1, 4, 1},
        new[] {2, 1, 0, 1},
        new[] {2, 1, 4, 2},
        new[] {2, 1, 0, 2},
        new[] {2, 1, 2, 2},
        new[] {2, 0, 4, 1},
        new[] {2, 5, 4, 1},
        new[] {2, 5, 0, 1},
        new[] {2, 5, 4, 2},
        new[] {2, 5, 0, 2},
        new[] {2, 5, 2, 2},
        new[] {1, 4, 0, 1},
        new[] {1, 3, 4, 1},
        new[] {1, 3, 0, 1},
        new[] {1, 3, 2, 1},
        new[] {1, 1, 4, 1},
        new[] {1, 1, 4, 2},
        new[] {1, 1, 0, 2},
        new[] {1, 1, 2, 2},
        new[] {1, 0, 4, 0},
        new[] {1, 0, 4, 1},
        new[] {1, 5, 2, 1},
        new[] {1, 5, 4, 2},
        new[] {1, 5, 0, 2},
        new[] {1, 5, 2, 2},
        new[] {0, 1, 2, 2},
        new[] {0, 5, 4, 2},
        new[] {0, 5, 2, 2},
        new[] {6, 4, 2, 1},
        new[] {6, 3, 4, 1},
        new[] {6, 3, 2, 1},
        new[] {6, 1, 4, 2},
        new[] {6, 1, 0, 2},
        new[] {6, 1, 2, 2},
        new[] {6, 0, 4, 1},
        new[] {6, 0, 0, 1},
        new[] {6, 0, 2, 1},
        new[] {6, 5, 4, 2},
        new[] {6, 5, 0, 2},
        new[] {6, 5, 2, 2}
    };

    private readonly int _numTextures = 6;

    private uint[] _whichClr;

    public int PathSegments = 2000;

    public int StarMax = 150;

    public Parameters(XorShift prng, int? masterSetOverride = null, int? colorOverride = null)
    {
        _clrs = new List<uint[]>
        {
            new[] {0xFFA59081, 0xFFF26B8F, 0xFF3C7373, 0xFF7CC4B0, 0xFFF2F2F5},
            new[] {0xFFF2F2F5, 0xFF0C2F40, 0xFF335E71, 0xFF71AABF, 0xFFA59081},
            new[] {0xFFF35453, 0xFF007074, 0xFFD2D8BE, 0xFFEFCF89, 0xFFF49831},
            new[] {0xFF2B5D75, 0xFFF35453, 0xFFF2F2F5, 0xFF5E382C, 0xFFCB7570},
            new[] {0xFFF9C169, 0xFF56C4B5, 0xFF214B73, 0xFF16163F, 0xFF9A5E1F},
            new[] {0xFFFBE5B6, 0xFFF9C169, 0xFF9C7447, 0xFF775D40, 0xFF4A5343},
            new[] {0xFFE2EBE1, 0xFFE7D9AD, 0xFF63AA62, 0xFF0C3A3C, 0xFF87C4C2},
            new[] {0xFFE8E8E8, 0xFFB9B9B9, 0xFF666666, 0xFF262626, 0xFF65D8E4},
            new[] {0xFF466E8B, 0xFFFEF5E7, 0xFFF1795E, 0xFF666073, 0xFF192348},
            new[] {0xFFFFFFFF, 0xFF8C8C8C, 0xFF404040, 0xFF8C8C8C, 0xFFF2F2F2}
        };

        var realMasterSet = prng.NextInt(1 * Fix64.One, 55 * Fix64.One);
        WhichMasterSet = masterSetOverride ?? realMasterSet;

        var realColor = prng.NextInt(0 * Fix64.One, 9 * Fix64.One);
        WhichColor = colorOverride ?? realColor;

        EndIdx = prng.NextInt(0, (GridSize - 1) * Fix64.One);

        BuildColors();

        WhichTex = new List<int>();
        WhichColorFlow = new List<int>();
        WhichRot = new List<int>();
        WhichRotDir = new List<int>();

        var universe = BuildUniverse(prng);

        BuildGrid();

        BuildPaths(universe);

        BuildStars(prng, this);
    }

    public int WhichMasterSet { get; set; }
    public int WhichColor { get; set; }
    public int EndIdx { get; set; }
    public int CLen { get; set; }

    public List<byte> MyColorsR { get; set; }
    public List<byte> MyColorsG { get; set; }
    public List<byte> MyColorsB { get; set; }
    public List<int> WhichTex { get; set; }
    public List<int> WhichColorFlow { get; set; }
    public List<int> WhichRot { get; set; }
    public List<int> WhichRotDir { get; set; }
    public List<Vector2> GridPoints { get; set; }
    public Bezier[] Paths { get; set; }
    public int NumPaths { get; set; }
    public List<Star> StarPositions { get; set; }

    private Universe BuildUniverse(XorShift prng)
    {
        var universe = new Universe();

        for (var i = 0; i < GridSize; i++)
        {
            switch (_masterSet[WhichMasterSet][0])
            {
                case 0:
                    universe.WhichBezierPattern.Add(0);
                    break;
                case 1:
                    universe.WhichBezierPattern.Add(1);
                    break;
                case 2:
                    universe.WhichBezierPattern.Add(2);
                    break;
                case 3:
                    universe.WhichBezierPattern.Add(3);
                    break;
                case 4:
                    universe.WhichBezierPattern.Add(4);
                    break;
                case 5:
                    universe.WhichBezierPattern.Add(5);
                    break;
                case 6:
                    universe.WhichBezierPattern.Add(prng.NextInt(0, 5 * Fix64.One));
                    break;
            }

            switch (_masterSet[WhichMasterSet][1])
            {
                case 0:
                    WhichTex.Add(0);
                    break;
                case 1:
                    WhichTex.Add(1);
                    break;
                case 2:
                    WhichTex.Add(2);
                    break;
                case 3:
                    WhichTex.Add(3);
                    break;
                case 4:
                    WhichTex.Add(4);
                    break;
                case 5:
                    WhichTex.Add(prng.NextInt(0, (_numTextures - 2) * Fix64.One));
                    break;
            }

            switch (_masterSet[WhichMasterSet][2])
            {
                case 0:
                    WhichColorFlow.Add(0);
                    break;
                case 1:
                    WhichColorFlow.Add(1);
                    break;
                case 2:
                    WhichColorFlow.Add(2);
                    break;
                case 3:
                    WhichColorFlow.Add(3);
                    break;
                case 4:
                    WhichColorFlow.Add(prng.NextInt(0, 3 * Fix64.One));
                    break;
            }

            switch (_masterSet[WhichMasterSet][3])
            {
                case 0:
                    WhichRot.Add(0);
                    break;
                case 1:
                    WhichRot.Add(1);
                    break;
                case 2:
                    WhichRot.Add(2);
                    break; // NO ROT
                case 3:
                    WhichRot.Add(prng.NextInt(0, 2 * Fix64.One));
                    break;
            }

            WhichRotDir.Add(prng.NextInt(0, 1 * Fix64.One));
            universe.WhichGridPos.Add(prng.NextInt(0, (GridSize - 1) * Fix64.One));
            universe.WhichBezierH1A.Add(prng.NextInt(-Fix64.Div(StageW * Fix64.One, Fix64.Two),
                Fix64.Div(StageW * Fix64.One, Fix64.Two)));
            universe.WhichBezierH1B.Add(prng.NextInt(-(StageH * Fix64.One), StageH * Fix64.One));
            universe.WhichBezierH2A.Add(prng.NextInt(-Fix64.Div(StageW * Fix64.One, Fix64.Two),
                Fix64.Div(StageW * Fix64.One, Fix64.Two)));
            universe.WhichBezierH2B.Add(prng.NextInt(-(StageH * Fix64.One), StageH * Fix64.One));
        }

        return universe;
    }

    private void BuildColors()
    {
        _whichClr = _clrs[WhichColor];

        var inter = Fix64.Div(Fix64.One, ColorSpread * Fix64.One);

        MyColorsR = new List<byte>(NumColors);
        MyColorsG = new List<byte>(NumColors);
        MyColorsB = new List<byte>(NumColors);

        for (uint i = 0; i < _whichClr.Length; i++)
        {
            var j = i == _whichClr.Length - 1 ? 0 : i + 1;

            for (uint x = 0; x < ColorSpread; x++)
            {
                var c = ColorMath.Lerp(_whichClr[i], _whichClr[j], Fix64.Mul(inter, x * Fix64.One));
                MyColorsR.Add((byte) (c >> 16));
                MyColorsG.Add((byte) (c >> 8));
                MyColorsB.Add((byte) (c >> 0));
            }
        }

        CLen = MyColorsR.Count;

        MyColorsR = MyColorsR.ToList();
        MyColorsG = MyColorsG.ToList();
        MyColorsB = MyColorsB.ToList();
    }

    private void BuildGrid()
    {
        GridPoints = new List<Vector2>();

        var stageW = StageW * Fix64.One;
        var stageH = StageH * Fix64.One;

        var ratio = Fix64.Div(NumCols * Fix64.One, NumRows * Fix64.One);
        var margin = Fix64.Min(GridMaxMargin * Fix64.One, Fix64.Div(stageW, Fix64.Two));

        var width = Fix64.Sub(stageW, Fix64.Mul(margin, Fix64.Two));
        var height = Fix64.Div(width, ratio);

        if (height > Fix64.Sub(stageH, Fix64.Mul(margin, Fix64.Two)))
        {
            height = Fix64.Sub(stageH, Fix64.Mul(margin, Fix64.Two));
            width = Fix64.Mul(height, ratio);
        }

        var gridStartX = Fix64.Div(-width, Fix64.Two);
        var gridStartY = Fix64.Div(-height, Fix64.Two);
        var gridSpaceX = Fix64.Div(width, Fix64.Sub(NumCols * Fix64.One, Fix64.One));
        var gridSpaceY = Fix64.Div(height, Fix64.Sub(NumRows * Fix64.One, Fix64.One));

        for (var i = 0; i < GridSize; i++)
        {
            var col = i % NumCols;
            var row = Fix64.Floor(Fix64.Div(i * Fix64.One, NumCols * Fix64.One));
            var x = Fix64.Add(gridStartX, Fix64.Mul(col * Fix64.One, gridSpaceX));
            var y = Fix64.Add(gridStartY, Fix64.Mul(row, gridSpaceY));

            GridPoints.Add(new Vector2(x, y));
        }
    }


    private void BuildPaths(Universe universe)
    {
        Paths = new Bezier[GridSize];

        NumPaths = 0;

        for (var i = 0; i < GridSize; i++)
        {
            var p1 = new Vector2 {X = GridPoints[i].X, Y = GridPoints[i].Y};
            var p2 = p1;
            var p3 = new Vector2 {X = GridPoints[EndIdx].X, Y = GridPoints[EndIdx].Y};
            var p4 = p3;

            switch (universe.WhichBezierPattern[i])
            {
                case 0: break;
                case 1:
                    p3 = p4 = new Vector2
                        {X = GridPoints[universe.WhichGridPos[i]].X, Y = GridPoints[universe.WhichGridPos[i]].Y};
                    break;
                case 2:
                    p3 = new Vector2
                        {X = universe.WhichBezierH1A[i] * Fix64.One, Y = universe.WhichBezierH1B[i] * Fix64.One};
                    p4 = new Vector2
                        {X = GridPoints[universe.WhichGridPos[i]].X, Y = GridPoints[universe.WhichGridPos[i]].Y};
                    break;
                case 3:
                    p3 = p4 = new Vector2 {X = GridPoints[i].X, Y = GridPoints[i].Y};
                    break;
                case 4:
                    p2 = new Vector2
                        {X = universe.WhichBezierH1A[i] * Fix64.One, Y = universe.WhichBezierH1B[i] * Fix64.One};
                    p3 = new Vector2
                        {X = universe.WhichBezierH2A[i] * Fix64.One, Y = universe.WhichBezierH2B[i] * Fix64.One};
                    p4 = new Vector2
                        {X = GridPoints[universe.WhichGridPos[i]].X, Y = GridPoints[universe.WhichGridPos[i]].Y};
                    break;
                case 5:
                    p2 = new Vector2
                        {X = universe.WhichBezierH1A[i] * Fix64.One, Y = universe.WhichBezierH1B[i] * Fix64.One};
                    p3 = new Vector2
                        {X = universe.WhichBezierH2A[i] * Fix64.One, Y = universe.WhichBezierH2B[i] * Fix64.One};
                    break;
            }

            Paths[NumPaths++] = new Bezier(p1, p2, p3, p4);
        }
    }

    public static void BuildStars(XorShift prng, Parameters parameters)
    {
        parameters.StarPositions = new List<Star>();

        for (var i = 0; i < parameters.StarMax; ++i)
        {
            var x = prng.NextInt(
                Fix64.Mul(parameters.GridPoints[0].X, 5368709120L /* 1.25 */),
                Fix64.Mul(parameters.GridPoints[GridSize - 1].X, 5368709120L)
            );

            var y = prng.NextInt(
                Fix64.Mul(parameters.GridPoints[0].Y, 4724464128L /* 1.1 */),
                Fix64.Mul(parameters.GridPoints[GridSize - 1].Y, 4724464128L)
            );

            var sTemp = prng.NextInt(1 * Fix64.One, 3 * Fix64.One);
            var s = sTemp == 1 ? 1000 : sTemp == 2 ? 2000 : 3000;
            var c = prng.NextInt(0, (parameters.CLen - 1) * Fix64.One);
            parameters.StarPositions.Add(new Star {X = x, Y = y, S = s, C = c});
        }
    }

    private class Universe
    {
        public readonly List<int> WhichBezierH1A = new();
        public readonly List<int> WhichBezierH1B = new();
        public readonly List<int> WhichBezierH2A = new();
        public readonly List<int> WhichBezierH2B = new();
        public readonly List<int> WhichBezierPattern = new();
        public readonly List<int> WhichGridPos = new();
    }
}