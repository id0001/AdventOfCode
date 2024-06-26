using AdventOfCode.Lib.Extensions.Linq;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Collections;

namespace AdventOfCode2017
{
    public static class KnotHash
    {
        public static byte[] Generate(string input)
        {
            var sparseHash = new CircularArray<byte>(256);
            for (var b = 0; b < 256; b++)
                sparseHash[b] = (byte)b;

            var i = 0;
            var skip = 0;

            foreach (var round in Enumerable.Range(0, 64))
            {
                foreach (var length in input.Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }))
                {
                    Twist(sparseHash, i, length);
                    i = (i + length + skip).Mod(sparseHash.Count);
                    skip++;
                }
            }

            return sparseHash.Chunk(16).Select(x => x.Xor()).ToArray();
        }

        private static void Twist(CircularArray<byte> hash, int start, int length)
        {
            var copy = new byte[length];
            hash.CopyTo(copy, start, length);

            for (var i = 0; i < length; i++)
                hash[start + i] = copy[^(i+1)];
        }
    }
}
