namespace AdventOfCode.Lib.Assembly
{
    public sealed record Instruction<TRegister>(string OpCode, IReadOnlyDictionary<string, string> Arguments)
        where TRegister : IParsable<TRegister>;
}
