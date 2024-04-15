using System.Text;
using AdventOfCode.Core.Resources;

namespace AdventOfCode.Core.IO;

public static class OcrExtensions
{
    public static string Ocr(this string source) => new Ocr().Parse(source);

    public static string Ocr(this bool[,] source)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++) sb.Append(source[y, x] ? '#' : '.');

            sb.AppendLine();
        }

        return sb.ToString().Ocr();
    }
}

public class Ocr
{
    private const string SmallAlphabetResourceName = "AdventOfCode.Core.Resources.alphabet_s.txt";

    public string Parse(string input)
    {
        var nl = Environment.NewLine;
        var letters = GetLetters();

        var lines = input.Replace('.', ' ').Split(nl, StringSplitOptions.RemoveEmptyEntries).Select(l => l.TrimEnd())
            .ToArray();
        var maxLength = lines.Max(l => l.Length);
        lines = lines.Select(l => l.PadRight(maxLength)).ToArray();

        var sb = new StringBuilder();
        for (var i = 0; i <= lines[0].Length / 5; i++)
        {
            var from = i * 5;
            var s = string.Join(nl, lines.Select(l => l[from..(from + 4)]));
            if (letters.TryGetValue(s, out var letter))
                sb.Append(letter);
        }

        return sb.ToString();
    }

    private Dictionary<string, char> GetLetters()
    {
        var nl = Environment.NewLine;
        var alphabet = ResourceHelper.Read(SmallAlphabetResourceName).Split($"{nl}{nl}");

        var letters = new Dictionary<string, char>();
        foreach (var part in alphabet)
        {
            var c = part[0];
            var r = string.Join(nl, Chunk(part[(nl.Length + 1)..].Replace(nl, string.Empty)));
            letters.Add(r, c);
        }

        return letters;
    }

    private IEnumerable<string> Chunk(string line)
    {
        for (var i = 0; i < line.Length - 1; i += 5)
            yield return line[i..(i + 4)];
    }
}