using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions.Linq;

namespace AdventOfCode2017.Challenges;

[Challenge(10)]
public class Challenge10(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var numbers = Enumerable.Range(0, 256).ToArray();
        var i = 0;
        var skip = 0;

        await foreach (var length in inputReader.ReadLineAsync<int>(10, ','))
        {
            numbers = numbers
                .Cycle() // make circle
                .Skip(i) // move i to 0
                .Take(length) // take the length
                .Reverse() // reverse the sequence
                .Concat(
                    numbers
                        .Cycle()
                        .Skip(i + length)
                        .Take(numbers.Length - length)
                ) // add the remaining numbers
                .Cycle() // make circle
                .Skip(numbers.Length - i) // move back to original index
                .Take(numbers.Length) // make original length
                .ToArray();

            i = (i + length + skip).Mod(numbers.Length);
            skip++;
        }

        return numbers.Take(2).Product().ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var sparseHash = Enumerable.Range(0, 256).ToArray();
        var i = 0;
        var skip = 0;

        foreach (var round in Enumerable.Range(0, 64))
        {
            await foreach (var length in inputReader.ReadLineAsync(10).Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }.ToAsyncEnumerable()))
            {
                sparseHash = sparseHash
                    .Cycle() // make circle
                    .Skip(i) // move i to 0
                    .Take(length) // take the length
                    .Reverse() // reverse the sequence
                    .Concat(
                        sparseHash
                            .Cycle()
                            .Skip(i + length)
                            .Take(sparseHash.Length - length)
                    ) // add the remaining numbers
                    .Cycle() // make circle
                    .Skip(sparseHash.Length - i) // move back to original index
                    .Take(sparseHash.Length) // make original length
                    .ToArray();

                i = (i + length + skip).Mod(sparseHash.Length);
                skip++;
            }
        }

        return string.Join("", sparseHash.Chunk(16).Select(x => x.Xor().ToHexString()));
    }
}