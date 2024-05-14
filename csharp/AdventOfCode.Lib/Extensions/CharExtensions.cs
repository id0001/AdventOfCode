namespace AdventOfCode.Lib
{
    public static class CharExtensions
    {
        public static int AsInteger(this char source) => (int)char.GetNumericValue(source);

        public static double AsDecimal(this char source) => char.GetNumericValue(source);
    }
}
