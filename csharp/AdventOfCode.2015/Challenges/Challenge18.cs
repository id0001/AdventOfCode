using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var state1 = await inputReader.ReadGridAsync(18);
        var bounds = new Rectangle(0, 0, 100, 100);

        for (var i = 0; i < 100; i++)
        {
            var state2 = new char[100, 100];

            for (var y = 0; y < state1.GetLength(0); y++)
            for (var x = 0; x < state1.GetLength(1); x++)
            {
                var onCount = new Point2(x, y).GetNeighbors(true).Where(bounds.Contains)
                    .Count(n => state1[n.Y, n.X] == '#');

                if (state1[y, x] == '#')
                    state2[y, x] = onCount == 2 || onCount == 3 ? '#' : '.';
                else
                    state2[y, x] = onCount == 3 ? '#' : '.';
            }

            state1 = state2;
        }

        return Enumerable.Range(0, 100 * 100).Count(i => state1[i / 100, i % 100] == '#').ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var state1 = await inputReader.ReadGridAsync(18);
        var bounds = new Rectangle(0, 0, 100, 100);

        for (var i = 0; i < 100; i++)
        {
            var state2 = new char[100, 100];

            for (var y = 0; y < state1.GetLength(0); y++)
            for (var x = 0; x < state1.GetLength(1); x++)
            {
                var onCount = new Point2(x, y).GetNeighbors(true).Where(bounds.Contains)
                    .Count(n => state1[n.Y, n.X] == '#');

                if (state1[y, x] == '#')
                    state2[y, x] = onCount == 2 || onCount == 3 ? '#' : '.';
                else
                    state2[y, x] = onCount == 3 ? '#' : '.';
            }

            state2[0, 0] = '#';
            state2[0, 99] = '#';
            state2[99, 0] = '#';
            state2[99, 99] = '#';

            state1 = state2;
        }

        return Enumerable.Range(0, 100 * 100).Count(i => state1[i / 100, i % 100] == '#').ToString();
    }
}