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
            ulong[] groups = new ulong[9];
            foreach (int i in data)
                groups[i]++;

            return CalculateFishies(80, groups).ToString();
        }

        [Part2]
        public string Part2()
        {
            ulong[] groups = new ulong[9];
            foreach (int i in data)
                groups[i]++;

            return CalculateFishies(256, groups).ToString();
        }

        private ulong CalculateFishies(int totalDays, ulong[] groups) => Breed(0, totalDays, groups).Sum(x => x);

        private ulong[] Breed(int dayFrom, int totalDays, ulong[] groups)
        {
            if (dayFrom == totalDays)
                return groups;

            return Breed(dayFrom + 1, totalDays, new ulong[] { groups[1], groups[2], groups[3], groups[4], groups[5], groups[6], groups[0] + groups[7], groups[8], groups[0] });
        }
    }
}
