using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2022.Challenges;

[Challenge(4)]
public class Challenge04
{
    private readonly IInputReader _inputReader;

    public Challenge04(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var sum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(4))
        {
            var pair = line.Split(',');
            var elf1 = pair[0].Split('-');
            var elf2 = pair[1].Split('-');
            var (from1, to1) = (int.Parse(elf1[0]), int.Parse(elf1[1]));
            var (from2, to2) = (int.Parse(elf2[0]), int.Parse(elf2[1]));

            if ((from1 >= from2 && to1 <= to2) || (from2 >= from1 && to2 <= to1))
                sum++;
        }

        return sum.ToString();
    }
    
    [Part2]
    public async Task<string> Part2Async()
    {
        var sum = 0;
        await foreach (var line in _inputReader.ReadLinesAsync(4))
        {
            var pair = line.Split(',');
            var elf1 = pair[0].Split('-');
            var elf2 = pair[1].Split('-');
            var (from1, to1) = (int.Parse(elf1[0]), int.Parse(elf1[1]));
            var (from2, to2) = (int.Parse(elf2[0]), int.Parse(elf2[1]));

            if ((from2 >= from1 && from2 <= to1) || (from1 >= from2 && from1 <= to2))
                sum++;
        }

        return sum.ToString();
    }
}