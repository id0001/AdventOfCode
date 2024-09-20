namespace AdventOfCode.Lib.Assembly;

public sealed record Instruction<TArguments>(string OpCode, TArguments Arguments);