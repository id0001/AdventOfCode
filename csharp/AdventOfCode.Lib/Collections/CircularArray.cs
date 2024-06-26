using System.Collections;

namespace AdventOfCode.Lib.Collections
{
    public class CircularArray<T>(int length) : IEnumerable<T>
    {
        private readonly T[] _array = new T[length];

        public CircularArray(T[] source) : this(source.Length)
        {
            Array.Copy(source, _array, source.Length);
        }

        public T this[int index]
        {
            get => _array[index.Mod(length)];
            set => _array[index.Mod(length)] = value;
        }

        public int Length => length;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _array)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(T[] destination, int from, int length)
        {
            for (var i = 0; i < length; i++)
                destination[i] = this[from + i];
        }
    }
}
