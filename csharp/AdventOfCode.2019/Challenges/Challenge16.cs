using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2019.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader InputReader)
{
    private static readonly int[] BasePattern = {0, 1, 0, -1};

    [Part1]
    public async Task<string> Part1Async()
    {
        var originalInput = await InputReader.ReadLineAsync(16).Select(x => int.Parse(x.ToString())).ToArrayAsync();
        for (var phase = 0; phase < 100; phase++)
        {
            var newInput = new int[originalInput.Length];
            for (var j = 0; j < newInput.Length; j++)
            for (var i = 0; i < originalInput.Length; i++)
                newInput[j] += originalInput[i] * GetPatternValueAtIndex(j, i);

            for (var i = 0; i < originalInput.Length; i++) originalInput[i] = Math.Abs(newInput[i]) % 10;
        }

        return string.Join("", originalInput.Take(8));
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var originalInput = await InputReader.ReadLineAsync(16).Select(x => int.Parse(x.ToString())).ToArrayAsync();

        var inputCount = originalInput.Length * 10000;
        var offset = int.Parse(string.Join("", originalInput.Take(7)));

        // Skip from offset
        var input = new int[inputCount - offset];
        for (var i = 0; i < input.Length; i++) input[i] = GetInput(originalInput, i + offset);

        for (var phase = 0; phase < 100; phase++)
        {
            // Looping in reverse makes this O(n) instead of o(n^2)
            var sum = 0;
            for (var j = input.Length - 1; j >= 0; j--)
            {
                sum += input[j];
                input[j] = sum % 10;
            }
        }

        return string.Join("", input.Take(8));
    }

    private static int GetInput(IReadOnlyList<int> input, int index) => input[index % input.Count];

    private static int GetPatternValueAtIndex(int outputIndex, int inputIndex)
    {
        var patternSize = (outputIndex + 1) * BasePattern.Length;
        var pi = (inputIndex + 1) % patternSize;
        var bpi = (int) Math.Floor(pi / (outputIndex + 1d));
        return BasePattern[bpi];
    }
}