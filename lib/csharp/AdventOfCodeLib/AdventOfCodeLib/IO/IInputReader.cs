using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCodeLib.IO
{
	public interface IInputReader
	{
		public Task<string> ReadAllTextAsync(int challenge);

		public IAsyncEnumerable<string> ReadLinesAsync(int challenge);

		public IAsyncEnumerable<char> ReadCharactersAsync(int challenge);

		public IAsyncEnumerable<T> ReadLinesAsync<T>(int challenge);
	}
}
