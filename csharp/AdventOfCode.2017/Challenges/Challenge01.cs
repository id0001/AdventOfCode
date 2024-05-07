using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2017.Challenges;

[Challenge(1)]
public class Challenge01(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async() => (await inputReader.ReadLineAsync(1).Select(c => (int)char.GetNumericValue(c)).ToListAsync())
        .Into(list => list.Where((v, i) => list[Euclid.Modulus(i + 1, list.Count)] == v).Sum())
        .ToString();

    [Part2]
    public async Task<string> Part2Async() => (await inputReader.ReadLineAsync(1).Select(c => (int)char.GetNumericValue(c)).ToListAsync())
        .Into(list => list.Where((v, i) => list[Euclid.Modulus(i + (list.Count/2), list.Count)] == v).Sum())
        .ToString();
}
