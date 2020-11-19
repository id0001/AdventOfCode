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
		private readonly Queue<long> _readQueue;

		public IntReader(Queue<long> queue)
		{
			_readQueue = queue;
		}

		public long Read()
		{
			lock (SyncRoot)
			{
				return _readQueue.Dequeue();
			}
		}

		public IEnumerable<long> ReadToEnd()
		{
			while (TryRead(out long output))
			{
				yield return output;
			}
		}

		public bool TryRead(out long output)
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

		public long[] Monitor()
		{
			lock (SyncRoot)
			{
				return _readQueue.ToArray();
			}
		}
	}
}
