namespace AdventOfCode.Lib.Tests
{
    public static class InputReader
    {
        public static async IAsyncEnumerable<string> ReadLinesAsync(string filename)
        {
            using var stream = File.OpenText(GetPath(filename));
            while (!stream.EndOfStream)
                yield return (await stream.ReadLineAsync())!;
        }

        public static async IAsyncEnumerable<T> ParseLinesAsync<T>(string filename, Func<string, T> parser)
        {
            using var reader = File.OpenText(GetPath(filename));

            while (!reader.EndOfStream)
                yield return parser.Invoke((await reader.ReadLineAsync())!);
        }

        private static string GetPath(string filename) => Path.Combine("TestData", filename);
    }
}
