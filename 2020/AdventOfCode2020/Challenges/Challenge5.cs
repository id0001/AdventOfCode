using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(5)]
    public class Challenge5
    {
        private readonly IInputReader inputReader;
        private string[] input;

        public Challenge5(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            input = await inputReader.ReadLinesAsync(5).ToArrayAsync();
        }

        [Part1]
        public string Part1()
        {
            int highestSeatId = 0;
            foreach (string query in input)
            {
                Point p = Search(8, 128, query);
                int id = (p.Y * 8) + p.X;
                if (id > highestSeatId)
                    highestSeatId = id;
            }

            return highestSeatId.ToString();
        }

        [Part2]
        public string Part2()
        {
            HashSet<int> seatIds = new HashSet<int>();
            foreach(string query in input)
            {
                Point p = Search(8, 128, query);
                int id = (p.Y * 8) + p.X;
                seatIds.Add(id);
            }

            int mySeat = Enumerable.Range(0, 8 * 128).Where(n => !seatIds.Contains(n) && seatIds.Contains(n - 1) && seatIds.Contains(n + 1)).First();
            return mySeat.ToString();
        }

        private Point Search(int width, int height, string query)
        {
            int xmin = 0, ymin = 0, xmax = width, ymax = height;
            foreach (char c in query)
            {
                switch (c)
                {
                    case 'F':
                        ymax = (ymax + ymin) / 2;
                        break;
                    case 'B':
                        ymin = (ymax + ymin) / 2;
                        break;
                    case 'L':
                        xmax = (xmax + xmin) / 2;
                        break;
                    case 'R':
                        xmin = (xmax + xmin) / 2;
                        break;
                }
            }

            return new Point(xmin, ymin);
        }
    }
}
