namespace AdventOfCode.Lib.Assembly
{
    public interface IMemory
    {
        int Ip { get; set; }

        void Clear();
    }
}
