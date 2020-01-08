using System;


namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Memory class TODO: Describe class here
	/// </summary>
	internal class Memory
	{
		private int[] _memory;

		public Memory()
		{
		}

		public void Clear()
		{
			_memory = new int[0];
		}

		public void Load(int[] memory)
		{
			_memory = new int[memory.Length];
			Array.Copy(memory, 0, _memory, 0, _memory.Length);
		}

		public void Load(int[] block, int destinationIndex)
		{
			if(destinationIndex < 0 || destinationIndex + block.Length > _memory.Length)
			{
				throw new InvalidOperationException(@"Block does not fit on the specified location.");
			}

			Array.Copy(block, 0, _memory, destinationIndex, _memory.Length);
		}

		public int Read(int address)
		{
			if(address < 0 || address > _memory.Length)
			{
				throw new InvalidOperationException(@"Memory address out of range");
			}

			return _memory[address];
		}

		public void Write(int address, int value)
		{
			if (address < 0 || address > _memory.Length)
			{
				throw new InvalidOperationException(@"Memory address out of range");
			}

			_memory[address] = value;
		}
	}
}
