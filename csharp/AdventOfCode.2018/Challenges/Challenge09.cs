using AdventOfCode.Core;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2018.Challenges;

[Challenge(9)]
public class Challenge09
{
    [Part1]
    public string Part1()
    {
        return PlayGame2(435, 71184).ToString();
    }

    [Part2]
    public string Part2()
    {
        return PlayGame2(435, 7118400).ToString();
    }

    private long PlayGame2(int playerCount, int lastmarbleWorth)
    {
        var circle = new Deque<long>([0]);
        var scores = new long[playerCount];

        foreach (var m in Enumerable.Range(1, lastmarbleWorth + 1))
            if (m % 23 == 0)
            {
                circle.Rotate(7);
                scores[m % playerCount] += m + circle.PopBack();
                circle.Rotate(-1);
            }
            else
            {
                circle.Rotate(-1);
                circle.PushBack(m);
            }

        return scores.Max();
    }
}