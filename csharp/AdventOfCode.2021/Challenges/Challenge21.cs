using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Challenges
{
    [Challenge(21)]
    public class Challenge21
    {
        private readonly IInputReader inputReader;
        private int player1 = 10;
        private int player2 = 4;

        //private int player1 = 4;
        //private int player2 = 8;

        public Challenge21(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Part1]
        public string Part1()
        {
            int die = 1;
            int[] pos = { player1 - 1, player2 - 1 };
            int[] score = { 0, 0 };
            int i = 0;
            int totalRolls = 0;

            while (!score.Any(x => x >= 1000))
            {
                int roll = die + MathEx.Mod(die + 1, 100) + MathEx.Mod(die + 2, 100);
                die = MathEx.Mod(die + 3, 100);
                totalRolls += 3;

                pos[i] = MathEx.Mod(pos[i] + roll, 10);
                score[i] += pos[i] + 1;
                i = MathEx.Mod(i + 1, 2);
            }

            return (score.Min() * totalRolls).ToString();
        }

        [Part2]
        public string Part2()
        {
            var cache = new Dictionary<StateKey, Score>();
            var wins = PlayTurn(new Position(player1 - 1, player2 - 1), new Score(0, 0), 0, cache);

            return wins.Max.ToString();
        }

        private Score PlayTurn(Position position, Score score, int player, Dictionary<StateKey, Score> winCache)
        {
            var key = new StateKey(position, score, player);
            if (winCache.TryGetValue(key, out Score winner))
            {
                return winner;
            }

            if (score.Player1 >= 21)
                return new Score(1, 0);

            if (score.Player2 >= 21)
                return new Score(0, 1);

            winner = new Score(0, 0);
            for (int z = 1; z < 4; z++)
            {
                for (int y = 1; y < 4; y++)
                {
                    for (int x = 1; x < 4; x++)
                    {
                        int roll = x + y + z;
                        Position newPosition;
                        Score newScore;
                        if (player == 0)
                        {
                            newPosition = new Position(MathEx.Mod(position.Player1 + roll, 10), position.Player2);
                            newScore = new Score(score.Player1 + newPosition.Player1 + 1, score.Player2);
                        }
                        else
                        {
                            newPosition = new Position(position.Player1, MathEx.Mod(position.Player2 + roll, 10));
                            newScore = new Score(score.Player1, score.Player2 + newPosition.Player2 + 1);
                        }

                        Score delta = PlayTurn(newPosition, newScore, MathEx.Mod(player + 1, 2), winCache);
                        winner = new Score(winner.Player1 + delta.Player1, winner.Player2 + delta.Player2);
                    }
                }
            }

            winCache.Add(key, winner);
            return winner;
        }

        private record StateKey(Position Position, Score Score, int Player);
        private record Position(int Player1, int Player2);
        private record Score(long Player1, long Player2)
        {
            public long Max => Math.Max(Player1, Player2);
        }
    }
}
