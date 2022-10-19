namespace AdventOfCode2019.IntCode.Core;

public enum OpCode : byte
{
    Nop = 0,
    Add = 1,
    Multiply = 2,
    Input = 3,
    Output = 4,
    JumpIfTrue = 5,
    JumpIfFalse = 6,
    LessThan = 7,
    Equals = 8,
    AjustRelativeBase = 9,
    Halt = 99
}