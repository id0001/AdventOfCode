using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.Collections.Trees
{
	public interface IDFSEnumerable<T> where T : IDFSEnumerable<T>
	{
		IEnumerable<T> EnumeratePreOrder();

		IEnumerable<T> EnumerateInOrder();

		IEnumerable<T> EnumeratePostOrder();
	}
}
