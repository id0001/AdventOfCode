using AdventOfCode.Lib.Extensions.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode2017
{
    public static class KnotHash
    {
        public static byte[] Generate(string input)
        {
            var sparseHash = Enumerable.Range(0, 256).Select(b => b.As<byte>()).ToArray();
            var i = 0;
            var skip = 0;

            foreach (var round in Enumerable.Range(0, 64))
            {
                foreach (var length in input.Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }))
                {
                    sparseHash = sparseHash
                        .Cycle() // make circle
                        .Skip(i) // move i to 0
                        .Take(length) // take the length
                        .Reverse() // reverse the sequence
                        .Concat(
                            sparseHash
                                .Cycle()
                                .Skip(i + length)
                                .Take(sparseHash.Length - length)
                        ) // add the remaining numbers
                        .Cycle() // make circle
                        .Skip(sparseHash.Length - i) // move back to original index
                        .Take(sparseHash.Length) // make original length
                        .ToArray();

                    i = (i + length + skip).Mod(sparseHash.Length);
                    skip++;
                }
            }

            return sparseHash.Chunk(16).Select(x => x.Xor()).ToArray();
        }
    }
}
