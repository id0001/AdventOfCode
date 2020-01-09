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
		private readonly Queue<int> _writeQueue;

		public event EventHandler Updated;

		public IntWriter(Queue<int> queue)
		{
			_writeQueue = queue;
		}

		public void Write(int value)
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

		public int[] Monitor()
		{
			lock (SyncRoot)
			{
				return _writeQueue.ToArray();
			}
		}
	}
}
