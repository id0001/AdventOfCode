using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The IntWriter class TODO: Describe class here
	/// </summary>
	internal class IntWriter
	{
		private static readonly object SyncRoot = new object();
		private readonly Queue<long> _writeQueue;

		public event EventHandler Updated;

		public IntWriter(Queue<long> queue)
		{
			_writeQueue = queue;
		}

		public void Write(long value)
		{
			lock (SyncRoot)
			{
				_writeQueue.Enqueue(value);
			}

			Updated?.Invoke(this, EventArgs.Empty);
		}

		public void Clear()
		{
			_writeQueue.Clear();
		}

		public long[] Monitor()
		{
			lock (SyncRoot)
			{
				return _writeQueue.ToArray();
			}
		}
	}
}
