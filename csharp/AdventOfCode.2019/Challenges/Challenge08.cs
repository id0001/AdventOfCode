using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(8)]
public class Challenge08
{
    private readonly IInputReader _inputReader;

    public Challenge08(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var rawInput = await _inputReader.ReadLineAsync(8).Select(Convert.ToInt32).ToArrayAsync();
        const int width = 25;
        const int height = 6;
        const int ppl = width * height;
        var layers = rawInput.Length / ppl;

        var segments = Enumerable.Range(0, layers).Select(i => new ArraySegment<int>(rawInput, i * ppl, ppl)).ToArray();
        var leastZeros = segments.MinBy(s => s.Count(x => x == 0));

        var c1 = 0;
        var c2 = 0;

        foreach (var el in leastZeros)
        {
            c1 += el == 1 ? 1 : 0;
            c2 += el == 2 ? 1 : 0;
        }

        return (c1 * c2).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var rawInput = await _inputReader.ReadLineAsync(8).Select(Convert.ToInt32).ToArrayAsync();
        const int width = 25;
        const int height = 6;
        const int ppl = width * height;
        var layers = rawInput.Length / ppl;

        var segments = Enumerable.Range(0, layers).Select(i => new ArraySegment<int>(rawInput, i * ppl, ppl)).ToArray();

        var image = Enumerable.Range(0, ppl).Select(i =>
        {
            for (var si = 0; si < segments.Length; si++)
            {
                if (segments[si][i] != 2)
                    return segments[si][i];
            }

            return -1;
        }).ToArray();

        var sb = new StringBuilder();
        sb.AppendLine();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++) sb.Append(image[y * width + x] == 1 ? '#' : ' ');

            sb.AppendLine();
        }

        return sb.ToString();
    }
}