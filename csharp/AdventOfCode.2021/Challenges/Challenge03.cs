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
            uint gamma = 0;
            for (int x = 0; x < data[0].Length; x++)
            {
                int c0 = 0;
                int c1 = 0;
                for (int y = 0; y < data.Length; y++)
                {
                    if (data[y][x] == '0') c0++;
                    if (data[y][x] == '1') c1++;
                }

                if (c1 > c0)
                    gamma += 1u << (data[0].Length - 1) - x;
            }

            uint epsilon = ~gamma & 0xFFF;

            return (gamma * epsilon).ToString();
        }

        [Part2]
        public string Part2()
        {

            List<string> oxygenList = data.ToList();
            char common = GetMostCommon(oxygenList, 0);

            for (int i = 0; i < oxygenList[0].Length; i++)
            {
                oxygenList = oxygenList.Where(x => x[i] == common).ToList();
                if (oxygenList.Count == 1)
                    break;

                common = GetMostCommon(oxygenList, i + 1);
            }


            List<string> co2List = data.ToList();
            common = GetMostCommon(co2List, 0);

            for (int i = 0; i < co2List[0].Length; i++)
            {
                co2List = co2List.Where(x => x[i] != common).ToList();
                if (co2List.Count == 1)
                    break;

                common = GetMostCommon(co2List, i + 1);
            }


            int o = Convert.ToInt32(oxygenList[0], 2);
            int c = Convert.ToInt32(co2List[0], 2);

            return (o * c).ToString();
        }

        private static char GetMostCommon(IList<string> list, int position)
        {
            int c0 = 0;
            int c1 = 0;
            for (int y = 0; y < list.Count; y++)
            {
                if (list[y][position] == '0') c0++;
                if (list[y][position] == '1') c1++;
            }

            return c1 >= c0 ? '1' : '0';
        }
    }
}
