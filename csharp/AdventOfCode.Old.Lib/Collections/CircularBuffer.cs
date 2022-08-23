using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Collections
{
	public class CircularBuffer<T> : IList<T>, IReadOnlyList<T>
	{
		private int size;
		private T[] buffer;
		private int position;

		public CircularBuffer(int size)
		{
			this.size = size;
			buffer = new T[size];
		}

		public T this[int index]
		{
			get => buffer[(position + index) % size];
			set => buffer[(position + index) % size] = value;
		}

		public int Count => size;

		bool ICollection<T>.IsReadOnly => false;

		public void Add(T item)
		{
			buffer[position] = item;
			position++;
			if (position == size)
				position = 0;
		}

		public void Clear()
		{
			for (int i = 0; i < size; i++)
				buffer[i] = default(T);

			position = 0;
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < size; i++)
			{
				if (buffer[i].Equals(item))
					return true;
			}

			return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(buffer.Skip(position).Union(buffer.Take(position)).ToArray(), 0, array, arrayIndex, size);
		}

		public IEnumerator<T> GetEnumerator() => buffer.Skip(position).Union(buffer.Take(position)).GetEnumerator();

		public int IndexOf(T item)
		{
			for (int i = 0; i < size; i++)
			{
				if (buffer[i].Equals(item))
					return (position + i) % size;
			}

			return -1;
		}

		public void Insert(int index, T item)
		{
			if (index < 0 || index >= size)
				throw new ArgumentOutOfRangeException(nameof(index));

			buffer[(index + position) % size] = item;
		}

		public bool Remove(T item)
		{
			int i = IndexOf(item);
			if (i < 0)
				return false;

			buffer[i] = default(T);
			return true;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= size)
				throw new ArgumentOutOfRangeException(nameof(index));

			buffer[index] = default(T);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
