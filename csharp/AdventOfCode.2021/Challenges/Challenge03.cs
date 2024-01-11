using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(3)]
public class Challenge03(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var data = await InputReader.ReadLinesAsync(3).ToArrayAsync();
        var len = data[0].Length;
        var counts = Enumerable.Range(0, len)
            .Select(i => data
                .Select(x => x[i])
                .GroupBy(x => x)
                .OrderBy(x => x.Key)
                .Select(x => x.Count())
                .ToArray())
            .ToList();

        var gamma = 0;

        for (var i = 0; i < counts.Count; i++)
            if (counts[i][1] > counts[i][0])
                gamma += 1 << (len - 1 - i);

        var epsilon = ~gamma & 0xFFF;

        return (gamma * epsilon).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var data = await InputReader.ReadLinesAsync(3).ToArrayAsync();
        var oxygenList = data.ToList();
        var co2List = data.ToList();

        var len = data[0].Length;
        for (var i = 0; i < len; i++)
        {
            if (oxygenList.Count == 1 && co2List.Count == 1)
                break;

            var commonOxy = GetMostCommon(oxygenList, i);
            var commonCo2 = GetMostCommon(co2List, i);

            if (oxygenList.Count > 1)
                oxygenList = oxygenList.Where(x => x[i] == commonOxy).ToList();

            if (co2List.Count > 1)
                co2List = co2List.Where(x => x[i] != commonCo2).ToList();
        }

        var o = Convert.ToInt32(oxygenList[0], 2);
        var c = Convert.ToInt32(co2List[0], 2);

        return (o * c).ToString();
    }

    private static char GetMostCommon(IList<string> list, int position) => list
        .Select(x => x[position])
        .GroupBy(x => x)
        .OrderByDescending(x => x.Count())
        .ThenByDescending(x => x.Key)
        .Select(x => x.Key)
        .First();
}