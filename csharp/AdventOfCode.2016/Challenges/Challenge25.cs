using AdventOfCode.Core;

namespace AdventOfCode2016.Challenges;

[Challenge(25)]
public class Challenge25
{
    [Part1]
    public string Part1()
    {
        var num = 182 * 14;

        for (var i = 0; i < num; i++)
        {
            var bin = Convert.ToString(num + i, 2);
            var one = true;
            var err = false;
            for (var j = 0; j < bin.Length; j++)
            {
                if ((one && bin[j] != '1') || (!one && bin[j] == '1'))
                {
                    err = true;
                    break;
                }

                one = !one;
            }

            if (!err)
                return i.ToString();
        }

        return string.Empty;
    }
}