using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<T, K>(this IDictionary<T, K> source, T key, K value)
        {
            Requires.NotNull(source, nameof(source));

            if (!source.TryAdd(key, value))
                source[key] = value;
        }

        public static void Update<T, K>(this IDictionary<T, K> source, T key, Func<K, K> update, K defaultValue = default)
        {
            Requires.NotNull(source, nameof(source));
            Requires.NotNull(update, nameof(update));

            if (!source.TryGetValue(key, out K oldValue))
            {
                source.Add(key, update(defaultValue));
            }
            else
            {
                source[key] = update(oldValue);
            }
        }
    }
}
