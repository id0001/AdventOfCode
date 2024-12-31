namespace AdventOfCode.Lib;

public static class Point3Extensions
{
    public static IEnumerable<Point3> GetNeighbors(this Point3 source, bool includeDiagonal)
    {
        for (var z = -1; z <= 1; z++)
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
        {
            if (!includeDiagonal && !((x == 0 && y == 0) ^ (x == 0 && z == 0) ^ (y == 0 && z == 0)))
                continue;

            if (x == 0 && y == 0 && z == 0)
                continue;

            yield return new Point3(source.X + x, source.Y + y, source.Z + z);
        }
    }
}