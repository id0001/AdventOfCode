using System;

namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Cpu class TODO: Describe class here
	/// </summary>
	internal partial class Cpu
	{
		private int Add()
		{
			var a = GetParameter(0);
			var b = GetParameter(1);
			var dest = GetParameter(2, true);

			_memory.Write(dest, a + b);

			return 4;
		}

		private int Multiply()
		{
			var a = GetParameter(0);
			var b = GetParameter(1);
			var dest = GetParameter(2, true);

			_memory.Write(dest, a * b);

			return 4;
		}

		private int Input()
		{
			var dest = GetParameter(0, true);

			if (!In.TryRead(out long input))
			{
				RequireAction();
				return 0;
			}

			_memory.Write(dest, input);

			return 2;
		}

		private int Output()
		{
			var value = GetParameter(0);

			Out.Write(value);

			return 2;
		}

		private int JumpIfTrue()
		{
			var a = GetParameter(0);
			var dest = GetParameter(1);

			if (a != 0) // Perform jump
			{
				_ip = dest;
				return 0;
			}

			return 3;
		}

		private int JumpIfFalse()
		{
			var a = GetParameter(0);
			var dest = GetParameter(1);

			if (a == 0) // Perform jump
			{
				_ip = dest;
				return 0;
			}

			return 3;
		}

		private int LessThan()
		{
			var a = GetParameter(0);
			var b = GetParameter(1);
			var dest = GetParameter(2, true);

			_memory.Write(dest, a < b ? 1 : 0);

			return 4;
		}

		private int Equals()
		{
			var a = GetParameter(0);
			var b = GetParameter(1);
			var dest = GetParameter(2, true);

			_memory.Write(dest, a == b ? 1 : 0);

			return 4;
		}

		private int AjustRelativeBase()
		{
			var a = GetParameter(0);
			_relativeBase += a;
			return 2;
		}
	}
}
