using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;
using Spectre.Console;
using System.Net.WebSockets;

namespace AdventOfCode2015.Challenges;

[Challenge(24)]
public class Challenge24(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var weights = await inputReader.ReadLinesAsync<int>(24).ToListAsync();
        var targetTotal = weights.Sum() / 3;

        var lowestQE = int.MaxValue;
        var prevLen = 0;
        foreach (var partition in weights.Partitions(3, true))
        {
            if (partition[0].Sum() != targetTotal)
                continue;

            if (!partition.All(x => x.Sum() == targetTotal))
                continue;

            lowestQE = Math.Min(lowestQE, partition[0].Product());
            if (prevLen > 0 && prevLen != partition[0].Length)
                break;

            prevLen = partition[0].Length;
        }

        return lowestQE.ToString();
        //return weights
        //    .Partitions(3, true)
        //    .Where(x => x.All(y => y.Sum() == targetTotal))
        //    .OrderBy(x => x[0].Product())
        //    .First()[0].Product()
        //    .ToString();
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
    }
}
