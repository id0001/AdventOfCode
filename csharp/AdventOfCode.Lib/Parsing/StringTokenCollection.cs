using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode.Lib.Parsing;

public class StringTokenCollection : IReadOnlyList<string>
{
    private readonly string[] _tokens;

    public StringTokenCollection(string[] tokens)
    {
        _tokens = tokens;
    }

    public string First => _tokens[0];
    public string Second => _tokens[1];
    public string Third => _tokens[2];

    public string this[int index] => _tokens[index];
    public int Count => _tokens.Length;
    public IEnumerator<string> GetEnumerator() => _tokens.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}