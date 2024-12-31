using AdventOfCode.Core;
using AdventOfCode.Core.IO;

// ReSharper disable RedundantAssignment

namespace AdventOfCode2024.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var input = await inputReader.ReadLinesAsync<long>(22).ToListAsync();
        long sum = 0;

        for (var n = 0; n < input.Count; n++)
        {
            var secret = input[n];
            for (var i = 0; i < 2000; i++)
                secret = CalculateSecret(secret);

            sum += secret;
        }

        return sum.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var total = new Dictionary<(long, long, long, long), long>();

        await foreach (var input in inputReader.ReadLinesAsync<long>(22))
        {
            var secret = input;
            long previous = 0;
            var seenSequences = new HashSet<(long, long, long, long)>();

            var (a, b, c, d) = (0L, 0L, 0L, 0L); // changes
            for (var i = 0; i < 2000; i++)
            {
                secret = CalculateSecret(secret);
                var price = secret % 10;
                (a, b, c, d) = (b, c, d, price - previous);
                previous = price;

                if (i < 3)
                    continue;

                if (!seenSequences.Add((a, b, c, d)))
                    continue;

                if (!total.TryAdd((a, b, c, d), price))
                    total[(a, b, c, d)] += price;
            }
        }

        return total.Values.Max().ToString();
    }

    private static long CalculateSecret(long secret)
    {
        var a = 64L * secret;
        secret ^= a;
        secret %= 16777216L;

        var b = secret / 32L;
        secret ^= b;
        secret %= 16777216L;

        var c = secret * 2048;
        secret ^= c;
        secret %= 16777216L;

        return secret;
    }
}