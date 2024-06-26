using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        CircularArray<byte> numbers = new(Enumerable.Range(0,256).Select(x => (byte)x).ToArray());
        var i = 0;
        var skip = 0;

        await foreach (var length in inputReader.ReadLineAsync<int>(10, ','))
        {
            Twist(numbers, i, length);
            i = (i + length + skip).Mod(numbers.Length);
            skip++;
        }

        return numbers.Take(2).Select(b => (int)b).Product().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        return string.Join("", KnotHash.Generate(await inputReader.ReadAllTextAsync(10)).Select(x => x.ToHexString()));
    }

    private static void Twist(CircularArray<byte> hash, int start, int length)
    {
        var (l, r) = ((int)Math.Round(length / 2d, MidpointRounding.AwayFromZero) - 1, (int)Math.Floor(length / 2d));
        for (; l >= 0 && r < length; l--, r++)
            (hash[start + l], hash[start + r]) = (hash[start + r], hash[start + l]);
    }
}