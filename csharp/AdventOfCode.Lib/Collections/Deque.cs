using Microsoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Lib.Collections
{
    [DebuggerTypeProxy(typeof(Deque<>.DequeDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    public class Deque<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
    {
        private const int MinimumGrow = 4;
        private const int GrowFactor = 2;

        private T[] _array;

        private int _head; // The index from which to remove or add if the deque isn't empty.
        private int _tail; // The index from which to remove or add if the deque isn't empty.
        private int _version;
        private int _size; // Amount of items in the deque.

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
            int center = capacity / 2;
            _tail = center;
            _head = center;
        }

        public Deque(IEnumerable<T> backCollection)
            : this(16)
        {
            Requires.NotNull(backCollection, nameof(backCollection));

            AddRangeLast(backCollection);
        }

        public Deque(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
            : this(16)
        {
            Requires.NotNull(backCollection, nameof(backCollection));
            Requires.NotNull(frontCollection, nameof(frontCollection));

            AddRangeLast(backCollection);
            AddRangeFirst(frontCollection);
        }

        public int Capacity => _array.Length;

        public int Count => _size;

        public bool IsEmpty => _size == 0;

        public IEnumerable<T> Reversed { get; }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public void AddFirst(T item)
        {
            int insertIndex = IsEmpty ? _head : _head - 1;

            if (insertIndex < 0)
            {
                int newCapacity = (int)Math.Max((_array.LongLength * (long)GrowFactor), _array.Length + MinimumGrow);
                SetCapacity(newCapacity);
                insertIndex = IsEmpty ? _head : _head - 1;
            }

            _array[insertIndex] = item;
            _head = insertIndex;
            _size++;
            _version++;
        }

        public void AddLast(T item)
        {
            int insertIndex = IsEmpty ? _tail : _tail + 1;

            if (insertIndex == _array.Length)
            {
                int newCapacity = (int)Math.Max((_array.LongLength * (long)GrowFactor), _array.Length + MinimumGrow);
                SetCapacity(newCapacity);
                insertIndex = IsEmpty ? _tail : _tail + 1;
            }

            _array[insertIndex] = item;
            _tail = insertIndex;
            _size++;
            _version++;
        }

        public void AddRangeFirst(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                AddFirst(item);
            }
        }

        public void AddRangeLast(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                AddLast(item);
            }
        }

        public void Clear()
        {
            if (_size != 0)
            {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                {
                    Array.Clear(_array, _head, _size);
                }

                _size = 0;
            }

            int center = _array.Length / 2;
            _head = center;
            _tail = center;
            _version++;
        }

        public T PeekFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("The collection is empty.");

            return _array[_head];
        }

        public T PeekLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("The collection is empty.");

            return _array[_tail];
        }

        public T PopFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("The collection is empty.");

            T removed = _array[_head];
            _array[_head] = default;

            _size--;
            if (!IsEmpty)
                _head++;

            _version++;

            return removed;
        }

        public T PopLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("The collection is empty.");

            T removed = _array[_tail];
            _array[_tail] = default;

            _size--;
            if (!IsEmpty)
                _tail--;

            _version++;

            return removed;
        }

        public T[] ToArray()
        {
            if (IsEmpty)
                return Array.Empty<T>();

            T[] arr = new T[_size];
            Array.Copy(_array, _head, arr, 0, _size);

            return arr;
        }

        public void CopyTo(T[] array, int index)
        {
            Requires.NotNull(array, nameof(array));
            Requires.Range(index >= 0 && index < array.Length, nameof(index));
            Requires.Argument(array.Length - index >= Count, null, "Offset and length are too small.");

            if (IsEmpty)
                return;

            Array.Copy(_array, _head, array, index, _size);
        }

        private void SetCapacity(int newCapacity)
        {
            T[] oldArray = _array;
            _array = new T[newCapacity];

            int ca = (int)((_tail + _head) / 2f);
            int cb = (int)(newCapacity / 2f);

            int newHead = cb - ca;
            int newTail = IsEmpty ? newHead : newHead + _size - 1;

            Array.Copy(oldArray, _head, _array, newHead, _size);

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
                Array.Copy(_array, _head, array, index, _size);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type.", nameof(array));
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly Deque<T> _source;
            private readonly int _version;
            private int _index;
            private T _currentElement;

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

                    return _currentElement;
                }
            }

            object IEnumerator.Current => Current;

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
                Debug.Assert(_index == -1 || _index == -2);
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
            public T[] Items => _deque.ToArray();
        }
    }
}
