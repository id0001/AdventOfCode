using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2019.IntCode.Core
{
	public partial class Cpu
	{
		private readonly IDictionary<OpCode, Func<bool>> instructions;
		private readonly ConcurrentQueue<long> inputBuffer;
		private readonly Memory memory;

		private long[] program;
		private long ip = 0;
		private long relativeBase = 0;
		private bool isRunning;

		private Action<long> outputCallback;

		public Cpu()
		{
			memory = new Memory();
			inputBuffer = new ConcurrentQueue<long>();
			instructions = InitializeInstructions();
		}

		public void RegisterOutput(Action<long> callback)
		{
			outputCallback = callback;
		}

		public void WriteInput(long value)
		{
			if (!isRunning)
				throw new InvalidOperationException("Cannot write input while the cpu is not running.");

			inputBuffer.Enqueue(value);
		}

		public void SetProgram(long[] program)
		{
			if (isRunning)
				throw new InvalidOperationException("Cannot load program while running.");

			this.program = program;
		}

		public Task<long> RunAsync(params long[] input) => RunAsync(input, CancellationToken.None);

		public async Task<long> RunAsync(long[] input, CancellationToken cancellationToken)
		{
			if (isRunning)
				throw new InvalidOperationException("Cpu is already running.");

			isRunning = true;
			memory.LoadProgram(program);
			relativeBase = 0;
			ip = 0;

			inputBuffer.Clear();
			foreach (var v in input)
				inputBuffer.Enqueue(v);

			while (!cancellationToken.IsCancellationRequested)
			{
				var opCode = GetOpCode();
				if (opCode == OpCode.Halt)
				{
					break;
				}

				bool interrupt = ExecuteInstruction(opCode);
				if (interrupt)
					await Task.Delay(10);
			}

			isRunning = false;
			return memory.Read(0);
		}

		private OpCode GetOpCode() => (OpCode)(memory.Read(ip) % 100);

		private ParameterMode GetParameterMode(int offset)
		{
			long m = memory.Read(ip);
			m = (m - m % 100) / 100;

			return (ParameterMode)(Math.Floor(m / Math.Pow(10, offset)) % 10);
		}

		//-----------------------------------------------------------------------------------------
		/// <summary>
		/// Get the parameter value at the given offset assuming it is an address.
		/// </summary>
		/// <param name="offset">The offset</param>
		/// <returns>An address</returns>
		private long GetAddress(int offset)
		{
			long parameter = memory.Read(ip + offset + 1);
			ParameterMode mode = GetParameterMode(offset);

			return mode switch
			{
				ParameterMode.Immediate => parameter,
				ParameterMode.Relative => parameter + relativeBase,
				ParameterMode.Positional => parameter,
				_ => throw new NotImplementedException(),
			};
		}

		/// <summary>
		/// Get the parameter value at the given offset.
		/// Read from memory if it's a pointer.
		/// </summary>
		/// <param name="offset">The offset</param>
		/// <returns>A value</returns>
		private long GetValue(int offset)
		{
			long parameter = memory.Read(ip + offset + 1);
			ParameterMode mode = GetParameterMode(offset);

			return mode switch
			{
				ParameterMode.Immediate => parameter, // parameter is value
				ParameterMode.Relative => memory.Read(parameter + relativeBase), // parameter is relative pointer
				ParameterMode.Positional => memory.Read(parameter), // parameter is pointer
				_ => throw new NotImplementedException()
			};
		}

		private bool ExecuteInstruction(OpCode opCode) => instructions[opCode].Invoke();

		private IDictionary<OpCode, Func<bool>> InitializeInstructions() => new Dictionary<OpCode, Func<bool>>
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
