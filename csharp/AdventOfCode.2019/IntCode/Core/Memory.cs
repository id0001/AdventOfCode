namespace AdventOfCode2019.IntCode.Core
{
	public class Memory
	{
		private long[] _memory;
		private readonly int _size;

		public Memory(int size = 100000)
		{
			_size = size;
			_memory = Array.Empty<long>();
		}

		public void Clear()
		{
			_memory = new long[_size];
		}

		public void LoadProgram(long[] program)
		{
			if (program.Length > _size)
				throw new ArgumentException("Program is larger than available memory.");

			_memory = new long[_size];
			Array.Copy(program, 0, _memory, 0, program.Length);
		}

		public long Read(long address)
		{
			if (address < 0 || address > _memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			return _memory[address];
		}

		public void Write(long address, long value)
		{
			if (address < 0 || address > _memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			_memory[address] = value;
		}

		public void Write(long address, long[] value)
		{
			if (address < 0 || address > _memory.Length)
				throw new ArgumentException("Memory address is out of range.");

			if (address + value.Length > _memory.Length)
				throw new ArgumentException("Block does not fit on the specified location.");

			Array.Copy(value, 0, _memory, address, value.Length);
		}
	}
}
