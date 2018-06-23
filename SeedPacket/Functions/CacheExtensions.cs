using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;


namespace SeedPacket.Functions
{
    // These extension methods are puposefully in the SeedPacket.Functions namespace as they are mostly associated with the functionality here.

    public static class CacheExtensions 
    {
        /// <summary>Will randomly take {count} number of items from the sourceList and return them in the new destinationList</summary>
        public static IList<T> TakeRandomItems<T> (this IList<T> sourceList, Random random, int count = 1, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeRandomItems Error. The Datasource list is null.");
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

        public static void AddItemByName<T>(this ExpandoObject expando, string name, T value)
        {
            var dictionary = (IDictionary<string, object>)expando;

            if (value == null)
                value = default(T);

            if (dictionary.ContainsKey(name))
            {
                dictionary[name] = value;
            }
            else
            {
                dictionary.Add(name, value);
            }
        }

        public static void RemoveItemByName(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>)expando;

            if (dictionary.ContainsKey(name))
            {
                dictionary.Remove(name);
            }
        }

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

 
