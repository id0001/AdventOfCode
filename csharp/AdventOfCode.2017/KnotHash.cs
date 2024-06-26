using AdventOfCode.Lib.Extensions.Linq;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017
{
    public static class KnotHash
    {
        public static byte[] Generate(string input)
        {
            CircularArray<byte> sparseHash = new(Enumerable.Range(0, 256).Select(x => (byte)x).ToArray());

            var i = 0;
            var skip = 0;

            foreach (var round in Enumerable.Range(0, 64))
            {
                foreach (var length in input.Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }))
                {
                    Twist(sparseHash, i, length);
                    i = (i + length + skip).Mod(sparseHash.Length);
                    skip++;
                }
            }

            return sparseHash.Chunk(16).Select(x => x.Xor()).ToArray();
        }

        private static void Twist(CircularArray<byte> hash, int start, int length)
        {
            var (l, r) = ((int)Math.Round(length / 2d, MidpointRounding.AwayFromZero) - 1, (int)Math.Floor(length / 2d));
            for (; l >= 0 && r < length; l--, r++)
                (hash[start + l], hash[start + r]) = (hash[start + r], hash[start + l]);
        }
    }
}
