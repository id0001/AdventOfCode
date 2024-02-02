using AdventOfCode.Core;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2015.Challenges;

[Challenge(22)]
public class Challenge22
{
    private const int EnemyDamage = 9;

    [Part1]
    public string Part1()
    {
        var astar = new AStar<State>(n => GetAdjacent(n, false), Weight);

        astar.TryPath(new State(50, 58, 500, 0, 0, 0, 0), IsFinished, out _, out var minSpent);
        return minSpent.ToString();
    }

    [Part2]
    public string Part2()
    {
        var astar = new AStar<State>(n => GetAdjacent(n, true), Weight);

        astar.TryPath(new State(50, 58, 500, 0, 0, 0, 0), IsFinished, out _, out var minSpent);
        return minSpent.ToString();
    }

    private int Weight(State current, State next) => next.Spent - current.Spent;

    private bool IsFinished(State state)
    {
        return state.EnemyHp <= 0;
    }

    private IEnumerable<State> GetAdjacent(State current, bool hardMode)
    {
        if (hardMode)
            current = current with {PlayerHp = current.PlayerHp - 1};

        if (current.PlayerHp <= 0) // No paths
            yield break;

        var (playerHp, enemyHp, mana, spent, shield, poison, recharge) = ApplyEffects(current);

        // Magic missile
        if (mana >= 53)
            yield return EnemyTurn(new State(playerHp, enemyHp - 4, mana - 53, spent + 53, shield, poison, recharge));

        // Drain
        if (mana >= 73)
            yield return EnemyTurn(
                new State(playerHp + 2, enemyHp - 2, mana - 73, spent + 73, shield, poison, recharge));

        // Shield
        if (mana >= 113 && shield == 0)
            yield return EnemyTurn(new State(playerHp, enemyHp, mana - 113, spent + 113, 6, poison, recharge));

        // Poison
        if (mana >= 173 && poison == 0)
            yield return EnemyTurn(new State(playerHp, enemyHp, mana - 173, spent + 173, shield, 6, recharge));

        // Recharge
        if (mana >= 229 && recharge == 0)
            yield return EnemyTurn(new State(playerHp, enemyHp, mana - 229, spent + 229, shield, poison, 5));
    }

    private State ApplyEffects(State current)
    {
        if (current.Poison > 0)
            current = current with {EnemyHp = current.EnemyHp - 3, Poison = current.Poison - 1};

        if (current.Recharge > 0)
            current = current with {Mana = current.Mana + 101, Recharge = current.Recharge - 1};

        if (current.Shield > 0)
            current = current with {Shield = current.Shield - 1};

        return current;
    }

    private State EnemyTurn(State current)
    {
        // Get armor
        var armor = current.Shield > 0 ? 7 : 0;

        // Apply effects
        current = ApplyEffects(current);

        // Do damage
        return current with {PlayerHp = current.PlayerHp - Math.Max(1, EnemyDamage - armor)};
    }

    private record State(int PlayerHp, int EnemyHp, int Mana, int Spent, int Shield, int Poison, int Recharge);
}