using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var expanded = await ReadInputAsync(inputReader);

        var empty = expanded.IndexOf(-1);
        var block = expanded.FindLastIndex(x => x != -1);

        while (empty < block)
        {
            expanded[empty] = expanded[block];
            expanded[block] = -1;
            while (expanded[empty] != -1)
                empty++;

            while (expanded[block] == -1)
                block--;
        }

        return Checksum(expanded).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var expanded = await ReadInputAsync(inputReader);

        for (var blockIndex = expanded.Count - 1; blockIndex > 0; blockIndex--)
        {
            if (expanded[blockIndex] == -1)
                continue;

            int blockEnd = blockIndex;
            int id = expanded[blockIndex];
            while (blockIndex > 0 && expanded[blockIndex - 1] == id)
                blockIndex--;

            int size = blockEnd - blockIndex + 1;
            if (TryFindEmptyBlock(expanded, blockIndex, size, out var emptyIndex))
            {
                for (var i = 0; i < size; i++)
                {
                    expanded[blockIndex + i] = -1;
                    expanded[emptyIndex + i] = id;
                }
            }
        }

        return Checksum(expanded).ToString();
    }

    private static bool TryFindEmptyBlock(List<int> list, int blockIndex, int sizeNeeded, out int idx)
    {
        int start = -1;
        for (var i = 0; i < blockIndex; i++)
        {
            if (list[i] != -1)
                continue;

            start = i;
            while (list[i + 1] == -1)
                i++;

            if (i - start + 1 >= sizeNeeded)
            {
                idx = start;
                return true;
            }
        }

        idx = -1;
        return false;
    }

    private static async Task<List<int>> ReadInputAsync(IInputReader inputReader)
    {
        var expanded = new List<int>();
        var s = 0;
        var id = 0;
        await foreach (var c in inputReader.ReadLineAsync(9))
        {
            var i = c.AsInteger();
            for (var j = 0; j < i; j++)
            {
                if (s == 0)
                    expanded.Add(id);
                else
                    expanded.Add(-1);
            }

            if (s == 0)
                id++;

            s = (s + 1).Mod(2);
        }

        return expanded;
    }

    private static long Checksum(List<int> list)
    {
        long sum = 0;
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i] == -1)
                continue;

            sum += (long)list[i] * i;
        }

        return sum;
    }
}
