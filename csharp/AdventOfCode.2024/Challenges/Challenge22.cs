using AdventOfCode.Core;
using AdventOfCode.Core.IO;

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
            var prices = new long[2000];
            var changes = new long[2000];
            long previous = 0;
            var seenSequences = new HashSet<(long,long,long,long)>();

            for (var i = 0; i < 2000; i++)
            {
                secret = CalculateSecret(secret);
                prices[i] = secret % 10;
                changes[i] = (secret % 10) - previous;
                previous = secret % 10;

                if (i < 3) 
                    continue;
                
                var key = (changes[i - 3], changes[i - 2], changes[i - 1], changes[i]);
                if (!seenSequences.Add(key)) 
                    continue;
                
                if (!total.TryAdd(key, prices[i]))
                    total[key] += prices[i];
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
