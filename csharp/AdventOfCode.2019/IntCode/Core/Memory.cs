using System;

namespace AdventOfCode2019.IntCode.Core
{
	public class Memory
	{
		private long[] memory;
		private readonly int size;

		public Memory(int size = 100000)
		{
			this.size = size;
		}

		public void Clear()
		{
			memory = new long[size];
		}

		public void LoadProgram(long[] program)
		{
			if (program.Length > size)
				throw new ArgumentException("Program is larger than available memory.");

			memory = new long[size];
			Array.Copy(program, 0, memory, 0, program.Length);
		}

		public long Read(long address)
		{
			if (address < 0 || address > memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			return memory[address];
		}

		public void Write(long address, long value)
		{
			if (address < 0 || address > memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			memory[address] = value;
		}

		public void Write(long address, long[] value)
		{
			if (address < 0 || address > memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			if (address + value.Length > memory.Length)
				throw new ArgumentException("Block does not fit on the specified location.");

			Array.Copy(value, 0, memory, address, value.Length);
		}
	}
}
