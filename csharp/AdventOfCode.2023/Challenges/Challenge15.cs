using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(15)]
public class Challenge15(IInputReader inputReader)
{
    [Part1]
    public async Task<string?> Part1Async()
    {
        return await inputReader.ReadLineAsync(15, ',').SumAsync(Hash).ToStringAsync();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var boxes = Enumerable.Range(0, 256).Select(_ => new List<Lens>()).ToArray();

        await foreach (var part in inputReader.ReadLineAsync(15, ','))
            if (part.Contains('='))
            {
                var lens = part.SplitBy("=")
                    .Into(parts => new Lens(parts.First(), parts.Second().As<int>()));
                var hash = Hash(lens.Label);
                var index = boxes[hash].IndexOf(lens);
                if (index < 0)
                    boxes[hash].Add(lens);
                else
                    boxes[hash][index] = lens;
            }
            else
            {
                var lens = new Lens(part[..^1], 0);
                var hash = Hash(lens.Label);
                var index = boxes[hash].IndexOf(lens);
                if (index >= 0)
                    boxes[hash].RemoveAt(index);
            }

        return boxes
            .SelectMany((b, bi) => b
                .Select((l, li) => (bi + 1) * (li + 1) * l.FocalLength))
            .Sum()
            .ToString();
    }

    private static int Hash(string s) => s.Aggregate(0, (a, b) => (b + a) * 17 % 256);

    private sealed record Lens(string Label, int FocalLength)
    {
        public bool Equals(Lens? other) => other is not null && other.Label == Label;

        public override int GetHashCode() => Label.GetHashCode();
    }
}