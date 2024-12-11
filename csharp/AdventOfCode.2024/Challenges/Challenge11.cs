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

    private static long Blink(long number, int remaining, Dictionary<(long, int), long> cache)
    {
        if (cache.ContainsKey((number, remaining)))
            return cache[(number, remaining)];

        var digits = number.CountDigits();

        if (remaining == 1)
            return digits % 2 == 0 ? 2 : 1;

        if (number == 0)
        {
            cache.TryAdd((number, remaining), Blink(1, remaining - 1, cache));
            return cache[(number, remaining)];
        }

        if (digits % 2 != 0)
        {
            cache.TryAdd((number, remaining), Blink(number * 2024, remaining - 1, cache));
            return cache[(number, remaining)];
        }

        long divisor = (long)Math.Pow(10, digits / 2);
        var n1 = Blink(number / divisor, remaining - 1, cache);
        var n2 = Blink(number % divisor, remaining - 1, cache);
        cache.TryAdd((number, remaining), n1 + n2);
        return cache[(number, remaining)];
    }
}
