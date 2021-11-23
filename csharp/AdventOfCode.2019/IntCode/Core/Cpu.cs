using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2019.IntCode.Core
{
    public partial class Cpu
    {
        private readonly IDictionary<OpCode, Action> instructions;
        private readonly ConcurrentQueue<long> inputBuffer;
        private readonly Memory memory;

        private long[] program;
        private long ip = 0;
        private long relativeBase = 0;
        private bool isRunning;
        private bool waitingForInput;
        private bool manualMode = false;

        private Action<long> outputCallback;
        private Action inputCallback;

        private TaskCompletionSource<long> taskCompletionSource;

        public long Result { get; private set; }

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

        public void RegisterInput(Action callback)
        {
            inputCallback = callback;
        }

        public void WriteInput(long value)
        {
            inputBuffer.Enqueue(value);
            if (waitingForInput)
            {
                waitingForInput = false;
                if (!manualMode)
                    Task.Run(RunUntilHaltOrInput);
            }
        }

        public void SetProgram(long[] program)
        {
            if (isRunning)
                throw new InvalidOperationException("Cannot load program while running.");

            this.program = program;
        }

        public Task<long> StartAsync(params long[] input)
        {
            if (isRunning)
                throw new InvalidOperationException("Cpu is already running.");

            manualMode = false;
            taskCompletionSource = new TaskCompletionSource<long>();
            isRunning = true;
            memory.LoadProgram(program);
            relativeBase = 0;
            ip = 0;

            foreach (var v in input)
                inputBuffer.Enqueue(v);

            Task.Run(RunUntilHaltOrInput);
            return taskCompletionSource.Task;
        }

        public void Start(params long[] input)
        {
            if (isRunning)
                throw new InvalidOperationException("Cpu is already running.");

            manualMode = true;
            isRunning = true;
            memory.LoadProgram(program);
            relativeBase = 0;
            ip = 0;

            foreach (var v in input)
                inputBuffer.Enqueue(v);
        }

        public bool Next()
        {
            if (!isRunning)
                return false;

            var opCode = GetOpCode();
            if (opCode == OpCode.Halt)
            {
                inputBuffer.Clear();
                isRunning = false;
                return false;
            }

            ExecuteInstruction(opCode);
            return true;
        }

        public void Halt()
        {
            isRunning = false;
            Result = memory.Read(0);
            if (!manualMode)
                taskCompletionSource.SetResult(memory.Read(0));
        }

        private void RunUntilHaltOrInput()
        {
            while (isRunning)
            {
                var opCode = GetOpCode();
                if (opCode == OpCode.Halt)
                {
                    inputBuffer.Clear();
                    isRunning = false;
                    taskCompletionSource.SetResult(memory.Read(0));
                    break;
                }

                ExecuteInstruction(opCode);

                if (waitingForInput)
                    return;
            }
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

        private void ExecuteInstruction(OpCode opCode) => instructions[opCode].Invoke();

        private IDictionary<OpCode, Action> InitializeInstructions() => new Dictionary<OpCode, Action>
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
