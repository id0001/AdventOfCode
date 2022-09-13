using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2015.Challenges;

[Challenge(8)]
public class Challenge08
{
    private readonly IInputReader _inputReader;

    public Challenge08(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var c1 = 0;
        var c2 = 0;
        await foreach (var (a,b) in _inputReader.ParseLinesAsync(8, ParseLine))
        {
            c1 += a;
            c2 += b;
        }

        return (c1 - c2).ToString();
    }
    
    [Part2]
    public async Task<string?> Part2Async()
    {
        var c1 = 0;
        var c2 = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(8))
        {
            c1 += line.Length + line.Count(c => c is '"' or '\\') + 2;
            c2 += line.Length;
        }

        return (c1 - c2).ToString();
    }

    private (int,int) ParseLine(string line)
    {
        var c1 = line.Length;

        var c2 = 0;
        var escaped = false;
        for (var i = 1; i < line.Length-1; i++)
        {
            if (line[i] == '\\' && !escaped)
            {
                escaped = true;
                continue;
            }

            if (escaped && line[i] == 'x')
            {
                i += 2;
            }

            c2++;
            escaped = false;
        }

        return (c1, c2);
    }
}