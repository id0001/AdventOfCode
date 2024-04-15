using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2016.Challenges;

[Challenge(8)]
public class Challenge08(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var matrix = await inputReader
            .ReadLinesAsync(8)
            .AggregateAsync(new bool[6, 50], ParseInstruction);

        return matrix.Count(x => x).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var matrix = await inputReader
            .ReadLinesAsync(8)
            .AggregateAsync(new bool[6, 50], ParseInstruction);

        return matrix.Ocr();
    }

    private bool[,] ParseInstruction(bool[,] matrix, string instruction)
    {
        if (instruction.StartsWith("rect"))
        {
            var (xlen, ylen, _) = instruction.Extract(@"(\d+)x(\d+)").As<int>();
            for (var y = 0; y < ylen; y++)
            for (var x = 0; x < xlen; x++)
                matrix[y, x] = true;
        }
        else if (instruction.StartsWith("rotate row"))
        {
            var (y, c, _) = instruction.Extract(@"y=(\d+) by (\d+)").As<int>();
            var row = matrix.GetRow(y).ToArray();
            for (var x = 0; x < row.Length; x++)
                matrix[y, Euclid.Modulus(x + c, row.Length)] = row[x];
        }
        else if (instruction.StartsWith("rotate column"))
        {
            var (x, c, _) = instruction.Extract(@"x=(\d+) by (\d+)").As<int>();
            var column = matrix.GetColumn(x).ToArray();
            for (var y = 0; y < column.Length; y++)
                matrix[Euclid.Modulus(y + c, column.Length), x] = column[y];
        }

        return matrix;
    }
}