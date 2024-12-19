using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(19)]
public class Challenge19(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (available, wanted) = await inputReader.ParseTextAsync(19, ParseInput);
        return wanted.Count(w => IsPossible(w, available)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (available, wanted) = await inputReader.ParseTextAsync(19, ParseInput);
        return wanted.Sum(w =>
        {
            var cache = new Dictionary<string, long>();
            return CountPossibilities(w,  available, cache);
        }).ToString();
    }

    private static bool IsPossible(string wanted, IList<string> available)
    {
        if (wanted.Length == 0)
            return true;

        return available
            .Where(wanted.StartsWith)
            .Any(test => IsPossible(wanted[test.Length..], available));
    }

    private static long CountPossibilities(string wanted, IList<string> available, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(wanted, out var cv))
            return cv;

        if (wanted.Length == 0)
            return 1;
        
        var count = 0L;
        foreach (var test in available)
        {
            if (!wanted.StartsWith(test)) 
                continue;
            
            var candidate = wanted[test.Length..];
            cache.TryAdd(candidate, CountPossibilities(candidate, available, cache));
            count += cache[candidate];
        }

        return count;
    }
    
    private static (IList<string> Available, IList<string> Wanted) ParseInput(string input)
    {
        var paragraphs = input.SelectParagraphs();
        var available = paragraphs.First().SplitBy(",").OrderByDescending(x => x.Length).ToList();
        var wanted = paragraphs.Second().SelectLines();

        return (available, wanted);
    }
}
