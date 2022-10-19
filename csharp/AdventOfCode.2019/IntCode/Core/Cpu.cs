using System.Collections.Concurrent;

namespace AdventOfCode2019.IntCode.Core;

public partial class Cpu
{
    private readonly IDictionary<OpCode, Action> _instructions;
    private readonly ConcurrentQueue<long> _inputBuffer;
    private readonly Memory _memory;

    private long[]? _program;
    private long _ip;
    private long _relativeBase;
    private bool _isRunning;
    private bool _waitingForInput;
    private bool _manualMode;

    private Action<long>? _outputCallback;
    private Action? _inputCallback;

    private TaskCompletionSource<long> _taskCompletionSource;

    public long Result { get; private set; }

    public Cpu()
    {
        _memory = new Memory();
        _inputBuffer = new ConcurrentQueue<long>();
        _instructions = InitializeInstructions();
        _taskCompletionSource = new TaskCompletionSource<long>();
    }

    public void RegisterOutput(Action<long> callback)
    {
        _outputCallback = callback;
    }

    public void RegisterInput(Action callback)
    {
        _inputCallback = callback;
    }

    public void WriteInput(long value)
    {
        _inputBuffer.Enqueue(value);
        if (!_waitingForInput) return;
        
        _waitingForInput = false;
        if (!_manualMode)
            Task.Run(RunUntilHaltOrInput);
    }

    public void SetProgram(long[] program)
    {
        if (_isRunning)
            throw new InvalidOperationException("Cannot load program while running.");

        _program = program;
    }

    public Task<long> StartAsync(params long[] input)
    {
        if (_isRunning)
            throw new InvalidOperationException("Cpu is already running.");

        if (_program == null)
            throw new InvalidOperationException("No program loaded.");

        _manualMode = false;
        _taskCompletionSource = new TaskCompletionSource<long>();
        _isRunning = true;
        _memory.LoadProgram(_program);
        _relativeBase = 0;
        _ip = 0;

        foreach (var v in input)
            _inputBuffer.Enqueue(v);

        Task.Run(RunUntilHaltOrInput);
        return _taskCompletionSource.Task;
    }

    public void Start(params long[] input)
    {
        if (_isRunning)
            throw new InvalidOperationException("Cpu is already running.");

        if (_program == null)
            throw new InvalidOperationException("No program loaded.");

        _manualMode = true;
        _isRunning = true;
        _memory.LoadProgram(_program);
        _relativeBase = 0;
        _ip = 0;

        foreach (var v in input)
            _inputBuffer.Enqueue(v);
    }

    public bool Next()
    {
        if (!_isRunning)
            return false;

        var opCode = GetOpCode();
        if (opCode == OpCode.Halt)
        {
            _inputBuffer.Clear();
            _isRunning = false;
            return false;
        }

        ExecuteInstruction(opCode);
        return true;
    }

    public void Halt()
    {
        _isRunning = false;
        Result = _memory.Read(0);
        if (!_manualMode)
            _taskCompletionSource.SetResult(_memory.Read(0));
    }

    private void RunUntilHaltOrInput()
    {
        while (_isRunning)
        {
            var opCode = GetOpCode();
            if (opCode == OpCode.Halt)
            {
                _inputBuffer.Clear();
                _isRunning = false;
                _taskCompletionSource.SetResult(_memory.Read(0));
                break;
            }

            ExecuteInstruction(opCode);

            if (_waitingForInput)
                return;
        }
    }

    private OpCode GetOpCode() => (OpCode)(_memory.Read(_ip) % 100);

    private ParameterMode GetParameterMode(int offset)
    {
        var m = _memory.Read(_ip);
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
        var parameter = _memory.Read(_ip + offset + 1);
        var mode = GetParameterMode(offset);

        return mode switch
        {
            ParameterMode.Immediate => parameter,
            ParameterMode.Relative => parameter + _relativeBase,
            ParameterMode.Positional => parameter,
            _ => throw new NotImplementedException()
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
        var parameter = _memory.Read(_ip + offset + 1);
        var mode = GetParameterMode(offset);

        return mode switch
        {
            ParameterMode.Immediate => parameter, // parameter is value
            ParameterMode.Relative => _memory.Read(parameter + _relativeBase), // parameter is relative pointer
            ParameterMode.Positional => _memory.Read(parameter), // parameter is pointer
            _ => throw new NotImplementedException()
        };
    }

    private void ExecuteInstruction(OpCode opCode) => _instructions[opCode].Invoke();

    private IDictionary<OpCode, Action> InitializeInstructions() => new Dictionary<OpCode, Action>
    {
        { OpCode.Add, Add },
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