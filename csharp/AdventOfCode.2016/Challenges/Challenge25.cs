using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2016.Challenges;

[Challenge(25)]
public class Challenge25(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var num = 182 * 14;

        for(var i = 0; i < num; i++)
        {
            var bin = Convert.ToString(num + i, 2);
            var one = true;
            var err = false;
            for(var j = 0; j < bin.Length; j++)
            {
                if((one && bin[j] != '1') || (!one && bin[j] == '1'))
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