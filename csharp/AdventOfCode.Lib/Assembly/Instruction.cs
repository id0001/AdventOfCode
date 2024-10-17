namespace AdventOfCode.Lib.Assembly;

public sealed record Instruction<TOpCode, TArguments>(TOpCode OpCode, TArguments Arguments);