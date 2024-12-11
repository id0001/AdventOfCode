using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(11)]
public class Challenge11(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var stones = await inputReader.ReadLineAsync<long>(11, ' ').ToListAsync();

        var cache = new Dictionary<(long, int), long>();
        return stones.Sum(n => Blink(n, 25, cache)).ToString();
    }

    [Part2]
     public async Task<string> Part2Async()
     {
         var stones = await inputReader.ReadLineAsync<long>(11, ' ').ToListAsync();

         var cache = new Dictionary<(long, int), long>();
         return stones.Sum(n => Blink(n, 75, cache)).ToString();
     }

    private long Blink(long number, int remaining, Dictionary<(long, int), long> cache)
    {
        if(cache.ContainsKey((number, remaining)))
            return cache[(number, remaining)];

        if (remaining == 1)
            return Blink(number).Length;

        var blink = Blink(number);
        var sum = blink.Select(n => Blink(n, remaining - 1, cache)).Sum();
        cache.TryAdd((number, remaining), sum);
        return sum;
    }

    private long[] Blink(long number)
    {
        if (number == 0)
            return [1];

        var str = number.ToString();
        if (str.Length % 2 != 0) return [number * 2024];
        
        var half = str.Length / 2;
        var n1 = long.Parse(str[..half]);
        var n2 = long.Parse(str[half..]);
        return [n1, n2];
    }
}
