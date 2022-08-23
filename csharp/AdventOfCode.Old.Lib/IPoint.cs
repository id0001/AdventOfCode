
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Lib
{
	public interface IPoint
	{
		int Dimensions { get; }

		int GetValue(int dimension);

		IEnumerable<IPoint> GetNeighbors();
	}
}
