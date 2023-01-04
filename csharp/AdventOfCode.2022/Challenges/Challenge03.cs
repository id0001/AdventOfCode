using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2022.Challenges;

[Challenge(3)]
public class Challenge03
{
    private readonly IInputReader _inputReader;

    public Challenge03(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var itemsInBoth = new List<char>();
        await foreach (var rucksack in _inputReader.ReadLinesAsync(3))
        {
            var set1 = rucksack[..(rucksack.Length / 2)].ToHashSet();
            var set2 = rucksack[(rucksack.Length / 2)..].ToHashSet();
            var inBoth = set1.Intersect(set2);
            itemsInBoth.Add(inBoth.First());
        }

        return itemsInBoth.Select(GetPriority).Sum().ToString();
    }
    
    [Part2]
    public async Task<string> Part2Async()
    {
        var badges = new List<char>();
        var chunks = (await _inputReader.ReadLinesAsync(3).ToListAsync()).Chunk(3);
        foreach (var elf in chunks)
        {
            var inAll = elf[0].Intersect(elf[1].Intersect(elf[2]));
            badges.Add(inAll.First());
        }

        return badges.Select(GetPriority).Sum().ToString();
    }

    private static int GetPriority(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
}