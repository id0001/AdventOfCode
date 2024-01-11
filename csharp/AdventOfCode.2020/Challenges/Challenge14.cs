using System.Collections;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(14)]
public class Challenge14(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var memory = new Dictionary<ulong, ulong>();

        string? mask = null;
        await foreach (var line in InputReader.ReadLinesAsync(14))
            if (line.StartsWith("mask = "))
            {
                mask = line["mask = ".Length..];
            }
            else
            {
                var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
                var addr = ulong.Parse(match.Groups[1].Value);
                var value = ulong.Parse(match.Groups[2].Value);

                memory.TryAdd(addr, 0);

                memory[addr] = ApplyMask(value, mask!);
            }

        return memory.Sum(kv => (long) kv.Value).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var memory = new Dictionary<ulong, ulong>();

        string? mask = null;
        await foreach (var line in InputReader.ReadLinesAsync(14))
            if (line.StartsWith("mask = "))
            {
                mask = line["mask = ".Length..];
            }
            else
            {
                var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)$");
                var addr = ulong.Parse(match.Groups[1].Value);
                var value = ulong.Parse(match.Groups[2].Value);

                foreach (var realAddr in EnumerateAddresses(addr, mask!))
                {
                    memory.TryAdd(realAddr, 0);

                    memory[realAddr] = value;
                }
            }

        return memory.Sum(kv => (long) kv.Value).ToString();
    }

    private static ulong ApplyMask(ulong value, string mask)
    {
        for (var i = 0; i < mask.Length; i++)
            value = mask[i] switch
            {
                '0' => value & MaskFor(i),
                '1' => (value & MaskFor(i)) + (1ul << (35 - i)),
                _ => value
            };

        return value;
    }

    private static IEnumerable<ulong> EnumerateAddresses(ulong value, string mask)
    {
        var floatingIndices = new List<int>();
        for (var i = 0; i < mask.Length; i++)
            switch (mask[i])
            {
                case '1':
                    value = (value & MaskFor(i)) + (1ul << (35 - i));
                    break;
                case 'X':
                    value = value & MaskFor(i);
                    floatingIndices.Add(i);
                    break;
            }

        var count = (int) Math.Pow(2, floatingIndices.Count);
        for (var i = 0; i < count; i++)
        {
            var bits = new BitArray(new[] {i});
            yield return AddBitVal(bits, floatingIndices, value, 0);
        }
    }

    private static ulong AddBitVal(BitArray bits, IList<int> indices, ulong value, int i)
    {
        return i >= bits.Count
            ? value
            : AddBitVal(bits, indices, value + (bits[i] ? 1ul << (35 - indices[i]) : 0ul), i + 1);
    }

    private static ulong MaskFor(int i)
    {
        const ulong realMask = 0x0000_000F_FFFF_FFFF;
        return realMask - (1ul << (35 - i));
    }
}