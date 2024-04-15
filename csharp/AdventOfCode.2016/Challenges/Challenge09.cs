using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => DecompressV1(await inputReader.ReadAllTextAsync(9)).ToString();

    [Part2]
    public async Task<string> Part2Async() => DecompressV2(await inputReader.ReadAllTextAsync(9), []).ToString();

    private static int DecompressV1(string compressed)
    {
        var totalLength = 0;
        for (var i = 0; i < compressed.Length; i++)
        {
            if (compressed[i] == '(')
            {
                var marker = compressed[i..(compressed.IndexOf(')', i) + 1)];
                i += marker.Length;
                var (x, y, _) = marker.Extract(@"(\d+)x(\d+)").As<int>();
                totalLength += x * y;
                i += x - 1;
                continue;
            }

            totalLength++;
        }

        return totalLength;
    }

    private static long DecompressV2(string compressed, Dictionary<string, long> cache)
    {
        long totalLength = 0;
        for (var i = 0; i < compressed.Length; i++)
        {
            if (compressed[i] == '(')
            {
                var marker = compressed[i..(compressed.IndexOf(')', i) + 1)];
                i += marker.Length;
                var (x, y, _) = marker.Extract(@"(\d+)x(\d+)").As<int>();

                var str = string.Join(string.Empty, Enumerable.Repeat(compressed[i..(i + x)], y));
                if (!cache.TryGetValue(str, out var length))
                {
                    length = DecompressV2(str, cache);
                    cache.TryAdd(str, length);
                }

                totalLength += length;
                i += x - 1;
                continue;
            }

            totalLength++;
        }

        return totalLength;
    }
}