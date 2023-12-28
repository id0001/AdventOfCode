using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await ParseInput();

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

    private async Task<Input> ParseInput()
    {
        var lines = await inputReader.ReadLinesAsync(19).ToListAsync();
        var p1 = lines.Take(lines.Count - 2);
        var p2 = lines[^1];

        var sequence = new List<string>();
        var sb = new StringBuilder();
        foreach (var c in p2)
        {
            if (char.IsUpper(c) && sb.Length > 0)
            {
                sequence.Add(sb.ToString());
                sb.Clear();
            }

            sb.Append(c);
        }

        if (sb.Length > 0)
        {
            sequence.Add(sb.ToString());
            sb.Clear();
        }

        var replacements = new Dictionary<string, List<string>>();
        foreach (var rep in p1)
        {
            var split = rep.Split(" => ", StringSplitOptions.RemoveEmptyEntries);
            if (!replacements.ContainsKey(split[0]))
                replacements.Add(split[0], new List<string>());

            replacements[split[0]].Add(split[1]);
        }

        return new Input(replacements, sequence.ToArray());
    }

    private record Input(Dictionary<string, List<string>> Replacements, string[] Sequence);
}