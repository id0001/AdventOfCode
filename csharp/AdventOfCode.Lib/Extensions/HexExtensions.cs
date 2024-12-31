namespace AdventOfCode.Lib;

public static class HexExtensions
{
    public static IEnumerable<Hex> GetNeighbors(this Hex source)
    {
        yield return source.North;
        yield return source.NorthEast;
        yield return source.SouthEast;
        yield return source.South;
        yield return source.SouthWest;
        yield return source.NorthWest;
    }
}