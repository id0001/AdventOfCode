﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeLib.IO
{
	public interface IChallengeInput
	{
		public Task<string> ReadAllTextAsync(int challenge);

		public IAsyncEnumerable<string> ReadLinesAsync(int challenge);

		public IAsyncEnumerable<char> ReadCharactersAsync(int challenge);

		public IAsyncEnumerable<int> ReadIntegersAsync(int challenge);
	}
}
