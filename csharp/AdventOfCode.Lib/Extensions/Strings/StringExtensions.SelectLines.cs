namespace AdventOfCode.Lib;

public static partial class StringExtensions
{
    public static IList<string> SelectLines(this string source) => source.SplitBy(Environment.NewLine);

    public static (string Header, IList<string> Lines) SelectLinesWithHeader(this string source)
        => source.SelectLines().Into(lines => (lines[0], lines.Take(1..).ToList()));
}