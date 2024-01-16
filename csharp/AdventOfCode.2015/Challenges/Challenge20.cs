using AdventOfCode.Core;

namespace AdventOfCode2015.Challenges;

[Challenge(20)]
public class Challenge20
{
    private const int Input = 29000000;

    public Challenge20()
    {
    }

    [Part1]
    public string Part1()
    {
        var arr = new int[Input / 10];
        for (var elf = 1; elf < Input / 10; elf++)
        {
            for (var house = elf; house < Input / 10; house += elf)
                arr[house - 1] += elf * 10;
        }

        for (var i = 0; i < arr.Length; i++)
            if (arr[i] >= Input)
                return (i + 1).ToString();

        return string.Empty;
    }

    [Part2]
    public string Part2()
    {
        var arr = new int[Input / 11];
        for (var elf = 1; elf < Input / 11; elf++)
        {
            for(var m = 0; m < 50; m++)
            {
                var house = elf + (m * elf);
                if (house >= arr.Length)
                    break;

                arr[house - 1] += elf * 11;
            }
        }

        for (var i = 0; i < arr.Length; i++)
            if (arr[i] >= Input)
                return (i + 1).ToString();

        return string.Empty;
    }
}
