using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2020.Challenges;

[Challenge(5)]
public class Challenge05(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await InputReader.ReadLinesAsync(5).ToArrayAsync();

        var highestSeatId = input.Select(query => Search(8, 128, query)).Select(p => p.Y * 8 + p.X).Prepend(0).Max();

        return highestSeatId.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await InputReader.ReadLinesAsync(5).ToArrayAsync();

        var seatIds = new HashSet<int>();
        foreach (var query in input)
        {
            var p = Search(8, 128, query);
            var id = p.Y * 8 + p.X;
            seatIds.Add(id);
        }

        var mySeat = Enumerable
            .Range(0, 8 * 128)
            .First(n => !seatIds.Contains(n) && seatIds.Contains(n - 1) && seatIds.Contains(n + 1));

        return mySeat.ToString();
    }

    private static Point2 Search(int width, int height, string query)
    {
        int xmin = 0, ymin = 0, xmax = width, ymax = height;
        foreach (var c in query)
            switch (c)
            {
                case 'F':
                    ymax = (ymax + ymin) / 2;
                    break;
                case 'B':
                    ymin = (ymax + ymin) / 2;
                    break;
                case 'L':
                    xmax = (xmax + xmin) / 2;
                    break;
                case 'R':
                    xmin = (xmax + xmin) / 2;
                    break;
            }

        return new Point2(xmin, ymin);
    }
}