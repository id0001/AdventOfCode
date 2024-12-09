using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode2024.Challenges;

[Challenge(9)]
public class Challenge09(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var expanded = await ReadInputAsync(inputReader);

        var bi = expanded.Count - 1;
        while (bi > 0 && expanded[bi] == -1)
            bi--;

        for (var ei = 0; ei < bi; ei++)
        {
            if (expanded[ei] != -1)
                continue;

            expanded[ei] = expanded[bi];
            expanded[bi] = -1;
            while (bi > ei && expanded[bi] == -1)
                bi--;
        }

        return Checksum(expanded).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var expanded = await ReadInputAsync(inputReader);

        int emptySearchFromIndex = 0;
        for(var bi = expanded.Count-1; bi > 0; bi--)
        {
            if (expanded[bi] == -1)
                continue;

            var id = expanded[bi];
            var block = GetMemoryBlock(expanded, bi);
            bi = block.Start;

            if(TryGetFirstAvailableEmptyIndex(expanded, emptySearchFromIndex, block, out var ei, out var isFirstEmptySpace))
            {
                for (var i = 0; i < block.Size; i++)
                {
                    expanded[bi + i] = -1;
                    expanded[ei + i] = id;
                }

                if(isFirstEmptySpace)
                {
                    emptySearchFromIndex = ei + block.Size;
                }
            }
        }

        return Checksum(expanded).ToString();
    }

    private static Block GetMemoryBlock(List<int> list, int index)
    {
        int end = index;
        int id = list[index];
        while (index > 0 && list[index - 1] == id)
            index--;

        return new Block(id, index, end - index + 1);
    }

    private static bool TryGetFirstAvailableEmptyIndex(List<int> list, int emptySearchFromIndex, Block block, out int idx, out bool isFirstEmptySpace)
    {
        isFirstEmptySpace = true;
        int start = -1;
        for (var i = emptySearchFromIndex; i < block.Start; i++)
        {
            if (list[i] != -1)
                continue;

            start = i;
            while (list[i + 1] == -1)
                i++;

            if (i - start + 1 >= block.Size)
            {
                idx = start;
                return true;
            }

            isFirstEmptySpace = false;
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

    private readonly record struct Block(int Id, int Start, int Size);
}
