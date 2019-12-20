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
		private readonly IDictionary<OpCode, Action<int[]>> _instructions;
		private int _ip;
		private int[] _program;
		private int[] _memory;

		public IntCodeComputer()
		{
			_instructions = new Dictionary<OpCode, Action<int[]>>
			{
				{ OpCode.Add, ExecAdd },
				{ OpCode.Multiply, ExecMultiply },
			};
		}

		public void LoadProgram(int[] program)
		{
			_program = program;

			_memory = new int[_program.Length];
			Array.Copy(_program, 0, _memory, 0, _memory.Length);
		}

		public int Execute()
		{
			_ip = 0;

			OpCode opCode;
			int[] parameters;

			int size = ParseInstruction(out opCode, out parameters);
			while (opCode != OpCode.Halt)
			{
				_instructions[opCode].Invoke(parameters);

				_ip += size;
				size = ParseInstruction(out opCode, out parameters);
			}

			return MemRead(0);
		}

		public void MemWrite(int address, int value) => _memory[address] = value;

		public int MemRead(int address) => _memory[address];

		#region Instructions

		private int ParseInstruction(out OpCode opCode, out int[] parameters)
		{
			opCode = (OpCode)MemRead(_ip);
			if (opCode == OpCode.Halt)
			{
				parameters = new int[0];
				return 1;
			}

			parameters = new int[] { MemRead(_ip + 1), MemRead(_ip + 2), MemRead(_ip + 3) };

			return 4;
		}

		private void ExecAdd(int[] parameters)
		{
			int a1 = parameters[0];
			int a2 = parameters[1];
			int a3 = parameters[2];

			MemWrite(a3, MemRead(a1) + MemRead(a2));
		}

		private void ExecMultiply(int[] parameters)
		{
			int a1 = parameters[0];
			int a2 = parameters[1];
			int a3 = parameters[2];

			MemWrite(a3, MemRead(a1) * MemRead(a2));
		}

		#endregion
	}
}
