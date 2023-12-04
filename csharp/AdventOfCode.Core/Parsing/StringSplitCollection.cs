using AdventOfCode.Core.Extensions;

namespace AdventOfCode.Core.Parsing;

public record StringSplitCollection(string[] Values)
{
    public string First() => Values[0];
    public string Second() => Values[1];
    public string Third() => Values[2];
    public T Select<T>(Func<StringSplitCollection, T> selector) => selector(this);
    public IEnumerable<StringSplitCollection> SplitEachBy(string splitter) => Values.Select(x => x.SplitBy(splitter));
}