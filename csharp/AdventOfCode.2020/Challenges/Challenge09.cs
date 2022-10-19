using AdventOfCode.Lib.Collections;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(9)]
public class Challenge09
{
    private readonly IInputReader _inputReader;

    public Challenge09(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async()
    {
        var buffer = new CircularBuffer<long>(25);
        var index = 0;

        await foreach (var value in _inputReader.ReadLinesAsync<long>(9))
        {
            if (index >= 25)
                if (!Validate(buffer, value))
                    return value.ToString();

            buffer.Push(value);
            index++;
        }

        return null;
    }

    [Part2]
    public async Task<string?> Part2Async()
    {
        const long target = 1930745883L;

        var buffer = new CircularBuffer<long>(25);
        var index = 0;

        await foreach (var value in _inputReader.ReadLinesAsync<long>(9))
        {
            if (index >= 25)
            {
                var set = FindContiguousSet(buffer, target);
                if (set.Length > 0)
                    return (set.Min() + set.Max()).ToString();
            }

            buffer.Push(value);
            index++;
        }

        return null;
    }

    private static bool Validate(CircularBuffer<long> buffer, long value)
    {
        for (var y = 0; y < buffer.Count; y++)
        for (var x = y; x < buffer.Count; x++)
            if (value == buffer[x] + buffer[y])
                return true;

        return false;
    }

    private static long[] FindContiguousSet(CircularBuffer<long> buffer, long target)
    {
        var sum = buffer[0] + buffer[1];

        if (sum == target)
            return buffer.Take(2).ToArray();

        for (var i = 2; i < buffer.Count; i++)
        {
            sum += buffer[i];
            if (sum == target) return buffer.Take(i + 1).ToArray();

            if (sum > target)
                break;
        }

        return Array.Empty<long>();
    }
}