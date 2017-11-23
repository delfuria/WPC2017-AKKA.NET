using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Library.Demo
{
    public static class DictionaryExtensions
    {
        // Either Add or overwrite
        public static void AddOrUpdate<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
        {
            dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }
        // Either Add or overwrite
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            dictionary[key] = value;
        }
    }
}
