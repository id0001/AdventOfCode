
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode.Lib.Collections
{
    public class PriorityQueue<TItem> : IReadOnlyList<TItem>
    {
        private List<TItem> heap;
        private readonly IComparer<TItem> comparer;

        public PriorityQueue(IComparer<TItem> comparer)
        {
            heap = new List<TItem>();
            this.comparer = comparer;
        }

        public PriorityQueue(int capacity, IComparer<TItem> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity should be greater or equal to 0");

            heap = new List<TItem>(capacity);
            this.comparer = comparer;
        }

        public PriorityQueue(IEnumerable<TItem> collection, IComparer<TItem> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            heap = collection.ToList();

            this.comparer = comparer;

            // Heapify process, O(n)
            for (int i = Math.Max(0, (heap.Count / 2) - 1); i >= 0; i--)
                Sink(i);
        }

        public IComparer<TItem> Comparer => comparer ?? Comparer<TItem>.Default;

        public int Count => heap.Count;

        public bool IsEmpty => Count == 0;

        public TItem this[int index] => heap[index];

        public void Clear() => heap.Clear();

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
                throw new InvalidOperationException("The heap is empty");

            return RemoveAt(0);
        }

        public TItem Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("The heap is empty");

            return heap[0];
        }

        public bool Remove(TItem item)
        {
            int index = heap.IndexOf(item);
            if (index < 0)
                return false;

            RemoveAt(index);
            return true;
        }

        private void Sink(int a)
        {
            while (true)
            {
                int left = 2 * a + 1; // Left node: 2i + 1 where i == parent index.
                int right = 2 * a + 2; // Right node: 2i + 2 where i == parent index.
                int smallest = left; // Assume left is smallest first.

                // Find the smallest child.
                if (right < Count && LessThan(right, left)) smallest = right;

                // Stop if we're outside the bounds of the tre or stop early if we cannot sink a anymore.
                if (left >= Count || LessThan(a, smallest)) break;

                // Swap the nodes
                Swap(smallest, a);
                a = smallest;
            }
        }

        private void Swim(int a)
        {
            // Get parent index
            int parent = (a - 1) / 2;

            while (a > 0 && LessThan(a, parent))
            {
                // Swap value of a with parent
                Swap(parent, a);

                // a becomes parent
                a = parent;

                // Grab parent of a
                parent = (a - 1) / 2;
            }
        }

        private TItem RemoveAt(int i)
        {
            if (IsEmpty)
                return default;

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

        private void Swap(int a, int b) => (heap[a], heap[b]) = (heap[b], heap[a]);

        private bool LessThan(int a, int b) => Comparer.Compare(heap[a], heap[b]) <= 0;

        public IEnumerator<TItem> GetEnumerator() => heap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
