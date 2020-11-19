using System;


namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Memory class TODO: Describe class here
	/// </summary>
	internal class Memory
	{
		private long[] _memory;
		private readonly int _size;

		public Memory(int size = 1000000)
		{
			_size = size;
		}

		public void Clear()
		{
			_memory = new long[_size];
		}

		public void Load(long[] memory)
		{
			_memory = new long[_size];
			Array.Copy(memory, 0, _memory, 0, memory.Length);
		}

		public void Load(long[] block, int destinationIndex)
		{
			if (destinationIndex < 0 || destinationIndex + block.Length > _memory.Length)
			{
				throw new InvalidOperationException(@"Block does not fit on the specified location.");
			}

			Array.Copy(block, 0, _memory, destinationIndex, block.Length);
		}

		public long Read(long address)
		{
			if (address < 0 || address > _memory.Length)
			{
				throw new InvalidOperationException(@"Memory address out of range");
			}

			return _memory[address];
		}

		public void Write(long address, long value)
		{
			if (address < 0 || address > _memory.Length)
			{
				throw new InvalidOperationException(@"Memory address out of range");
			}

			_memory[address] = value;
		}
	}
}
