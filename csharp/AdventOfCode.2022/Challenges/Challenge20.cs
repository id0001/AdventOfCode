using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2022.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var numbers = await InputReader.ReadLinesAsync<int>(20).ToListAsync();
        var indices = Enumerable.Range(0, numbers.Count).ToList();

        for (var i = 0; i < numbers.Count; i++)
        {
            var idx = indices.IndexOf(i);
            var num = numbers[idx];
            numbers.RemoveAt(idx);
            indices.RemoveAt(idx);
            var newIndex = Euclid.Modulus(idx + num, numbers.Count);
            if (newIndex == 0)
            {
                numbers.Add(num);
                indices.Add(i);
            }
            else
            {
                numbers.Insert(newIndex, num);
                indices.Insert(newIndex, i);
            }
        }

        var idxOf0 = numbers.IndexOf(0);
        return (numbers[Euclid.Modulus(idxOf0 + 1000, numbers.Count)] +
                numbers[Euclid.Modulus(idxOf0 + 2000, numbers.Count)] +
                numbers[Euclid.Modulus(idxOf0 + 3000, numbers.Count)]).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var numbers = await InputReader.ReadLinesAsync<int>(20).Select(x => x * 811589153L).ToListAsync();
        var indices = Enumerable.Range(0, numbers.Count).ToList();

        for (var j = 0; j < 10; j++)
        for (var i = 0; i < numbers.Count; i++)
        {
            var idx = indices.IndexOf(i);
            var num = numbers[idx];
            numbers.RemoveAt(idx);
            indices.RemoveAt(idx);
            var newIndex = (int) Euclid.Modulus(idx + num, numbers.Count);
            if (newIndex == 0)
            {
                numbers.Add(num);
                indices.Add(i);
            }
            else
            {
                numbers.Insert(newIndex, num);
                indices.Insert(newIndex, i);
            }
        }

        var idxOf0 = numbers.IndexOf(0);
        return (numbers[Euclid.Modulus(idxOf0 + 1000, numbers.Count)] +
                numbers[Euclid.Modulus(idxOf0 + 2000, numbers.Count)] +
                numbers[Euclid.Modulus(idxOf0 + 3000, numbers.Count)]).ToString();
    }
}