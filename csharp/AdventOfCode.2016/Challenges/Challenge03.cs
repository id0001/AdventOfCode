using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => await inputReader
            .ReadLinesAsync(3)
            .Select(line => line.SplitBy(" ").As<int>())
            .CountAsync(arr =>
                arr.First() + arr.Second() > arr.Third()
                && arr.Second() + arr.Third() > arr.First()
                && arr.Third() + arr.First() > arr.Second())
            .ToStringAsync();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader
            .ReadLinesAsync(3)
            .Select(line => line.SplitBy(" ").As<int>())
            .ToListAsync())
            .Into(lines => lines.Select(l => l[0]).Concat(lines.Select(l => l[1]).Concat(lines.Select(l => l[2]))))
            .Chunk(3)
            .Count(arr =>
                arr.First() + arr.Second() > arr.Third()
                && arr.Second() + arr.Third() > arr.First()
                && arr.Third() + arr.First() > arr.Second()
            ).ToString();
}
