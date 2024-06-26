namespace AdventOfCode.Lib
{
    public static class ByteArrayExtension
    {
        /// <summary>
        /// Converts an array of bytes to their bit equivalent where each byte starts with the most significant bit.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool[] ToBits(this byte[] source)
        {
            var bitCount = source.Length * 8;
            var bitArray = new bool[bitCount];

            for (var i = 0; i < source.Length; i++)
            {
                for (var bit = 0; bit < 8; bit++)
                    bitArray[i * 8 + bit] = ((source[i] >> (7 - bit)) & 1) == 1;
            }

            return bitArray;
        }
    }
}
