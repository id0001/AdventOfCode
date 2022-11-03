using AdventOfCode.Core;

namespace AdventOfCode2020.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part1]
    public string Part1()
    {
        const long pub1 = 13135480;
        const long pub2 = 8821721;

        var loopSize1 = FindLoopSize(7, pub1);

        var key1 = Transform(pub2, loopSize1);

        return key1.ToString();
    }

    private static long FindLoopSize(long subjectNumber, long target)
    {
        long value = 1;

        var i = 0;
        while (value != target)
        {
            value = value * subjectNumber;
            value %= 20201227;
            i++;
        }

        return i;
    }

    private static long Transform(long subjectNumber, long loopSize)
    {
        long value = 1;
        for (var i = 0; i < loopSize; i++)
        {
            value *= subjectNumber;
            value %= 20201227;
        }

        return value;
    }
}