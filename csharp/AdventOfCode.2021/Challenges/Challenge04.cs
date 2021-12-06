using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
    [Challenge(4)]
    public class Challenge04
    {
        private const int BoardLength = 5;

        private readonly IInputReader inputReader;
        private Queue<int> numbers;
        private List<int[]> boards;

        public Challenge04(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            var lines = await inputReader.ReadLinesAsync(4).ToArrayAsync();
            numbers = new Queue<int>(lines[0].Split(',').Select(int.Parse));

            boards = new List<int[]>();
            List<int> board = new List<int>();
            for (int i = 2; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    boards.Add(board.ToArray());
                    board.Clear();
                    continue;
                }

                board.AddRange(lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse));
            }

            boards.Add(board.ToArray());
        }

        [Part1]
        public string Part1()
        {
            List<int[]> score = Enumerable.Range(0,boards.Count).Select(_ => new int[BoardLength * 2]).ToList();
            List<int> history = new List<int>();

            while (numbers.Count > 0)
            {
                int number = numbers.Dequeue();
                history.Add(number);

                for (int b = 0; b < boards.Count; b++)
                {
                    int index = Array.IndexOf(boards[b], number);
                    if (index == -1)
                        continue;

                    int y = index / BoardLength;
                    int x = index - (y*BoardLength);

                    score[b][x]++;
                    score[b][y + BoardLength]++;

                    if (score[b].Any(x => x == 5))
                    {
                        // Winner winner chicken dinner
                        return CalculateScore(boards[b], history, number).ToString();
                    }
                }
            }

            return null;
        }

        [Part2]
        public string Part2()
        {
            List<int[]> score = Enumerable.Range(0, boards.Count).Select(_ => new int[BoardLength * 2]).ToList();
            List<int> history = new List<int>();

            while (numbers.Count > 0)
            {
                int number = numbers.Dequeue();
                history.Add(number);

                for (int b = 0; b < boards.Count; b++)
                {
                    int index = Array.IndexOf(boards[b], number);
                    if (index == -1)
                        continue;

                    int y = index / BoardLength;
                    int x = index - (y * BoardLength);

                    score[b][x]++;
                    score[b][y + BoardLength]++;

                    if (score.All(s => s.Any(x => x == 5)))
                    {
                        // Winner winner chicken dinner
                        return CalculateScore(boards[b], history, number).ToString();
                    }
                }
            }

            return null;
        }

        private int CalculateScore(int[] board, IEnumerable<int> numbers, int lastNumberCalled)
        {
            int unmarked = board.Except(numbers).Sum();
            return unmarked * lastNumberCalled;
        }
    }
}
