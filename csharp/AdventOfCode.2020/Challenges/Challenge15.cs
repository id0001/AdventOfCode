using AdventOfCode.Core;

namespace AdventOfCode2020.Challenges;

[Challenge(15)]
public class Challenge15
{
    private static readonly int[] StartingNumbers = { 6, 13, 1, 15, 2, 0 };

    [Part1]
    public string Part1()
    {
        return RunGame(StartingNumbers, 2020).ToString();
    }

    [Part2]
    public string Part2()
    {
        return RunGame(StartingNumbers, 30000000).ToString();
    }

    private static int RunGame(IReadOnlyList<int> startingNumbers, int turns)
    {
        var lookup = new Dictionary<int, int>();

        for (var i = 0; i < startingNumbers.Count - 1; i++)
            lookup.Add(startingNumbers[i], i);

        var last = 0;

        for (var i = startingNumbers.Count; i < turns; i++)
        {
            if (!lookup.ContainsKey(last))
            {
                lookup.Add(last, i - 1);
                last = 0;
            }
            else
            {
                var diff = i - 1 - lookup[last];
                lookup[last] = i - 1;
                last = diff;
            }
        }

        return last;
    }
}