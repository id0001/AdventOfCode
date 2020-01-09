using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The IntReader class TODO: Describe class here
	/// </summary>
	internal class IntReader
	{
		private static readonly object SyncRoot = new object();
		private readonly Queue<int> _readQueue;

		public IntReader(Queue<int> queue)
		{
			_readQueue = queue;
		}

		public int Read()
		{
			lock (SyncRoot)
			{
				return _readQueue.Dequeue();
			}
		}

		public IEnumerable<int> ReadToEnd()
		{
			while (TryRead(out int output))
			{
				yield return output;
			}
		}

		public bool TryRead(out int output)
		{
			lock (SyncRoot)
			{
				return _readQueue.TryDequeue(out output);
			}
		}

		public void Clear()
		{
			_readQueue.Clear();
		}

		public int[] Monitor()
		{
			lock (SyncRoot)
			{
				return _readQueue.ToArray();
			}
		}
	}
}
