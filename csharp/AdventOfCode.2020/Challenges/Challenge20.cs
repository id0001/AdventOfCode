using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;
using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(20)]
public class Challenge20
{
    private readonly IInputReader _inputReader;
    private List<Image> _images = new();

    public Challenge20(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var rawData = new List<string>();
        await foreach (var line in _inputReader.ReadLinesAsync(20))
        {
            if (string.IsNullOrEmpty(line))
            {
                _images.Add(new Image(rawData));
                rawData = new List<string>();
                continue;
            }

            rawData.Add(line);
        }

        if (rawData.Count > 0) _images.Add(new Image(rawData));
    }

    [Part1]
    public string Part1()
    {
        var map = StitchImage();

        var l = map.Bounds.GetMin(0);
        var r = map.Bounds.GetMax(0) - 1;
        var t = map.Bounds.GetMin(1);
        var b = map.Bounds.GetMax(1) - 1;

        return (map[new Point2(l, t)].Id * map[new Point2(r, t)].Id * map[new Point2(l, b)].Id *
                map[new Point2(r, b)].Id).ToString();
    }

    [Part2]
    public string Part2()
    {
        var map = StitchImage();

        var lake = new List<string> { "Tile 0000:" };
        for (var mapY = map.Bounds.GetMin(1); mapY < map.Bounds.GetMax(1); mapY++)
        for (var lineY = 1; lineY < 9; lineY++)
        {
            var sb = new StringBuilder();
            for (var mapX = map.Bounds.GetMin(0); mapX < map.Bounds.GetMax(0); mapX++)
            {
                var p = new Point2(mapX, mapY);

                for (var lineX = 1; lineX < 9; lineX++) sb.Append(map[p][lineY, lineX]);
            }

            lake.Add(sb.ToString());
        }

        var monster = new[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };

        var lakeImg = new Image(lake);

        var monsterCount = 0;
        foreach (var orientation in Image.PossibleOrientations)
        {
            lakeImg.Orientation = orientation;
            for (var y = 0; y < lakeImg.Height - 3; y++)
            for (var x = 0; x < lakeImg.Width - monster[0].Length; x++)
            {
                if (IsMonster(lakeImg, monster, x, y))
                {
                    monsterCount++;
                    MarkMonster(lakeImg, monster, x, y);
                }
            }

            if (monsterCount > 0)
                break;
        }

        var foamCount = 0;
        for (var y = 0; y < lakeImg.Height; y++)
        for (var x = 0; x < lakeImg.Width; x++)
        {
            if (lakeImg[y, x] == '#')
                foamCount++;
        }

        return foamCount.ToString();
    }

    private static bool IsMonster(Image lake, IReadOnlyList<string> monster, int lx, int ly)
    {
        for (var my = 0; my < monster.Count; my++)
        for (var mx = 0; mx < monster[my].Length; mx++)
        {
            if (monster[my][mx] == '#' && lake[ly + my, lx + mx] != '#')
                return false;
        }

        return true;
    }

    private static void MarkMonster(Image lake, IReadOnlyList<string> monster, int lx, int ly)
    {
        for (var my = 0; my < monster.Count; my++)
        for (var mx = 0; mx < monster[my].Length; mx++)
        {
            if (monster[my][mx] == '#' && lake[ly + my, lx + mx] == '#')
                lake[ly + my, lx + mx] = 'O';
        }
    }

    private SparseSpatialMap<Point2, Image> StitchImage()
    {
        var map = new SparseSpatialMap<Point2, Image>();
        var list = _images.ToList();
        map.Add(Point2.Zero, list[0]);
        list.Remove(map[Point2.Zero]);

        var stack = new Stack<Point2>(new[] { Point2.Zero });
        while (stack.Count > 0)
        {
            var coord = stack.Pop();

            var neighbors = new[]
            {
                coord + new Point2(0, -1),
                coord + new Point2(1, 0),
                coord + new Point2(0, 1),
                coord + new Point2(-1, 0)
            };

            var toRemove = new List<Image>();
            foreach (var img in list)
            foreach (var orientation in Image.PossibleOrientations)
            {
                img.Orientation = orientation;

                if (!map.ContainsKey(neighbors[0]) && img.Bottom == map[coord].Top)
                {
                    map.Add(neighbors[0], img);
                    toRemove.Add(img);
                    stack.Push(neighbors[0]);
                    break;
                }

                if (!map.ContainsKey(neighbors[1]) && img.Left == map[coord].Right)
                {
                    map.Add(neighbors[1], img);
                    toRemove.Add(img);
                    stack.Push(neighbors[1]);
                    break;
                }

                if (!map.ContainsKey(neighbors[2]) && img.Top == map[coord].Bottom)
                {
                    map.Add(neighbors[2], img);
                    toRemove.Add(img);
                    stack.Push(neighbors[2]);
                    break;
                }

                if (!map.ContainsKey(neighbors[3]) && img.Right == map[coord].Left)
                {
                    map.Add(neighbors[3], img);
                    toRemove.Add(img);
                    stack.Push(neighbors[3]);
                    break;
                }
            }

            foreach (var item in toRemove)
                list.Remove(item);
        }

        return map;
    }

    private class Image
    {
        public static readonly IList<int[]> PossibleOrientations;

        private readonly string[] _data;

        static Image()
        {
            PossibleOrientations = new List<int[]>(8);
            PossibleOrientations.Add(new[] { 0, 1, 2, 3 });
            PossibleOrientations.Add(new[] { 1, 2, 3, 0 });
            PossibleOrientations.Add(new[] { 2, 3, 0, 1 });
            PossibleOrientations.Add(new[] { 3, 0, 1, 2 });
            PossibleOrientations.Add(new[] { 3, 2, 1, 0 });
            PossibleOrientations.Add(new[] { 2, 1, 0, 3 });
            PossibleOrientations.Add(new[] { 1, 0, 3, 2 });
            PossibleOrientations.Add(new[] { 0, 3, 2, 1 });
        }

        public Image(IList<string> rawData)
        {
            Id = long.Parse(rawData[0].Substring(5, 4));
            Height = rawData.Count - 1;
            Width = rawData[1].Length;

            var top = CalcSide(rawData[1]);
            var bottom = CalcSide(string.Concat(rawData[10].Reverse()));
            var right = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[9])));
            var left = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[0]).Reverse()));

            var topr = CalcSide(rawData[1], true);
            var bottomr = CalcSide(string.Concat(rawData[10].Reverse()), true);
            var rightr = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[9])), true);
            var leftr = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[0]).Reverse()), true);

            Sides.Add((0, 1), top);
            Sides.Add((1, 2), right);
            Sides.Add((2, 3), bottom);
            Sides.Add((3, 0), left);
            Sides.Add((1, 0), topr);
            Sides.Add((2, 1), rightr);
            Sides.Add((3, 2), bottomr);
            Sides.Add((0, 3), leftr);

            Orientation = new[] { 0, 1, 2, 3 };

            _data = rawData.Skip(1).ToArray();
        }

        public long Id { get; }

        public int Width { get; }

        public int Height { get; }

        public int[] Orientation { get; set; }

        public int Top => Sides[(Orientation[0], Orientation[1])];

        public int Right => Sides[(Orientation[1], Orientation[2])];

        public int Bottom => Sides[(Orientation[3], Orientation[2])];

        public int Left => Sides[(Orientation[0], Orientation[3])];

        public IDictionary<(int, int), int> Sides { get; } = new Dictionary<(int, int), int>();

        public char this[int y, int x]
        {
            get
            {
                if (PossibleOrientations.Take(4).Contains(Orientation)) // 0 1 2 3
                {
                    var (px, py) = Rotate(x, y, Orientation[0]);
                    return _data[py][px];
                }
                else // 3 2 1 0
                {
                    y = Height - 1 - y;
                    var (px, py) = Rotate(x, y, Orientation[3]);
                    return _data[py][px];
                }
            }

            set
            {
                int px, py;
                if (PossibleOrientations.Take(4).Contains(Orientation)) // 0 1 2 3
                {
                    (px, py) = Rotate(x, y, Orientation[0]);
                }
                else // 3 2 1 0
                {
                    y = Height - 1 - y;
                    (px, py) = Rotate(x, y, Orientation[3]);
                }

                var s = _data[py].Remove(px, 1);
                _data[py] = s.Insert(px, value.ToString());
            }
        }

        private (int, int) Rotate(int x, int y, int amount)
        {
            var cx = (Width - 1) / 2d;
            var cy = (Height - 1) / 2d;
            var angle = amount * (Math.PI / 2d);

            var px = (int)Math.Round((x - cx) * Math.Cos(angle) - (y - cy) * Math.Sin(angle) + cx);
            var py = (int)Math.Round((x - cx) * Math.Sin(angle) + (y - cy) * Math.Cos(angle) + cy);
            return (px, py);
        }

        private static int CalcSide(string line, bool reverse = false)
        {
            var s = 0;
            for (var i = 0; i < line.Length; i++)
            {
                if (!reverse)
                    s += line[i] == '#' ? 1 << i : 0;
                else
                    s += line[i] == '#' ? 1 << (line.Length - 1 - i) : 0;
            }

            return s;
        }
    }
}