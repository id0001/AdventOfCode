//-------------------------------------------------------------------------------------------------
//
// IntCodeComputer.cs -- The IntCodeComputer class.
//
// Copyright (c) 2019 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The IntCodeComputer class TODO: Describe class here
	/// </summary>
	internal class IntCodeComputer
	{
		private const int OpCodeMask = 100;
		private const int ModeMask = 10;

		private readonly IDictionary<OpCode, Func<int>> _instructions;
		private int _ip;
		private int[] _program;
		private int[] _memory;

		public IntCodeComputer()
		{
			_instructions = new Dictionary<OpCode, Func<int>>
			{
				{ OpCode.Add, ExecAdd },
				{ OpCode.Multiply, ExecMultiply },
				{ OpCode.Input, ExecInput },
				{ OpCode.Print, ExecPrint },
				{ OpCode.JumpIfFalse, ExecJumpIfFalse },
				{ OpCode.JumpIfTrue, ExecJumpIfTrue },
				{ OpCode.LessThan, ExecLessThan },
				{ OpCode.Equals, ExecEquals }
			};
		}

		public bool SuspendOnInput { get; set; }

		public Func<int> OnInput { get; set; } = () =>
		{
			int input;
			do
			{
				Console.Write("Enter an integer> ");
			}
			while (!int.TryParse(Console.ReadLine(), out input));
			return input;
		};

		public Action<int> OnPrint { get; set; } = (value) =>
		{
			Console.WriteLine($">>> {value}");
		};

		/// <summary>
		/// Load a program into memory.
		/// </summary>
		/// <param name="program">The program</param>
		public void LoadProgram(int[] program)
		{
			_program = program;

			_memory = new int[_program.Length];
			Array.Copy(_program, 0, _memory, 0, _memory.Length);
		}

		/// <summary>
		/// Execute the program stored in memory.
		/// </summary>
		/// <returns>The result</returns>
		public int Execute()
		{
			_ip = 0;

			OpCode opCode = NextInstruction(0);
			while (opCode != OpCode.Halt)
			{
				opCode = NextInstruction(_instructions[opCode].Invoke());
			}

			return MemRead(0);
		}

		public int Resume()
		{
			OpCode opCode = NextInstruction(_instructions[opCode].Invoke());
			while (opCode != OpCode.Halt)
			{
				opCode = NextInstruction(_instructions[opCode].Invoke());
				if(opCode == OpCode.Input && SuspendOnInput)
				{
					return;
				}
			}
		}

		private void Loop()
		{
			OpCode opCode = NextInstruction(0);
			while (opCode != OpCode.Halt)
			{
				opCode = NextInstruction(_instructions[opCode].Invoke());
				return;
			}
		}

		/// <summary>
		/// Write a value to memory at the specified address.
		/// </summary>
		/// <param name="address">The address to write to</param>
		/// <param name="value">The value</param>
		public void MemWrite(int address, int value) => _memory[address] = value;

		/// <summary>
		/// Read a value from memory at the specified address.
		/// </summary>
		/// <param name="address">The address to read from</param>
		/// <returns>The value at the specified memory location</returns>
		public int MemRead(int address) => _memory[address];

		protected void Suspend()
		{

		}


		/// <summary>
		/// Get a parameter value according to its mode.
		/// </summary>
		/// <param name="offset">The offset from IP+1</param>
		/// <param name="useMode">True if the mode must be used. False to just return the immediate value</param>
		/// <returns>The value of the parameter</returns>
		private int GetParameter(int offset, bool useMode = true)
		{
			int paramValue = MemRead(_ip + offset + 1);
			int mode = useMode ? GetMode(offset) : 1;

			return mode switch
			{
				1 => paramValue, // immediate mode
				_ => MemRead(paramValue) // position mode
			};
		}

		/// <summary>
		/// Get the mode of a parameter.
		/// </summary>
		/// <param name="offset">The offset from IP+1</param>
		/// <returns>The mode of the parameter</returns>
		private int GetMode(int offset)
		{
			int modes = (MemRead(_ip) - (MemRead(_ip) % OpCodeMask)) / 100;
			return (int)Math.Floor(modes / Math.Pow(10, offset)) % ModeMask;
		}

		/// <summary>
		/// Move to the next instruction.
		/// </summary>
		/// <param name="offset">The offset by which to increase the IP</param>
		/// <returns>The next opcode</returns>
		private OpCode NextInstruction(int offset)
		{
			_ip += offset;
			return (OpCode)(MemRead(_ip) % OpCodeMask);
		}

		#region Instructions

		/// <summary>
		/// Add 2 values together and store them in memory.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecAdd()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);
			int p3 = GetParameter(2, false);

			MemWrite(p3, p1 + p2);

			return 4;
		}

		/// <summary>
		/// Multiply 2 values together and store them in memory.
		/// </summary>
		/// <param name="modes">The modes for the parameters</param>
		/// <returns>Size of the instruction</returns>
		private int ExecMultiply()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);
			int p3 = GetParameter(2, false);

			MemWrite(p3, p1 * p2);

			return 4;
		}

		/// <summary>
		/// Ask the use to input an integer and then store it in memory.
		/// </summary>
		/// <param name="modes">The modes for the parameters</param>
		/// <returns>Size of the instruction</returns>
		private int ExecInput()
		{
			int p1 = GetParameter(0, false);

			MemWrite(p1, OnInput());

			return 2;
		}

		/// <summary>
		/// Prints the value from parameter 1 to the console.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecPrint()
		{
			int p1 = GetParameter(0);
			OnPrint(p1);
			return 2;
		}

		/// <summary>
		/// Jump to the address given by parameter 2 if parameter 1 is non-zero.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecJumpIfTrue()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);

			if (p1 != 0)
			{
				_ip = p2;
				return 0;
			}

			return 3;
		}

		/// <summary>
		/// Jump to the address given by parameter 2 if parameter 1 is zero.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecJumpIfFalse()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);

			if (p1 == 0)
			{
				_ip = p2;
				return 0;
			}

			return 3;
		}

		/// <summary>
		/// Store 1 if parameter 1 is less than paramter 2, else store 0 in parameter 3.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecLessThan()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);
			int p3 = GetParameter(2, false);

			MemWrite(p3, p1 < p2 ? 1 : 0);

			return 4;
		}

		/// <summary>
		/// Store 1 if parameter 1 and 2 are equal, else store 0 in parameter 3.
		/// </summary>
		/// <returns>Size of the instruction</returns>
		private int ExecEquals()
		{
			int p1 = GetParameter(0);
			int p2 = GetParameter(1);
			int p3 = GetParameter(2, false);

			MemWrite(p3, p1 == p2 ? 1 : 0);

			return 4;
		}

		#endregion
	}
}
