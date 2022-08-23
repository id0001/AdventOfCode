using Microsoft;
using System;
using System.Collections.Generic;

namespace AdventOfCode.Lib.Collections
{
    public class SparseSpatialMap<TKey, TValue> : Dictionary<TKey, TValue> where TKey : IPoint, new()
    {
        private BoundingBox<TKey> bounds = new BoundingBox<TKey>();

        public SparseSpatialMap()
        {
        }

        public BoundingBox<TKey> Bounds => bounds;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            if (!Bounds.Contains(key))
                Bounds.Inflate(key);
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                this[key] = value;
            }
        }

        public void AddOrUpdate(TKey key, Func<TValue, TValue> update, TValue defaultValue = default)
        {
            Requires.NotNull(update, nameof(update));

            if (ContainsKey(key))
            {
                Add(key, update(defaultValue));
            }
            else
            {
                this[key] = update(this[key]);
            }
        }

        public new void Remove(TKey key)
        {
            if (base.Remove(key))
                Bounds.Deflate(Keys, key);
        }

        public TValue GetValue(TKey p, TValue defaultValue = default(TValue))
        {
            if (!ContainsKey(p))
                return defaultValue;

            return this[p];
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetNeighbors(TKey p)
        {
            foreach (TKey neighbor in p.GetNeighbors())
            {
                if (TryGetValue(neighbor, out TValue value))
                {
                    yield return new KeyValuePair<TKey, TValue>(neighbor, value);
                }
            }
        }
    }
}
