using AdventOfCode.Core;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2021.Challenges;

[Challenge(21)]
public class Challenge21
{
    private const int Player1 = 10;
    private const int Player2 = 4;

    [Part1]
    public string Part1()
    {
        var die = 1;
        int[] pos = {Player1 - 1, Player2 - 1};
        int[] score = {0, 0};
        var i = 0;
        var totalRolls = 0;

        while (!score.Any(x => x >= 1000))
        {
            var roll = die + Euclid.Modulus(die + 1, 100) + Euclid.Modulus(die + 2, 100);
            die = Euclid.Modulus(die + 3, 100);
            totalRolls += 3;

            pos[i] = Euclid.Modulus(pos[i] + roll, 10);
            score[i] += pos[i] + 1;
            i = Euclid.Modulus(i + 1, 2);
        }

        return (score.Min() * totalRolls).ToString();
    }

    [Part2]
    public string Part2()
    {
        var cache = new Dictionary<StateKey, Score>();
        var wins = PlayTurn(new Position(Player1 - 1, Player2 - 1), new Score(0, 0), 0, cache);

        return wins.Max.ToString();
    }

    private Score PlayTurn(Position position, Score score, int player, Dictionary<StateKey, Score> winCache)
    {
        var key = new StateKey(position, score, player);
        if (winCache.TryGetValue(key, out var winner)) return winner;

        if (score.Player1 >= 21)
            return new Score(1, 0);

        if (score.Player2 >= 21)
            return new Score(0, 1);

        winner = new Score(0, 0);
        for (var z = 1; z < 4; z++)
        for (var y = 1; y < 4; y++)
        for (var x = 1; x < 4; x++)
        {
            var roll = x + y + z;
            Position newPosition;
            Score newScore;
            if (player == 0)
            {
                newPosition = new Position(Euclid.Modulus(position.Player1 + roll, 10), position.Player2);
                newScore = new Score(score.Player1 + newPosition.Player1 + 1, score.Player2);
            }
            else
            {
                newPosition = new Position(position.Player1, Euclid.Modulus(position.Player2 + roll, 10));
                newScore = new Score(score.Player1, score.Player2 + newPosition.Player2 + 1);
            }

            var delta = PlayTurn(newPosition, newScore, Euclid.Modulus(player + 1, 2), winCache);
            winner = new Score(winner.Player1 + delta.Player1, winner.Player2 + delta.Player2);
        }

        winCache.Add(key, winner);
        return winner;
    } // ReSharper disable NotAccessedPositionalProperty.Local
    private record StateKey(Position Position, Score Score, int Player);

    private record Position(int Player1, int Player2);

    private record Score(long Player1, long Player2)
    {
        public long Max => Math.Max(Player1, Player2);
    }
}