namespace AdventOfCode.Lib
{
    public static partial class StringExtensions
    {
        public static IList<string> Paragraphs(this string source) => source.SplitBy($"{Environment.NewLine}{Environment.NewLine}");
    }
}
