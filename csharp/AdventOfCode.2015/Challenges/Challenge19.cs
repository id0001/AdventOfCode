using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = ParseInput(await InputReader.ReadAllTextAsync(19));

        var set = new HashSet<string>();
        foreach (var kv in input.Replacements)
            foreach (var repl in kv.Value)
                for (var i = 0; i < input.Sequence.Length; i++)
                    if (input.Sequence[i] == kv.Key)
                    {
                        var s = string.Join("", input.Sequence.Take(i - 1)) + repl +
                                string.Join("", input.Sequence.Skip(i + 1));
                        set.Add(s);
                    }

        return set.Count.ToString();
    }

    private void Replace(Dictionary<string, List<string>> replacements, string node, int i, string[] sequence,
        HashSet<(string, int)> parts, HashSet<string> results)
    {
        if (parts.Contains((node, i)))
            return;

        if (i == sequence.Length)
        {
            results.Add(node);
            return;
        }

        if (!replacements.ContainsKey(sequence[i]))
        {
            node += sequence[i];
            parts.Add((node, i));
            Replace(replacements, node, i + 1, sequence, parts, results);
        }
        else
        {
            foreach (var replacement in replacements[sequence[i]])
            {
                node += replacement;
                parts.Add((node, i));
                Replace(replacements, node, i + 1, sequence, parts, results);
            }
        }
    }

    // [Part2]
    public async Task<string> Part2Async()
    {
        return string.Empty;
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