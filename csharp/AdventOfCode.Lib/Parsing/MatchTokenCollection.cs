using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode.Lib.Parsing
{
    public class MatchTokenCollection : IReadOnlyList<Match>
    {
        private readonly Match[] _tokens;

        public MatchTokenCollection(Match[] tokens)
        {
            _tokens = tokens;
        }


        public Match First => _tokens[0];
        public Match Second => _tokens[1];
        public Match Third => _tokens[2];

        public Match this[int index] => _tokens[index];
        public int Count => _tokens.Length;
        public IEnumerator<Match> GetEnumerator() => _tokens.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
