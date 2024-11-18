using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(13)]
public class Challenge13(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(13);
        var carts = ExtractCarts(grid).Cast<ActiveCart>().ToArray();

        while (true)
            for (var i = 0; i < carts.Length; i++)
            {
                carts[i] = Move(grid, carts[i]);
                if (carts.DistinctBy(x => x.Pose.Position).Count() != carts.Length)
                    return $"{carts[i].Pose.Position.X},{carts[i].Pose.Position.Y}";
            }
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(13);
        var carts = ExtractCarts(grid).ToArray();
        while (true)
        {
            for (var i = 0; i < carts.Length; i++)
                if (carts[i] is ActiveCart cart)
                {
                    carts[i] = cart = Move(grid, cart);
                    var crashIndex = FindCrashIndex(carts, cart, i);
                    if (crashIndex >= 0)
                    {
                        carts[i] = new CrashedCart();
                        carts[crashIndex] = new CrashedCart();
                    }
                }

            if (carts.OfType<ActiveCart>().Count() == 1)
            {
                var final = carts.OfType<ActiveCart>().Single();
                return $"{final.Pose.Position.X},{final.Pose.Position.Y}";
            }
        }
    }

    private static ActiveCart Move(char[,] grid, ActiveCart cart)
    {
        // Step
        cart = cart with {Pose = cart.Pose.Step()};

        // Turn
        return (grid[cart.Pose.Position.Y, cart.Pose.Position.X], cart.Pose.Face) switch
        {
            ('+', _) => TurnAtIntersection(cart),
            ('/', var face) when face == Face.Up || face == Face.Down => cart with {Pose = cart.Pose.TurnRight()},
            ('/', var face) when face == Face.Left || face == Face.Right => cart with {Pose = cart.Pose.TurnLeft()},
            ('\\', var face) when face == Face.Up || face == Face.Down => cart with {Pose = cart.Pose.TurnLeft()},
            ('\\', var face) when face == Face.Left || face == Face.Right => cart with {Pose = cart.Pose.TurnRight()},
            _ => cart
        };
    }

    private static ActiveCart TurnAtIntersection(ActiveCart cart) => cart.Choice switch
    {
        0 => new ActiveCart(cart.Pose.TurnLeft(), (cart.Choice + 1).Mod(3)),
        2 => new ActiveCart(cart.Pose.TurnRight(), (cart.Choice + 1).Mod(3)),
        _ => cart with {Choice = (cart.Choice + 1).Mod(3)}
    };

    private int FindCrashIndex(Cart[] carts, ActiveCart cart, int index) => carts
        .Select((x, i) => (Item: x, Index: i))
        .Where(x => x.Index != index && x.Item is ActiveCart active && active.Pose.Position == cart.Pose.Position)
        .Select(x => x.Index)
        .FirstOrDefault(-1);

    private static IEnumerable<Cart> ExtractCarts(char[,] grid)
    {
        var positions = grid.Where((_, c) => c is 'v' or '>' or '<' or '^').ToArray();
        foreach (var p in positions)
        {
            var dir = grid[p.Y, p.X] switch
            {
                '>' => Face.Right,
                '<' => Face.Left,
                '^' => Face.Up,
                'v' => Face.Down,
                _ => throw new NotImplementedException()
            };

            grid[p.Y, p.X] = grid[p.Y, p.X] switch
            {
                '>' => '-',
                '<' => '-',
                '^' => '|',
                'v' => '|',
                _ => grid[p.Y, p.X]
            };

            yield return new ActiveCart(new Pose2(p, dir), 0);
        }
    }

    private record Cart;

    private record ActiveCart(Pose2 Pose, int Choice) : Cart;

    private record CrashedCart : Cart;
}