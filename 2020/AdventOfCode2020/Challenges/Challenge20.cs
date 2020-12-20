using AdventOfCodeLib;
using AdventOfCodeLib.Collections;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(20)]
    public class Challenge20
    {
        private readonly IInputReader inputReader;
        private IList<Image> images;

        public Challenge20(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            images = new List<Image>();

            var rawData = new List<string>();
            await foreach (var line in inputReader.ReadLinesAsync(20))
            {
                if (string.IsNullOrEmpty(line))
                {
                    images.Add(new Image(rawData));
                    rawData = new List<string>();
                    continue;
                }

                rawData.Add(line);
            }

            if (rawData.Count > 0)
            {
                images.Add(new Image(rawData));
            }
        }

        [Part1]
        public string Part1()
        {
            var map = new SparseSpatialMap<Point2, Image>();
            var list = images.ToList();
            map.Add(Point2.Zero, list[0]);
            list.Remove(map[Point2.Zero]);

            var stack = new Stack<Point2>(new[] { Point2.Zero });
            while (stack.Count > 0)
            {
                var coord = stack.Pop();

                var neighbors = new Point2[4]
                {
                    coord + new Point2(0,-1),
                    coord + new Point2(1, 0),
                    coord + new Point2(0,1),
                    coord + new Point2(-1,0)
                };

                List<Image> toRemove = new List<Image>();
                foreach (var img in list)
                {
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
                        else if (!map.ContainsKey(neighbors[1]) && img.Left == map[coord].Right)
                        {
                            map.Add(neighbors[1], img);
                            toRemove.Add(img);
                            stack.Push(neighbors[1]);
                            break;
                        }
                        else if (!map.ContainsKey(neighbors[2]) && img.Top == map[coord].Bottom)
                        {
                            map.Add(neighbors[2], img);
                            toRemove.Add(img);
                            stack.Push(neighbors[2]);
                            break;
                        }
                        else if (!map.ContainsKey(neighbors[3]) && img.Right == map[coord].Left)
                        {
                            map.Add(neighbors[3], img);
                            toRemove.Add(img);
                            stack.Push(neighbors[3]);
                            break;
                        }
                    }
                }

                foreach (var item in toRemove)
                    list.Remove(item);
            }

            for (int y = map.Bounds.GetMin(1); y < map.Bounds.GetMax(1); y++)
            {
                var ids = new List<long>();
                for (int x = map.Bounds.GetMin(0); x < map.Bounds.GetMax(0); x++)
                {
                    if (!map.ContainsKey(new Point2(x, y)))
                        ids.Add(0);
                    else
                        ids.Add(map[new Point2(x, y)].Id);
                }

                Console.WriteLine(string.Join(",", ids));
            }

            int l = map.Bounds.GetMin(0);
            int r = map.Bounds.GetMax(0) - 1;
            int t = map.Bounds.GetMin(1);
            int b = map.Bounds.GetMax(1) - 1;

            return (map[new Point2(l, t)].Id * map[new Point2(r, t)].Id * map[new Point2(l, b)].Id * map[new Point2(r, b)].Id).ToString();
        }

        private class Image
        {
            public static readonly IList<int[]> PossibleOrientations;

            private readonly string[] data;

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

                int top = CalcSide(rawData[1]);
                int bottom = CalcSide(string.Concat(rawData[10].Reverse()));
                int right = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[9])));
                int left = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[0]).Reverse()));

                int topr = CalcSide(rawData[1], true);
                int bottomr = CalcSide(string.Concat(rawData[10].Reverse()), true);
                int rightr = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[9])), true);
                int leftr = CalcSide(string.Concat(rawData.Skip(1).Select(e => e[0]).Reverse()), true);

                Sides.Add((0, 1), top);
                Sides.Add((1, 2), right);
                Sides.Add((2, 3), bottom);
                Sides.Add((3, 0), left);
                Sides.Add((1, 0), topr);
                Sides.Add((2, 1), rightr);
                Sides.Add((3, 2), bottomr);
                Sides.Add((0, 3), leftr);

                Orientation = new int[] { 0, 1, 2, 3 };

                data = rawData.Skip(1).ToArray();
            }

            public long Id { get; }

            public int Width => 10;

            public int Height => 10;

            public int[] Orientation { get; set; }

            public int Top => Sides[(Orientation[0], Orientation[1])];

            public int Right => Sides[(Orientation[1], Orientation[2])];

            public int Bottom => Sides[(Orientation[3], Orientation[2])];

            public int Left => Sides[(Orientation[0], Orientation[3])];

            public IDictionary<(int, int), int> Sides { get; } = new Dictionary<(int, int), int>();

            //public char this[int y, int x]
            //{
            //    get
            //    {
            //        if(Orientation[0] == 0 && Orientation[1] == 1)
            //        {
            //            return data[y][x];
            //        }
            //        else if (Orientation[0] == 1 && Orientation[1] == 2)
            //        {
            //            return 
            //        }
            //    }
            //}

            private static int CalcSide(string line, bool reverse = false)
            {
                int s = 0;
                for (int i = 0; i < line.Length; i++)
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
}
