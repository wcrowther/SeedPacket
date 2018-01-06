using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using NewLibrary.ForType;

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

        public static T ToOject<T>(this XElement element) where T : class, new()
        {
            try
            {
                T instance = new T();
                var metaModel = new MetaModel(typeof(T), instance);
                foreach (var metaProperty in metaModel.GetMetaProperties())
                {
                    var xattribute = element.Attribute(metaProperty.Name);
                    var xelement = element.Element(metaProperty.Name);
                    var propertyType = Nullable.GetUnderlyingType(metaProperty.PropertyType) ?? metaProperty.PropertyType;
                    var value = xattribute?.Value ?? xelement.Value;

                    try
                    {
                        if (value != null)
                        {
                            if (metaProperty.CanWrite)
                            {
                                metaProperty.SetInstanceValue(Convert.ChangeType(value, propertyType));
                            }
                        }
                    }
                    catch  // If Error let the value remain default for that property type
                    {
                        Console.WriteLine("Not able to parse value " + value +  " for type '" + metaProperty.PropertyType + "' for property " + metaProperty.Name );
                    } 
                }
                return instance;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }
} 

 
