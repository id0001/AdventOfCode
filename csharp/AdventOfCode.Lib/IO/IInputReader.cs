using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.IO
{
	public interface IInputReader
	{
		Task<string> ReadAllTextAsync(int challenge);

		IAsyncEnumerable<char> ReadCharactersAsync(int challenge);


		IAsyncEnumerable<string> ReadLinesAsync(int challenge);
		IAsyncEnumerable<T> ReadLinesAsync<T>(int challenge);


		IAsyncEnumerable<string> ReadLineAsync(int challenge, char separator);
		IAsyncEnumerable<T> ReadLineAsync<T>(int challenge, char separator);
	}
}
