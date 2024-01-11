using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2020.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader InputReader)
{
    private readonly IDictionary<long, long> _offsets = new SortedDictionary<long, long>();
    private long[] _busses = Array.Empty<long>();
    private long _earliestDepartureTime;

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await InputReader.ReadLinesAsync(13).ToArrayAsync();
        _earliestDepartureTime = long.Parse(lines[0]);
        _busses = lines[1].Split(',').Where(e => e != "x").Select(long.Parse).ToArray();

        var offset = 0;
        foreach (var s in lines[1].Split(','))
        {
            if (s != "x") _offsets.Add(long.Parse(s), offset);
            offset++;
        }
    }

    [Part1]
    public string Part1()
    {
        var ordered = _busses
            .ToDictionary(kv => kv, kv => (int) (Math.Ceiling(_earliestDepartureTime / (double) kv) * kv))
            .OrderBy(kv => kv.Value).ToArray();
        return (ordered[0].Key * (ordered[0].Value - _earliestDepartureTime)).ToString();
    }

    [Part2]
    public string Part2()
    {
        var totalMod = _busses.Product();
        var total = 0L;
        for (var i = 1; i < _busses.Length; i++)
        {
            var bi = _busses[i] - _offsets[_busses[i]];
            var ni = totalMod / _busses[i];
            var xi = Euclid.ModInverse(ni, _busses[i]);
            total += bi * ni * xi;
        }

        return (total % totalMod).ToString();
    }
}