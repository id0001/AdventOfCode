namespace AdventOfCode2019.IntCode.Core
{
	public partial class Cpu
	{
		private bool Add()
		{
			var a = GetValue(0);
			var b = GetValue(1);
			var dest = GetAddress(2);

			memory.Write(dest, a + b);

			ip += 4;
			return false;
		}

		private bool Multiply()
		{
			var a = GetValue(0);
			var b = GetValue(1);
			var dest = GetAddress(2);

			memory.Write(dest, a * b);

			ip += 4;
			return false;
		}

		private bool Input()
		{
			var dest = GetAddress(0);
			if (!inputBuffer.TryDequeue(out long input))
				return true;

			memory.Write(dest, input);

			ip += 2;
			return false;
		}

		private bool Output()
		{
			var value = GetValue(0);
			outputCallback.Invoke(value);

			ip += 2;
			return false;
		}

		private bool JumpIfTrue()
		{
			var a = GetValue(0);
			var b = GetValue(1);

			ip = a != 0 ? b : ip + 3;

			return false;
		}

		private bool JumpIfFalse()
		{
			var a = GetValue(0);
			var b = GetValue(1);

			ip = a == 0 ? b : ip + 3;

			return false;
		}

		private bool LessThan()
		{
			var a = GetValue(0);
			var b = GetValue(1);
			var dest = GetAddress(2);

			memory.Write(dest, a < b ? 1 : 0);

			ip += 4;
			return false;
		}

		private bool Equals()
		{
			var a = GetValue(0);
			var b = GetValue(1);
			var dest = GetAddress(2);

			memory.Write(dest, a == b ? 1 : 0);

			ip += 4;
			return false;
		}

		private bool AjustRelativeBase()
		{
			var a = GetValue(0);
			relativeBase += a;

			ip += 2;
			return false;
		}
	}
}
