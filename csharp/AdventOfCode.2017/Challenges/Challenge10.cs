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
        var numbers = new CircularArray<byte>(256);
        for(var b = 0; b < 256; b++)
            numbers[b] = (byte)b;

        var i = 0;
        var skip = 0;

        await foreach (var length in inputReader.ReadLineAsync<int>(10, ','))
        {
            Twist(numbers, i, length);
            i = (i + length + skip).Mod(numbers.Count);
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
        var copy = new byte[length];
        hash.CopyTo(copy, start, length);

        for (var i = 0; i < length; i++)
            hash[start + i] = copy[^(i + 1)];
    }
}