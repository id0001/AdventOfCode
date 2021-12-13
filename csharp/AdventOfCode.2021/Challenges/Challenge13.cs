using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(13)]
    public class Challenge13
    {
        private readonly IInputReader inputReader;
        private ISet<Point2> points;
        private List<string> folds;

        public Challenge13(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            var lines = await inputReader.ReadLinesAsync(13).ToArrayAsync();
            int indexOfNewline = Array.IndexOf(lines, string.Empty);

            points = lines.Take(indexOfNewline).Select(x =>
            {
                var point = x.Split(',');
                return new Point2(int.Parse(point[0]), int.Parse(point[1]));
            }).ToHashSet();

            folds = lines.Skip(indexOfNewline + 1).ToList();
        }

        [Part1]
        public string Part1()
        {
            var (axis, index) = ReadFold(folds[0]);
            var set = Fold(points, axis, index);

            return set.Count.ToString();
        }

        [Part2]
        public string Part2()
        {
            ISet<Point2> output = points;
            foreach(var (axis, index) in folds.Select(ReadFold))
            {
                output = Fold(output, axis, index);
            }

            int xmax = output.Max(p => p.X);
            int ymax = output.Max(p => p.Y);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            for (int y = 0; y <= ymax; y++)
            {
                for(int x = 0; x <= xmax; x++)
                {
                    if(output.Contains(new Point2(x, y)))
                    {
                        sb.Append("#");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private (string, int) ReadFold(string fold)
        {
            var match = Regex.Match(fold, @"^fold along (x|y)=(\d+)$");
            return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
        }

        private static ISet<Point2> Fold(ISet<Point2> points, string axis, int index)
        {
            ISet<Point2> newPoints = new HashSet<Point2>();
            foreach (var point in points)
            {
                if (axis == "x")
                {

                    if (point.X < index)
                    {
                        newPoints.Add(point);
                    }
                    else
                    {
                        int diff = point.X - index;
                        newPoints.Add(new Point2(point.X - 2 * diff, point.Y));
                    }
                }
                else
                {
                    if (point.Y < index)
                    {
                        newPoints.Add(point);
                    }
                    else
                    {
                        int diff = point.Y - index;
                        newPoints.Add(new Point2(point.X,point.Y - 2 * diff));
                    }
                }
            }

            return newPoints;
        }
    }
}
