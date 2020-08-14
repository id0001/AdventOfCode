using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataStructures
{
	public class PriorityQueue<TItem>
	{
		private readonly IList<TItem> heap = new List<TItem>();
		private readonly IComparer<TItem> comparer;

		public PriorityQueue(IComparer<TItem> comparer)
		{
			this.comparer = comparer;
		}

		public PriorityQueue(IEnumerable<TItem> items, IComparer<TItem> comparer)
		{
			this.comparer = comparer;

			// Add all elements to the heap
			foreach(var item in items)
			{
				heap.Add(item);
			}

			// Heapify process, O(n)
			for (int i = Math.Max(0, (heap.Count / 2) - 1); i >= 0; i--)
				Sink(i);
		}

		public int Count => heap.Count;

		public void Clear() => heap.Clear();

		public bool IsEmpty => Count == 0;

		public void Enqueue(TItem item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			heap.Add(item);

			// Heapify
			int indexOfLastItem = heap.Count - 1;
			Swim(indexOfLastItem);
		}

		public TItem Dequeue()
		{
			if (IsEmpty)
				throw new InvalidOperationException("The heap is empty.");

			return RemoveAt(0);
		}

		public TItem Peek()
		{
			if (IsEmpty)
				throw new InvalidOperationException("The heap is empty.");

			return heap[0];
		}

		private void Swim(int a)
		{
			// Get parent index 
			int parent = (a - 1) / 2;

			while (a > 0 && lessThan(a,parent))
			{
				// Swap value of a with parent
				Swap(parent, a);

				// a becomes parent
				a = parent;

				// Grab parent of a
				parent = (a - 1) / 2;
			}
		}

		private void Sink(int a)
		{
			while(true)
			{
				int left = 2 * a + 1; // Left node: 2i + 1 where i == parent index.
				int right = 2 * a + 2; // Right node: 2i + 2 where i == parent index.
				int smallest = left; // Assume left is smallest first.

				// Find the smallest child.
				if (right < Count && lessThan(right, left)) smallest = right;

				// Stop if we're outside the bounds of the tre or stop early if we cannot sink a anymore.
				if (left >= Count || lessThan(a, smallest)) break;

				// Swap the nodes
				Swap(smallest, a);
				a = smallest;
			}
		}

		private void Swap(int a, int b) => (heap[a], heap[b]) = (heap[b], heap[a]);

		private bool lessThan(int a, int b) => comparer.Compare(heap[a], heap[b]) <= 0;

		private TItem RemoveAt(int i)
		{
			if (IsEmpty) return default;

			int indexOfLastItem = Count - 1;
			TItem removed = heap[i];

			// Swap item at i with last item
			Swap(i, indexOfLastItem);

			// Remove the last item
			heap.RemoveAt(indexOfLastItem);

			// Check if last item was removed.
			if (i == indexOfLastItem) 
				return removed;

			TItem item = heap[i];

			// Try sinking item
			Sink(i);

			// If sinking didn't work try swimming.
			if (heap[i].Equals(item))
				Swim(i);

			return removed;
		}
	}
}
