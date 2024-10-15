using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(14)]
public class Challenge14(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadNumberAsync<int>(14);

        var indices = new[] { 0, 1 };
        var list = new List<int> { 3, 7 };

        for (var i = 0; list.Count < input + 10; i++)
        {
            var sum = list[indices[0]] + list[indices[1]];
            foreach (var c in sum.ToString())
                list.Add((int)char.GetNumericValue(c));

            indices[0] = (indices[0] + 1 + list[indices[0]]).Mod(list.Count);
            indices[1] = (indices[1] + 1 + list[indices[1]]).Mod(list.Count);
        }

        return string.Concat(list[^10..]);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadAllTextAsync(14);
        var inputNumbers = input.Select(c => (int)char.GetNumericValue(c)).ToArray();

        var indices = new[] { 0, 1 };
        var list = new List<int> { 3, 7 };

        while (true)
        {
            var sum = (list[indices[0]] + list[indices[1]]).ToString();

            foreach (var c in sum)
                list.Add((int)char.GetNumericValue(c));

            indices[0] = (indices[0] + 1 + list[indices[0]]).Mod(list.Count);
            indices[1] = (indices[1] + 1 + list[indices[1]]).Mod(list.Count);

            if (list.Count > input.Length && sum[0] == input[^1])
            {
                if (list[^(input.Length + 1)..^1].SequenceEqual(inputNumbers))
                    return (list.Count - input.Length - 1).ToString();
            }
        }
    }
}