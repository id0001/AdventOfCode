using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.Graphs
{
	public interface IGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		bool IsDirected { get; }
	}
}
