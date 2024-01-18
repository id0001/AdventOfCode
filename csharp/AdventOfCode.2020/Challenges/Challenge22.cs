using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader inputReader)
{
    private readonly Queue<int> _deck1 = new();
    private readonly Queue<int> _deck2 = new();


    [Setup]
    public async Task SetupAsync()
    {
        var state = 0;

        await foreach (var line in inputReader.ReadLinesAsync(22))
        {
            if (line is "Player 1:" or "Player 2:")
                continue;

            if (string.IsNullOrEmpty(line))
            {
                state++;
                continue;
            }

            if (state == 0)
                _deck1.Enqueue(int.Parse(line));
            else
                _deck2.Enqueue(int.Parse(line));
        }
    }

    [Part1]
    public string Part1()
    {
        while (_deck1.Count > 0 && _deck2.Count > 0)
        {
            var card1 = _deck1.Dequeue();
            var card2 = _deck2.Dequeue();

            if (card1 > card2)
            {
                _deck1.Enqueue(card1);
                _deck1.Enqueue(card2);
            }
            else
            {
                _deck2.Enqueue(card2);
                _deck2.Enqueue(card1);
            }
        }

        var winningDeck = _deck1.Count > 0 ? _deck1.ToArray() : _deck2.ToArray();

        var score = 0;
        for (var i = winningDeck.Length - 1; i >= 0; i--) score += winningDeck[i] * (winningDeck.Length - i);


        return score.ToString();
    }

    [Part2]
    public string Part2()
    {
        var cache = new Dictionary<string, int>();
        var winner = PlayRecurseCombat(_deck1, _deck2, cache);

        var winningDeck = winner == 1 ? _deck1.ToArray() : _deck2.ToArray();

        var score = 0;
        for (var i = winningDeck.Length - 1; i >= 0; i--) score += winningDeck[i] * (winningDeck.Length - i);

        return score.ToString();
    }

    private int PlayRecurseCombat(Queue<int> deck1, Queue<int> deck2, IDictionary<string, int> gameCache)
    {
        var key = GetKey(deck1, deck2);

        if (gameCache.TryGetValue(key, out var combat))
            return combat;

        var history1 = new HashSet<string>();
        var history2 = new HashSet<string>();

        while (deck1.Count > 0 && deck2.Count > 0)
        {
            var deck1Array = string.Join(", ", deck1);
            var deck2Array = string.Join(", ", deck2);

            // If a previous configuration is played, player 1 wins
            if (history1.Contains(deck1Array) || history2.Contains(deck2Array))
            {
                gameCache.Add(key, 1);
                return 1;
            }

            history1.Add(deck1Array);
            history2.Add(deck2Array);

            // Play the game as normal
            var card1 = deck1.Dequeue();
            var card2 = deck2.Dequeue();

            int winner;
            if (deck1.Count >= card1 && deck2.Count >= card2)
                // If both players have a deck count greater or equal to the value of the current cars, play a new game of combat to determine the winner of the round.
                winner = PlayRecurseCombat(new Queue<int>(deck1.Take(card1).ToArray()),
                    new Queue<int>(deck2.Take(card2).ToArray()), gameCache);
            else
                // Play as normal.
                winner = card1 > card2 ? 1 : 2;

            // Finish round and add cards to winning deck.
            if (winner == 1)
            {
                deck1.Enqueue(card1);
                deck1.Enqueue(card2);
            }
            else
            {
                deck2.Enqueue(card2);
                deck2.Enqueue(card1);
            }
        }

        // Add to cache.
        gameCache.Add(key, deck1.Count > 0 ? 1 : 2);
        return deck1.Count > 0 ? 1 : 2;
    }

    private static string GetKey(IEnumerable<int> deck1, IEnumerable<int> deck2) =>
        string.Concat(string.Join(", ", deck1), " vs ", string.Join(", ", deck2));
}