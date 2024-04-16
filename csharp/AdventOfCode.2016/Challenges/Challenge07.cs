using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(7)]
public class Challenge07(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        return await inputReader
            .ReadLinesAsync(7)
            .Where(SupportsTls)
            .CountAsync()
            .ToStringAsync();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        return await inputReader
            .ReadLinesAsync(7)
            .Where(SupportsSsl)
            .CountAsync()
            .ToStringAsync();
    }

    private bool SupportsTls(string line)
    {
        var hasAbba = false;
        var inHypernetSequence = false;
        var iSinceStart = 0;

        for (var i = 0; i < line.Length; i++)
            switch (line[i])
            {
                case '[':
                    inHypernetSequence = true;
                    iSinceStart = 0;
                    break;
                case ']':
                    inHypernetSequence = false;
                    iSinceStart = 0;
                    break;
                default:
                    if (iSinceStart++ < 3)
                        continue;

                    if (line[i - 3] == line[i] && line[i - 2] == line[i - 1] && line[i] != line[i - 1])
                    {
                        if (inHypernetSequence)
                            return false;
                        hasAbba = true;
                    }

                    break;
            }

        return hasAbba;
    }

    private bool SupportsSsl(string line)
    {
        var abaSequences = new HashSet<string>();
        var babSequences = new HashSet<string>();
        var inHypernetSequence = false;
        var iSinceStart = 0;

        for (var i = 0; i < line.Length; i++)
            switch (line[i])
            {
                case '[':
                    inHypernetSequence = true;
                    iSinceStart = 0;
                    break;
                case ']':
                    inHypernetSequence = false;
                    iSinceStart = 0;
                    break;
                default:
                    if (iSinceStart++ < 2)
                        continue;

                    if (line[i] == line[i - 2] && line[i] != line[i - 1])
                    {
                        if (inHypernetSequence)
                            babSequences.Add(new string([line[i - 1], line[i], line[i - 1]]));
                        else
                            abaSequences.Add(line[(i - 2)..(i + 1)]);

                        if (abaSequences.Intersect(babSequences).Count() > 0)
                            return true;
                    }

                    break;
            }

        return false;
    }
}