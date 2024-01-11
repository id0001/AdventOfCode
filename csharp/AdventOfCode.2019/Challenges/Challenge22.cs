using System.Numerics;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2019.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader InputReader)
{
    private readonly List<ShuffleType> _shuffleList = new();

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in InputReader.ReadLinesAsync(22))
            if (line == "deal into new stack")
            {
                _shuffleList.Add(new ShuffleType(0, 0));
            }
            else if (line.StartsWith("cut"))
            {
                var num = int.Parse(line.Substring(4));
                _shuffleList.Add(new ShuffleType(1, num));
            }
            else if (line.StartsWith("deal with increment"))
            {
                var num = int.Parse(line.Substring(20));
                _shuffleList.Add(new ShuffleType(2, num));
            }
    }

    [Part1]
    public string Part1()
    {
        var position = 2019;

        foreach (var action in _shuffleList)
            position = action.Type switch
            {
                0 => PositionAfterReverse(10007, position),
                1 => PositionAfterCut(10007, position, action.Number),
                2 => PositionAfterIncrement(10007, position, action.Number),
                _ => throw new NotSupportedException()
            };

        return position.ToString();
    }

    [Part2]
    public string Part2()
    {
        var deckSize = new BigInteger(119315717514047L); // N
        var times = new BigInteger(101741582076661L); // M
        BigInteger target = 2020;

        // Combine all the shuffle operations into 1 formula.
        BigInteger a = BigInteger.One, b = BigInteger.Zero;
        foreach (var action in _shuffleList)
        {
            BigInteger la, lb;
            switch (action.Type)
            {
                case 0:
                    (la, lb) = (BigInteger.MinusOne, BigInteger.MinusOne); // Reverse(-x-1 % N) -> a = -1, b = -1
                    break;
                case 1:
                    (la, lb) = (BigInteger.One, new BigInteger(-action.Number)); // Cut(x-n % N) -> a = 1, b = -n
                    break;
                case 2:
                    (la, lb) = (new BigInteger(action.Number), BigInteger.Zero); // Increment(xn % N) -> a = n, b = 0
                    break;
                default:
                    throw new NotImplementedException();
            }

            a = Euclid.Modulus(la * a, deckSize);
            b = Euclid.Modulus(la * b + lb, deckSize);
        }

        // This operation now needs to be applied M times.
        // la = a, lb = b
        // a = 1, b = 0
        // for(range(0, M)):
        //    (a = (a * la) % N, b = (la * b + lb) % N)

        // a => (a**M) % N
        // b => (a**(M-1)b)+(a**(M-2)b)+...+(a**(M-M)b) => b * ((a**(M-1))+(a**(M-2))+...+(a**(M-M))) => ((b * ((a**M)-1)) / (a-1)) % N

        var ma = BigInteger.ModPow(a, times, deckSize); // ma = (a**M) % N
        var mb = Euclid.Modulus(b * (ma - 1) * Euclid.ModInverse(a - 1, deckSize),
            deckSize); // mb = ((b * ((a**M)-1)) / (a-1)) % N

        var r = Euclid.Modulus((target - mb) * Euclid.ModInverse(ma, deckSize), deckSize); // (x-b)/a % N

        return r.ToString();
    }

    private static int PositionAfterReverse(int deckSize, int position) => Euclid.Modulus(-position - 1, deckSize);

    private static int PositionAfterCut(int deckSize, int position, int n) => Euclid.Modulus(position - n, deckSize);

    private static int PositionAfterIncrement(int deckSize, int position, int n) =>
        Euclid.Modulus(position * n, deckSize);

    private record ShuffleType(int Type, int Number);
}