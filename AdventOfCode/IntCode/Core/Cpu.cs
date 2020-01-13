using System;
using System.Collections.Generic;

namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Cpu class TODO: Describe class here
	/// </summary>
	internal partial class Cpu
	{
		private readonly IDictionary<OpCode, Func<int>> _instructions;
		private readonly Memory _memory;
		private long _ip;
		private long _relativeBase;

		public Cpu()
		{
			_memory = new Memory();
			_instructions = InitInstructions();
		}

		public bool IsHalted => State == ExecutionState.Halted;

		public ExecutionState State { get; private set; } = ExecutionState.Halted;

		public IntReader In { get; set; }

		public IntWriter Out { get; set; }

		public long ExitCode => _memory.Read(0);

		public void LoadProgram(long[] program)
		{
			if (State != ExecutionState.Halted)
			{
				throw new InvalidOperationException(@"Cannot load program while executing.");
			}

			_memory.Load(program);
			Reset();
		}

		public void Reset()
		{
			if (State != ExecutionState.Halted)
			{
				throw new InvalidOperationException(@"Cannot reload while executing.");
			}

			_ip = 0;
			_relativeBase = 0;
			In.Clear();
			Out.Clear();
		}

		public void Run()
		{
			State = ExecutionState.Running;
		}

		public void Next()
		{
			var opCode = GetOpCode();
			if (opCode == OpCode.Halt)
			{
				Halt();
				return;
			}

			int size = ExecuteInstruction(opCode);
			_ip += size;
		}

		public void Halt()
		{
			State = ExecutionState.Halted;
		}

		private void RequireAction()
		{
			State = ExecutionState.ActionRequired;
		}

		private OpCode GetOpCode() => (OpCode)(_memory.Read(_ip) % 100);

		private ParameterMode GetParameterMode(int offset)
		{
			long m = _memory.Read(_ip);
			m = (m - m % 100) / 100;

			return (ParameterMode)(Math.Floor(m / Math.Pow(10, offset)) % 10);
		}

		private long GetParameter(int offset, bool forWrite = false)
		{
			long parameter = _memory.Read(_ip + offset + 1);
			ParameterMode mode = GetParameterMode(offset);

			return mode switch
			{
				ParameterMode.Immediate => parameter,
				ParameterMode.Relative => forWrite ? parameter + _relativeBase : _memory.Read(parameter + _relativeBase),
				_ => forWrite ? parameter : _memory.Read(parameter)
			};
		}

		private int ExecuteInstruction(OpCode opCode)
		{
			return _instructions[opCode].Invoke();
		}


		private IDictionary<OpCode, Func<int>> InitInstructions() => new Dictionary<OpCode, Func<int>>
		{
			{ OpCode.Add, Add  },
			{ OpCode.Multiply, Multiply },
			{ OpCode.Input, Input },
			{ OpCode.Output, Output },
			{ OpCode.JumpIfTrue, JumpIfTrue },
			{ OpCode.JumpIfFalse, JumpIfFalse },
			{ OpCode.LessThan, LessThan },
			{ OpCode.Equals, Equals },
			{ OpCode.AjustRelativeBase, AjustRelativeBase }
		};
	}
}
