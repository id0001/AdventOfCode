using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var text = await inputReader.ReadAllTextAsync(13);
        var nl = Environment.NewLine;

        var cols = 0;
        var rows = 0;
        foreach (var block in text.SplitBy($"{nl}{nl}").Select(line => line.SplitBy(nl).ToArray()))
        {
            var r = GetReflectedRow(block, false);
            rows += r;
            if (r == 0)
                cols += GetReflectedColumn(block, false);
        }

        return (rows * 100 + cols).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var text = await inputReader.ReadAllTextAsync(13);
        var nl = Environment.NewLine;

        var cols = 0;
        var rows = 0;
        foreach (var block in text.SplitBy($"{nl}{nl}").Select(line => line.SplitBy(nl).ToArray()))
        {
            var r = GetReflectedRow(block, true);
            rows += r;
            if (r == 0)
                cols += GetReflectedColumn(block, true);
        }

        return (rows * 100 + cols).ToString();
    }

    private int GetReflectedRow(string[] block, bool withSmudge)
    {
        for (var y = 0; y < block.Length - 1; y++)
        {
            var noReflection = false;
            var foundSmudge = false;
            for (var x = 0; x < block[y].Length; x++)
            {
                for (var s = 0; s <= Math.Min(y, block.Length - 2 - y); s++)
                    if (block[y - s][x] != block[y + 1 + s][x])
                    {
                        if (withSmudge && !foundSmudge)
                        {
                            foundSmudge = true;
                        }
                        else
                        {
                            noReflection = true;
                            break;
                        }
                    }

                if (noReflection)
                    break;
            }

            if (withSmudge && !foundSmudge)
                continue;

            if (!noReflection)
                return y + 1;
        }

        return 0;
    }

    private int GetReflectedColumn(string[] block, bool withSmudge)
    {
        for (var x = 0; x < block[0].Length - 1; x++)
        {
            var noReflection = false;
            var foundSmudge = false;
            for (var y = 0; y < block.Length; y++)
            {
                for (var s = 0; s <= Math.Min(x, block[0].Length - 2 - x); s++)
                    if (block[y][x - s] != block[y][x + 1 + s])
                    {
                        if (withSmudge && !foundSmudge)
                        {
                            foundSmudge = true;
                        }
                        else
                        {
                            noReflection = true;
                            break;
                        }
                    }

                if (noReflection)
                    break;
            }

            if (withSmudge && !foundSmudge)
                continue;

            if (!noReflection)
                return x + 1;
        }

        return 0;
    }
}