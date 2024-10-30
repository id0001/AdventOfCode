namespace AdventOfCode.Lib.Assembly;

public interface IMemory
{
    int Ip { get; }

    void Clear();
}