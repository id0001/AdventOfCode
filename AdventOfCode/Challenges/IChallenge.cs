using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public interface IChallenge
	{
		string Id { get; }

		Task<string> RunAsync();
	}
}
