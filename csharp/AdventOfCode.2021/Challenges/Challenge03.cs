using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(3)]
    public class Challenge03
    {
        private readonly IInputReader inputReader;
        private string[] data;

        public Challenge03(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLinesAsync(3).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int len = data[0].Length;
            var counts = Enumerable.Range(0, len)
                .Select(i => data
                                .Select(x => x[i])
                                .GroupBy(x => x)
                                .OrderBy(x => x.Key)
                                .Select(x => x.Count())
                                .ToArray())
                .ToList();

            int gamma = 0;

            for(int i = 0; i < counts.Count; i++)
            {
                if (counts[i][1] > counts[i][0])
                    gamma += 1 << (len - 1) - i;
            }

            int epsilon = ~gamma & 0xFFF;

            return (gamma * epsilon).ToString();
        }

        [Part2]
        public string Part2()
        {
            List<string> oxygenList = data.ToList();
            List<string> co2List = data.ToList();

            int len = data[0].Length;
            for (int i = 0; i < len; i++)
            {
                if (oxygenList.Count == 1 && co2List.Count == 1)
                    break;

                char commonOxy = GetMostCommon(oxygenList, i);
                char commonCo2 = GetMostCommon(co2List, i);

                if (oxygenList.Count > 1)
                    oxygenList = oxygenList.Where(x => x[i] == commonOxy).ToList();

                if(co2List.Count > 1)
                    co2List = co2List.Where(x => x[i] != commonCo2).ToList();
            }

            int o = Convert.ToInt32(oxygenList[0], 2);
            int c = Convert.ToInt32(co2List[0], 2);

            return (o * c).ToString();
        }

        private static char GetMostCommon(IList<string> list, int position) => list
            .Select(x => x[position])
            .GroupBy(x => x)
            .OrderByDescending(x => x.Count())
            .ThenByDescending( x => x.Key)
            .Select(x => x.Key)
            .First();
    }
}
