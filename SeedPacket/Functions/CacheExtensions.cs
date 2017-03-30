using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static class CacheExtensions 
    {
        /// <summary>Will randomly take X number of items from the sourceList and put them in the new destinationList</summary>
        public static IList<T> TakeRandomItems<T> (this IList<T> sourceList, Random random, int count = 1, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("Error: " + nameof(sourceList) + " is null.");
            }

            var destinationList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(sourceList.Count);
                T element = sourceList.ElementAtOrDefault(index);
                if (element == null)
                {
                    break;
                }
                else
                {
                    destinationList.Add(element);
                    if (remove)
                    {
                        sourceList.Remove(element);
                    }
                }
            }
            return destinationList;
        }

        public static T GetByItemName<T> (this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>) expando;

            return dictionary.ContainsKey(name) ? (T) dictionary[name] : default(T);
        }

        // FOR ROW DICTIONARY CONVERT DICTIONARY TO DYNAMIC SIMILAR to CACHE??
        public static TVal Get<TKey, TVal> (this Dictionary<TKey, TVal> dictionary, TKey key, TVal defaultVal = default(TVal))
        {
            TVal val;
            if (dictionary.TryGetValue(key, out val))
            {
                return val;
            }
            return defaultVal;
        }

    }
} 

 
