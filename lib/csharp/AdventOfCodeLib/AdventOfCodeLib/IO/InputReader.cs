using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCodeLib.IO
{
	public class InputReader : IInputReader
	{
		private readonly string basePath;

		public InputReader(IOptions<InputReaderOptions> options)
		{
			basePath = options.Value.InputFolder;
		}

		private string GetPath(int challenge) => Path.Combine(basePath, $"{challenge}.txt");

		public async Task<string> ReadAllTextAsync(int challenge) => await File.ReadAllTextAsync(GetPath(challenge));

		public async IAsyncEnumerable<string> ReadLinesAsync(int challenge)
		{
			using var stream = File.OpenText(GetPath(challenge));

			while (!stream.EndOfStream)
			{
				string line = await stream.ReadLineAsync();
				yield return line;
			}
		}

		public async IAsyncEnumerable<char> ReadCharactersAsync(int challenge)
		{
			using var stream = File.OpenText(GetPath(challenge));

			char[] buffer = new char[1024];
			while (!stream.EndOfStream)
			{
				int readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
				for(int i = 0; i < readCount; i++)
				{
					yield return buffer[i];
				}
			}
		}

		public async IAsyncEnumerable<int> ReadIntegersAsync(int challenge)
		{
			using var stream = File.OpenText(GetPath(challenge));

			while (!stream.EndOfStream)
			{
				string line = await stream.ReadLineAsync();
				yield return int.Parse(line);
			}
		}
	}
}
