namespace AdventOfCode.Lib.Assembly;

public interface IMemory<TProgram>
{
    int Ip { get; }

    IList<TProgram> Program { get; }

    void Clear();
}