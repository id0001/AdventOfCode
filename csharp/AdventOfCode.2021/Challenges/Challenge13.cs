﻿using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    private async Task<(HashSet<Point2>, List<string>)> ReadInputAsync()
    {
        var lines = await inputReader.ReadLinesAsync(13).ToArrayAsync();
        var indexOfNewline = Array.IndexOf(lines, string.Empty);

        var points = lines.Take(indexOfNewline).Select(x =>
        {
            var point = x.Split(',');
            return new Point2(int.Parse(point[0]), int.Parse(point[1]));
        }).ToHashSet();

        var folds = lines.Skip(indexOfNewline + 1).ToList();
        return (points, folds);
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var (points, folds) = await ReadInputAsync();

        var (axis, index) = ReadFold(folds[0]);
        var set = Fold(points, axis, index);

        return set.Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (points, folds) = await ReadInputAsync();

        var output = points;
        foreach (var (axis, index) in folds.Select(ReadFold)) output = Fold(output, axis, index);

        var xmax = output.Max(p => p.X);
        var ymax = output.Max(p => p.Y);

        var sb = new StringBuilder();
        sb.AppendLine();

        for (var y = 0; y <= ymax; y++)
        {
            for (var x = 0; x <= xmax; x++)
                if (output.Contains(new Point2(x, y)))
                    sb.Append("#");
                else
                    sb.Append(".");

            sb.AppendLine();
        }

        return sb.ToString().Ocr();
    }

    private static (string, int) ReadFold(string fold)
    {
        var match = Regex.Match(fold, @"^fold along (x|y)=(\d+)$");
        return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
    }

    private static HashSet<Point2> Fold(IEnumerable<Point2> points, string axis, int index)
    {
        var newPoints = new HashSet<Point2>();
        foreach (var point in points)
            if (axis == "x")
            {
                if (point.X < index)
                {
                    newPoints.Add(point);
                }
                else
                {
                    var diff = point.X - index;
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
                    var diff = point.Y - index;
                    newPoints.Add(new Point2(point.X, point.Y - 2 * diff));
                }
            }

        return newPoints;
    }
}