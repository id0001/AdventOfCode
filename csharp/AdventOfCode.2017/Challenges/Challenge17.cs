using AdventOfCode.Core;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(17)]
public class Challenge17
{
    private const int Input = 304;

    [Part1]
    public string Part1()
    {
        var list = new LinkedList<int>();
        list.AddLast(0);

        var current = list.First!;
        for (var i = 0; i < 2017; i++)
        {
            var steps = Input % list.Count;
            for (var j = 0; j < steps; j++)
                current = current.NextOrFirst();

            list.AddAfter(current, i + 1);
            current = current.NextOrFirst();
        }


        return list.Find(2017)!.NextOrFirst().Value.ToString();
    }

    [Part2]
    public string Part2()
    {
        var count = 1;
        var currentIndex = 0;
        var result = 0;

        for (var i = 0; i < 50_000_000; i++)
        {
            currentIndex = (currentIndex + Input).Mod(count) + 1;
            if (currentIndex == 1) // this currently is the answer
                result = i + 1;

            count++;
        }

        return result.ToString();
    }
}