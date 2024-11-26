namespace AdventOfCode.Lib
{
    public static partial class StringExtensions
    {
        public static IList<string> Lines(this string source) => source.SplitBy(Environment.NewLine);

        public static (string Header, IList<string> Lines) LinesWithHeader(this string source)
           => source.Lines().Into(lines => (lines[0], lines.Take(1..).ToList()));
    }
}
