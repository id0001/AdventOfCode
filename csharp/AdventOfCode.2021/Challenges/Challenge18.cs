using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(18)]
public class Challenge18
{
    private const string NumberNumberPattern = @"^\[(\d),(\d)\]$";
    private const string NumberPairPattern = @"^\[(\d),(.+)\]$";
    private const string PairNumberPattern = @"^\[(.+),(\d)\]$";

    private readonly IInputReader _inputReader;

    public Challenge18(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await _inputReader.ReadLinesAsync(18).ToArrayAsync();
        var number = lines[0];
        for (var i = 1; i < lines.Length; i++)
        {
            number = Add(number, lines[i]);
            number = Reduce(number);
        }

        return GetMagnitude(number).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var lines = await _inputReader.ReadLinesAsync(18).ToArrayAsync();
        var maxMagnitude = 0;
        for (var y = 0; y < lines.Length; y++)
        {
            var line1 = lines[y];
            for (var x = 0; x < lines.Length; x++)
            {
                var line2 = lines[x];

                var num1 = Add(line1, line2);
                var mag = GetMagnitude(Reduce(num1));
                if (mag > maxMagnitude)
                    maxMagnitude = mag;

                var num2 = Add(line2, line1);
                mag = GetMagnitude(Reduce(num2));
                if (mag > maxMagnitude)
                    maxMagnitude = mag;
            }
        }

        return maxMagnitude.ToString();
    }

    private string Add(string number1, string number2)
    {
        return $"[{number1},{number2}]";
    }

    private string Reduce(string line)
    {
        var result = line;
        while (true)
        {
            if (!TryExplode(result, out result) && !TrySplit(result, out result))
                return result;
        }
    }

    private bool TryExplode(string line, out string result)
    {
        var depth = 0;
        var lastNumberIndex = -1;
        var lastNumberLength = 0;
        for (var i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case '[' when depth == 4:
                    var m = Regex.Match(line.Substring(i), @"^\[(\d+),(\d+)\]");
                    var n1 = int.Parse(m.Groups[1].Value);
                    var n2 = int.Parse(m.Groups[2].Value);
                    var sb = new StringBuilder();
                    sb.Append(line.Substring(0, i));
                    if (lastNumberIndex > 0)
                    {
                        var num = int.Parse(line.Substring(lastNumberIndex, lastNumberLength));
                        sb.Remove(lastNumberIndex, lastNumberLength);
                        sb.Insert(lastNumberIndex, n1 + num);
                    }

                    sb.Append('0');
                    i += m.Length;

                    for (; i < line.Length && !char.IsDigit(line[i]); i++)
                        sb.Append(line[i]);

                    if (i != line.Length)
                    {
                        var num = GetNumber(line.Substring(i));
                        sb.Append(int.Parse(num) + n2);
                        i += num.Length;
                    }

                    sb.Append(line.Substring(i));
                    result = sb.ToString();
                    return true;
                case '[':
                    depth++;
                    break;
                case ']':
                    depth--;
                    break;
                case var c when char.IsDigit(c):
                    lastNumberIndex = i;
                    lastNumberLength = Regex.Match(line.Substring(i), @"^(\d+)").Length;
                    i += lastNumberLength - 1;
                    break;
            }
        }

        result = line;
        return false;
    }

    private bool TrySplit(string line, out string result)
    {
        for (var i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case var c when char.IsDigit(c):
                    var num = GetNumber(line.Substring(i));
                    if (num.Length >= 2)
                    {
                        var number = int.Parse(num);
                        var l = number / 2;
                        var r = number - l;
                        var sb = new StringBuilder();
                        sb.Append(line.AsSpan(0, i));
                        sb.Append($"[{l},{r}]");
                        i += num.Length;
                        sb.Append(line.AsSpan(i));
                        result = sb.ToString();
                        return true;
                    }

                    break;
            }
        }

        result = line;
        return false;
    }

    private static string GetNumber(string line)
    {
        var m = Regex.Match(line, @"^(\d+)");
        return m.Groups[1].Value;
    }

    private static int GetMagnitude(string line)
    {
        var match = Regex.Match(line, NumberNumberPattern);
        if (match.Success) return 3 * int.Parse(match.Groups[1].Value) + 2 * int.Parse(match.Groups[2].Value);

        match = Regex.Match(line, NumberPairPattern);
        if (match.Success) return 3 * int.Parse(match.Groups[1].Value) + 2 * GetMagnitude(match.Groups[2].Value);

        match = Regex.Match(line, PairNumberPattern);
        if (match.Success) return 3 * GetMagnitude(match.Groups[1].Value) + 2 * int.Parse(match.Groups[2].Value);

        // Find splitting point
        var splitIndex = FindSplitIndex(line);
        return 3 * GetMagnitude(line.Substring(1, splitIndex - 1)) +
               2 * GetMagnitude(line.Substring(splitIndex + 1, line.Length - splitIndex - 2));
    }

    private static int FindSplitIndex(string line)
    {
        var bracket = 0;
        for (var i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case ',' when bracket == 1:
                    return i;
                case '[':
                    bracket++;
                    break;
                case ']':
                    bracket--;
                    break;
            }
        }

        return -1;
    }
}