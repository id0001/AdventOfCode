using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(6)]
    public class Challenge06
    {
        private readonly IInputReader inputReader;
        private List<int> data;

        public Challenge06(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            data = await inputReader.ReadLineAsync<int>(6, ',').ToListAsync();
        }

        [Part1]
        public string Part1()
        {
            for (int d = 0; d < 80; d++)
            {
                int len = data.Count;
                for (int i = 0; i < len; i++)
                {
                    data[i]--;
                    if (data[i] < 0)
                    {
                        data[i] = 6;
                        data.Add(8);
                    }
                }
            }

            return data.Count.ToString();
        }

        [Part2]
        public string Part2()
        {
            var map = data.GroupBy(x => x).Select(x => new FishGroup(x.Key, (ulong)x.Count())).ToList();

            for (int d = 0; d < 256; d++)
            {
                int len = map.Count;
                for (int i = 0; i < len; i++)
                {
                    (int sexyTime, _) = map[i];
                    sexyTime--;
                    if (sexyTime < 0)
                    {
                        sexyTime = 6;
                        map.Add(new FishGroup(8, map[i].Amount));
                    }

                    map[i] = new FishGroup(sexyTime, map[i].Amount);
                }

                map = map.GroupBy(x => x.SexyTime).Select(x => new FishGroup(x.Key, x.Sum(y => y.Amount))).ToList();
            }

            return map.Sum(x => x.Amount).ToString();
        }

        private record FishGroup(int SexyTime, ulong Amount);
    }
}
