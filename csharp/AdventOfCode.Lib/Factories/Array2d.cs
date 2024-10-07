namespace AdventOfCode.Lib.Factories
{
    public static class Array2d
    {
        public static T[,] Create<T>(int width, int height, T defaultValue = default!)
        {
            var array = new T[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    array[y, x] = defaultValue;
                }
            }

            return array;
        }

        public static T[,] Create<T>(int width, int height, Func<Point2, T> map)
        {
            var array = new T[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    array[y, x] = map(new(x, y));
                }
            }

            return array;
        }

        public static IEnumerable<Point2> Range(int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    yield return new Point2(x, y);
                }
            }
        }
    }
}
