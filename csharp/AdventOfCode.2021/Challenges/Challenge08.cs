using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(8)]
public class Challenge08
{
    private readonly IInputReader _inputReader;
    private IoPair[] _data = Array.Empty<IoPair>();

    public Challenge08(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        _data = await _inputReader.ReadLinesAsync(8).Select(line =>
        {
            var ioSplit = line.Split(new[] { '|' }, StringSplitOptions.TrimEntries);
            return new IoPair(new Input(ioSplit[0].Split(' ')), new Output(ioSplit[1].Split(' ')));
        }).ToArrayAsync();
    }

    [Part1]
    public string Part1()
    {
        var lengths = new[] { 2, 3, 4, 7 };
        var count = _data.SelectMany(x => x.Output.Values).Count(x => lengths.Contains(x.Length));

        return count.ToString();
    }

    [Part2]
    public string Part2()
    {
        return _data.Select(FindNumberForSignal).Sum().ToString();
    }

    private static int FindNumberForSignal(IoPair io)
    {
        var mapping = MapSignalsToNumbers(io.Input);
        var value = 0;
        for (var i = 0; i < io.Output.Values.Length; i++)
        {
            var num = Array.IndexOf(mapping, mapping.Single(x => x.SetEquals(io.Output.Values[i])));
            value += num * (int)Math.Pow(10, io.Output.Values.Length - i - 1);
        }

        return value;
    }

    private static HashSet<char>[] MapSignalsToNumbers(Input input)
    {
        var result = new HashSet<char>[10];

        var values = input.Values.OrderBy(x => x.Length).ToArray();
        result[1] = new HashSet<char>(values[0]);
        result[7] = new HashSet<char>(values[1]);
        result[4] = new HashSet<char>(values[2]);
        result[8] = new HashSet<char>(values[^1]);

        var ab = result[1];
        ISet<char> d = result[7].Except(result[1]).ToHashSet();
        ISet<char> ef = result[4].Except(result[1]).ToHashSet();
        ISet<char> gc = result[8].Except(result[4].Union(d)).ToHashSet();

        ISet<char> c = values.Where(x => x.Length == 6).Select(x => x.Except(result[4].Union(d)))
            .Single(x => x.Count() == 1).ToHashSet();
        ISet<char> g = gc.Except(c).ToHashSet();
        ISet<char> e = values.Where(x => x.Length == 6).Select(x => x.Except(result[7].Union(gc)))
            .Single(x => x.Count() == 1).ToHashSet();
        ISet<char> f = ef.Except(e).ToHashSet();

        ISet<char> a = values.Where(x => x.Length == 5).Select(x => x.Except(gc.Union(f).Union(d)))
            .Single(x => x.Count() == 1).ToHashSet();
        ISet<char> b = ab.Except(a).ToHashSet();

        result[0] = result[8].Except(f).ToHashSet();
        result[2] = result[8].Except(e).Except(b).ToHashSet();
        result[3] = result[8].Except(e).Except(g).ToHashSet();
        result[5] = result[8].Except(a).Except(g).ToHashSet();
        result[6] = result[8].Except(a).ToHashSet();
        result[9] = result[8].Except(g).ToHashSet();

        return result;
    }

    private record Input(string[] Values);

    private record Output(string[] Values);

    private record IoPair(Input Input, Output Output);
}