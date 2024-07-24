using System.Globalization;

namespace AdventOfCode.Lib.Assembly
{
    public sealed record Arguments<TValue>(IReadOnlyDictionary<string, string> arguments, DefaultMemory<TValue> memory)
        where TValue : notnull, IParsable<TValue>
    {
        public TValue Value(string key) => TValue.TryParse(arguments[key], CultureInfo.InvariantCulture, out var result)
            ? result
            : memory.Get(arguments[key]);

        public string Register(string key) => arguments[key];
    }
}