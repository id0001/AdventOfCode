using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = ParseInput(await inputReader.ReadAllTextAsync(19));

        var set = new HashSet<string>();
        foreach (var key in input.Replacements.Keys)
        foreach (var repl in input.Replacements[key])
            for (var i = 0; i < input.Sequence.Length; i++)
                if (input.Sequence[i] == key)
                {
                    var s = string.Join(string.Empty, input.Sequence[..i]) + repl +
                            string.Join(string.Empty, input.Sequence[(i + 1)..]);
                    set.Add(s);
                }

        return set.Count.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = ParseInput(await inputReader.ReadAllTextAsync(19));
        var total = input.Sequence.Length;
        var arrn = input.Sequence.Count(x => x is "Ar" or "Rn");
        var y = input.Sequence.Count(x => x == "Y");

        return (total - arrn - 2 * y - 1).ToString();
    }

    private static Input ParseInput(string text)
    {
        var nl = Environment.NewLine;
        return text.SplitBy($"{nl}{nl}")
            .Into(parts =>
            {
                return new Input(
                    parts
                        .First()
                        .SplitBy(nl)
                        .Select(x => x.SplitBy("=>"))
                        .GroupBy(x => x.First(), x => x.Second())
                        .ToDictionary(kv => kv.Key, kv => kv.ToList()),
                    ParseSequence(parts.Second()).ToArray());
            });
    }

    private static IEnumerable<string> ParseSequence(string sequence)
    {
        foreach (var w in sequence.Windowed(2))
        {
            if (char.IsLower(w[0]))
                continue;

            if (char.IsUpper(w[1]))
                yield return w[0].ToString();
            else
                yield return new string(w.ToArray());
        }
    }

    private record Input(Dictionary<string, List<string>> Replacements, string[] Sequence);
}