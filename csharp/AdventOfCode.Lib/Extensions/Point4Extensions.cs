namespace AdventOfCode.Lib
{
    public static class Point4Extensions
    {
        public static IEnumerable<Point4> GetNeighbors(this Point4 source, bool includeDiagonal)
        {
            for (var w = -1; w <= 1; w++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        for (var x = -1; x <= 1; x++)
                        {
                            if (!includeDiagonal && 
                                !(
                                    (x == 0 && y == 0) ^
                                    (x == 0 && z == 0) ^
                                    (y == 0 && z == 0) ^
                                    (x == 0 && w == 0) ^
                                    (y == 0 && w == 0) ^
                                    (z == 0 && w == 0)
                                ))
                                continue;

                            if (x == 0 && y == 0 && z == 0 && w == 0)
                                continue;

                            yield return new Point4(source.X + x, source.Y + y, source.Z + z, source.W + w);
                        }
                    }
                }
            }
        }
    }
}
