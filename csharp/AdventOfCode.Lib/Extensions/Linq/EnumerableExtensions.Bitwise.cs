using System.Numerics;

namespace AdventOfCode.Lib.Extensions.Linq
{
    public static partial class EnumerableExtensions
    {
        public static TNumber Xor<TNumber>(this IEnumerable<TNumber> source)
            where TNumber : IBitwiseOperators<TNumber, TNumber, TNumber> => source.Aggregate((a, b) => a ^ b);
    }
}
