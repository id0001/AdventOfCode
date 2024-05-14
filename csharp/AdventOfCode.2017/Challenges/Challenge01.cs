using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => (await inputReader.ReadLineAsync(1).Select(c => c.AsInteger()).ToListAsync())
        .Into(list => list
            .Cycle()
            .Skip(1)
            .Zip(list)
            .Where(pair => pair.First == pair.Second)
            .Select(pair => pair.First)
            .Sum())
        .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadLineAsync(1).Select(c => c.AsInteger()).ToListAsync())
        .Into(list => list
            .Cycle()
            .Skip(list.Count / 2)
            .Zip(list)
            .Where(pair => pair.First == pair.Second)
            .Select(pair => pair.First)
            .Sum())
        .ToString();
}
