using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await inputReader.ReadLinesAsync(3).ToListAsync();

        var sum = 0;
        var numberPattern = new Regex(@"(\d+)", RegexOptions.Compiled);
        for (var l = 0; l < lines.Count; l++)
        {
            var line = lines[l];
            var matches = numberPattern.Matches(line);
            for (var i = 0; i < matches.Count; i++)
            {
                var numstr = matches[i].Groups[1].Value;
                var index = matches[i].Groups[1].Index;

                var neighbors = Enumerable.Range(index, numstr.Length).Select(x => new Point2(x, l))
                    .SelectMany(p => p.GetNeighbors(true)).ToHashSet();
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.Y < 0 || neighbor.Y >= lines.Count || neighbor.X < 0 || neighbor.X >= line.Length)
                        continue;

                    var c = lines[neighbor.Y][neighbor.X];
                    if (!char.IsDigit(c) && c != '.')
                    {
                        sum += int.Parse(numstr);
                        break;
                    }
                }
            }
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await inputReader.ReadLinesAsync(3).ToListAsync();

        var sum = 0;
        for (var y = 0; y < lines.Count; y++)
        for (var x = 0; x < lines[y].Length; x++)
            if (lines[y][x] == '*')
                sum += FindGearRatio(lines, x, y);

        return sum.ToString();
    }

    private int FindGearRatio(List<string> lines, int x, int y)
    {
        var numPosCheck = new HashSet<Point2>();
        var numbers = new List<int>();
        foreach (var neighbor in new Point2(x, y).GetNeighbors(true))
            if (char.IsDigit(lines[neighbor.Y][neighbor.X]))
            {
                var (index, length) = ExpandNumber(lines, neighbor.X, neighbor.Y);
                var p = new Point2(index, neighbor.Y);
                if (!numPosCheck.Contains(p))
                {
                    numPosCheck.Add(p);
                    numbers.Add(int.Parse(lines[neighbor.Y].Substring(index, length)));
                }
            }

        if (numbers.Count != 2)
            return 0;

        return numbers.Product();
    }

    private (int, int) ExpandNumber(List<string> lines, int x, int y)
    {
        int index;
        for (index = x; index >= 0; index--)
            if (index - 1 < 0 || !char.IsDigit(lines[y][index - 1]))
                break;

        int length;
        for (length = index; length < lines[y].Length; length++)
            if (length + 1 >= lines[y].Length || !char.IsDigit(lines[y][length + 1]))
                break;

        length = length - index + 1;
        return (index, length);
    }
}