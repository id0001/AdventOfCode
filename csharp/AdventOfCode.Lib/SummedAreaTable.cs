using System.Numerics;

namespace AdventOfCode.Lib
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Summed-area_table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record SummedAreaTable<T> where T : IBinaryInteger<T>
    {
        private readonly T[,] _aux;

        public SummedAreaTable(T[,] sourceTable)
        {
            _aux = Preprocess(sourceTable);
        }

        public T SumQuery(int x, int y, int width, int height)
        {
            int brx = x + width - 1;
            int bry = y + height - 1;

            T res = _aux[bry, brx];

            if (y > 0)
                res -= _aux[y - 1, brx];

            if (x > 0)
                res -= _aux[bry, x - 1];

            if (x > 0 && y > 0)
                res += _aux[y - 1, x - 1];

            return res;
        }

        private T[,] Preprocess(T[,] source)
        {
            var height = source.GetLength(0);
            var width = source.GetLength(1);

            var aux = new T[height, width];

            for (var x = 0; x < width; x++)
                aux[0, x] = source[0, x];

            for (var y = 1; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    aux[y, x] = source[y, x] + aux[y - 1, x];
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 1; x < width; x++)
                {
                    aux[y, x] += aux[y, x - 1];
                }
            }

            return aux;
        }
    }
}
