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

        var (a, b) = (0, 1);
        var list = new List<int> { 3, 7 };

        for (var i = 0; list.Count < input + 10; i++)
        {
            var sum = list[a] + list[b];

            if (sum >= 10)
                list.Add(sum / 10); // First digit
            list.Add(sum % 10); // Second or only digit

            a = (a + 1 + list[a]) % list.Count;
            b = (b + 1 + list[b]) % list.Count;
        }

        return string.Concat(list[^10..]);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = (await inputReader.ReadNumberAsync<int>(14)).EnumerateDigits().ToArray();

        var (a, b) = (0, 1);
        var list = new List<int> { 3, 7 };

        while (true)
        {
            var sum = list[a] + list[b];

            if (sum >= 10)
                list.Add(sum / 10); // First digit
            list.Add(sum % 10); // Second or only digit

            a = (a + 1 + list[a]) % list.Count;
            b = (b + 1 + list[b]) % list.Count;

            if (list.Count > input.Length && (sum >= 10 ? list[^2] : list[^1]) == input[^1] && list[^(input.Length + 1)..^1].SequenceEqual(input))
                return (list.Count - input.Length - 1).ToString();
        }
    }
}