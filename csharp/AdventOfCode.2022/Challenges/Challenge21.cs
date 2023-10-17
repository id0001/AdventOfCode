using System.Linq.Expressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2022.Challenges;

[Challenge(21)]
public class Challenge21
{
    private readonly IInputReader _inputReader;

    public Challenge21(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var lines = await _inputReader.ReadLinesAsync(21)
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToDictionaryAsync(kv => kv[0], kv => kv[1]);

        var result = Expression.Lambda<Func<long>>(Parse(new Dictionary<string, Expression>(), lines, "root")).Compile();
        return result().ToString();
    }

    private static Expression Parse(IDictionary<string, Expression> cache, IDictionary<string, string> map, string key)
    {
        if (cache.ContainsKey(key))
            return cache[key];
        
        var line = map[key];
        if (long.TryParse(line, out var num))
        {
            cache.Add(key, Expression.Constant(num));
        }
        else
        {
            var split = line.Split(' ');
            cache.Add(key, split[1] switch
            {
                "+" => Expression.Add(Parse(cache, map, split[0]), Parse(cache, map, split[2])),
                "-" => Expression.Subtract(Parse(cache, map, split[0]), Parse(cache, map, split[2])),
                "*" => Expression.Multiply(Parse(cache, map, split[0]), Parse(cache, map, split[2])),
                "/" => Expression.Divide(Parse(cache, map, split[0]), Parse(cache, map, split[2])),
                _ => throw new NotImplementedException()
            });
        }
        
        return cache[key];
    }
}