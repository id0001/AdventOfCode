using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2017.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadAllTextAsync(9);

        var score = 0;
        var depth = 0;
        var inGarbage = false;
        var shouldIgnore = false;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case '<' when !inGarbage && !shouldIgnore:
                    inGarbage = true;
                    break;
                case '>' when inGarbage && !shouldIgnore:
                    inGarbage = false;
                    break;
                case '{' when !inGarbage && !shouldIgnore:
                    depth++;
                    break;
                case '}' when !inGarbage && !shouldIgnore:
                    score += depth;
                    depth--;
                    break;
                case '!' when !shouldIgnore:
                    shouldIgnore = true;
                    continue;
            }

            shouldIgnore = false;
        }

        return score.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var input = await inputReader.ReadAllTextAsync(9);

        var score = 0;
        var inGarbage = false;
        var shouldIgnore = false;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case '<' when !inGarbage && !shouldIgnore:
                    inGarbage = true;
                    break;
                case '>' when inGarbage && !shouldIgnore:
                    inGarbage = false;
                    break;
                case '{' when !inGarbage && !shouldIgnore:
                    break;
                case '}' when !inGarbage && !shouldIgnore:
                    break;
                case '!' when !shouldIgnore:
                    shouldIgnore = true;
                    continue;
                default:
                    if (!shouldIgnore && inGarbage)
                        score++;
                    break;
            }

            shouldIgnore = false;
        }

        return score.ToString();
    }
}