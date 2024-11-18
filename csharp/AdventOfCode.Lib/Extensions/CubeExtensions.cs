namespace AdventOfCode.Lib;

public static class CubeExtensions
{
    public static IEnumerable<Cube> Subdivide(this Cube cube, int divisor)
    {
        for (var z = cube.Front; z < cube.Back; z += cube.Depth / divisor)
        for (var y = cube.Top; y < cube.Bottom; y += cube.Height / divisor)
        for (var x = cube.Left; x < cube.Right; x += cube.Width / divisor)
            yield return new Cube(x, y, z, cube.Width / divisor, cube.Height / divisor, cube.Depth / divisor);
    }
}