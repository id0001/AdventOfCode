using Microsoft;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Lib.Collections
{
    [DebuggerTypeProxy(typeof(Deque<>.DequeDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    public class Deque<T> : ICollection, IReadOnlyCollection<T>
    {
        private const int MinimumGrow = 4;
        private const int GrowFactor = 2;

        private T?[] _array;

        private int _head; // The index from which to remove or add if the deque isn't empty.
        private int _tail; // The index from which to remove or add if the deque isn't empty.
        private int _version;

        public Deque()
        {
            _array = Array.Empty<T>();
            _tail = 0;
            _head = 0;
        }

        public Deque(int capacity)
        {
            Requires.Argument(capacity >= 0, nameof(capacity), "Parameter must be greater or equal to 0.");

            _array = new T[capacity];
            var center = capacity / 2;
            _tail = center;
            _head = center;
        }

        public Deque(IEnumerable<T> backCollection)
            : this(16)
        {
            PushRangeBack(backCollection);
        }

        public Deque(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
            : this(16)
        {
            PushRangeBack(backCollection);
            PushRangeFront(frontCollection);
        }

        public int Capacity => _array.Length;

        public int Count { get; private set; }

        public bool IsEmpty => Count == 0;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public void PushFront(T item)
        {
            var insertIndex = IsEmpty ? _head : _head - 1;

            if (insertIndex < 0)
            {
                var newCapacity = (int)System.Math.Max(_array.LongLength * GrowFactor, _array.Length + MinimumGrow);
                SetCapacity(newCapacity);
                insertIndex = IsEmpty ? _head : _head - 1;
            }

            _array[insertIndex] = item;
            _head = insertIndex;
            Count++;
            _version++;
        }

        public void PushBack(T item)
        {
            var insertIndex = IsEmpty ? _tail : _tail + 1;

            if (insertIndex == _array.Length)
            {
                var newCapacity = (int)System.Math.Max(_array.LongLength * GrowFactor, _array.Length + MinimumGrow);
                SetCapacity(newCapacity);
                insertIndex = IsEmpty ? _tail : _tail + 1;
            }

            _array[insertIndex] = item;
            _tail = insertIndex;
            Count++;
            _version++;
        }

        public void PushRangeFront(IEnumerable<T> items)
        {
            foreach (var item in items)
                PushFront(item);
        }

        public void PushRangeBack(IEnumerable<T> items)
        {
            foreach (var item in items)
                PushBack(item);
        }

        public void Clear()
        {
            if (Count != 0)
            {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    Array.Clear(_array, _head, Count);

                Count = 0;
            }

            var center = _array.Length / 2;
            _head = center;
            _tail = center;
            _version++;
        }

        public T PeekFront()
        {
            Verify.Operation(!IsEmpty,"The collection is empty.");

            return _array[_head]!;
        }

        public T PeekBack()
        {
            Verify.Operation(!IsEmpty,"The collection is empty.");

            return _array[_tail]!;
        }

        public T PopFront()
        {
            Verify.Operation(!IsEmpty,"The collection is empty.");

            var removed = _array[_head]!;
            _array[_head] = default;

            Count--;
            if (!IsEmpty)
                _head++;

            _version++;

            return removed;
        }

        public T PopBack()
        {
            Verify.Operation(!IsEmpty,"The collection is empty.");

            var removed = _array[_tail]!;
            _array[_tail] = default;

            Count--;
            if (!IsEmpty)
                _tail--;

            _version++;

            return removed;
        }

        public T[] ToArray()
        {
            if (IsEmpty)
                return Array.Empty<T>();

            var arr = new T[Count];
            Array.Copy(_array, _head, arr, 0, Count);

            return arr;
        }

        public void CopyTo(T[] array, int index)
        {
            Requires.NotNull(array, nameof(array));
            Requires.Range(index >= 0 && index < array.Length, nameof(index));
            Requires.Argument(array.Length - index >= Count, null, "Offset and length are too small.");

            if (IsEmpty)
                return;

            Array.Copy(_array, _head, array, index, Count);
        }

        private void SetCapacity(int newCapacity)
        {
            var oldArray = _array;
            _array = new T[newCapacity];

            var ca = (int)((_tail + _head) / 2f);
            var cb = (int)(newCapacity / 2f);

            var newHead = cb - ca;
            var newTail = IsEmpty ? newHead : newHead + Count - 1;

            Array.Copy(oldArray, _head, _array, newHead, Count);

            _head = newHead;
            _tail = newTail;

            _version++;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Requires.NotNull(array, nameof(array));
            Requires.Argument(array.Rank == 1, nameof(array), $"Parameter has more than 1 dimension.");
            Requires.Argument(array.GetLowerBound(0) == 0, nameof(array), "Parameter has a non zero lower bound.");
            Requires.Range(index >= 0 && index < array.Length, nameof(index));
            Requires.Argument(array.Length - index >= Count, null, "Offset and length are too small.");

            if (IsEmpty)
                return;

            try
            {
                Array.Copy(_array, _head, array, index, Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type.", nameof(array));
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Deque<T> _source;
            private readonly int _version;
            private int _index;
            private T? _currentElement;

            internal Enumerator(Deque<T> source)
            {
                _source = source;
                _version = _source._version;
                _index = -1;
                _currentElement = default;
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                        ThrowEnumerationNotStartedOrEnded();

                    return _currentElement!;
                }
            }

            object IEnumerator.Current => Current!;

            public void Dispose()
            {
                _index = -2;
                _currentElement = default;
            }

            public bool MoveNext()
            {
                Verify.Operation(_version == _source._version, "The collection has been modified.");

                if (_index == -2)
                    return false;

                _index = _index == -1 ? _source._head : _index + 1;

                if (_index == _source._tail + 1)
                {
                    _index = -2;
                    _currentElement = default;
                    return false;
                }

                _currentElement = _source._array[_index];
                return true;
            }

            public void Reset()
            {
                Verify.Operation(_version == _source._version, "The collection has been modified.");

                _index = -1;
                _currentElement = default;
            }

            private void ThrowEnumerationNotStartedOrEnded()
            {
                Debug.Assert(_index is -1 or -2);
                throw new InvalidOperationException(_index == -1 ? $"Enumeration has not started." : $"Enumeration has ended.");
            }
        }

        internal sealed class DequeDebugView
        {
            private readonly Deque<T> _deque;

            public DequeDebugView(Deque<T> deque)
            {
                Requires.NotNull(deque, nameof(deque));

                _deque = deque;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public IEnumerable<T> Items => _deque;
        }
    }
}
