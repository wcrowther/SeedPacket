using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using WildHare;
using WildHare.Extensions;

namespace SeedPacket
{
    public class SeedCore
    {
        private IGenerator generator; 

        public SeedCore (IGenerator generator = null)
        {
            this.generator = generator ?? new MultiGenerator(); 
        }

        public IEnumerable<T> SeedList<T> (IEnumerable<T> iEnumerable) 
        {
            DebugSeedType(typeof(T).Name);

            if (generator.CustomName != null)
            {
                iEnumerable = CreateValueTypeList<T>(); // for named custom types
            }
            else if (typeof(T).GetConstructor(Type.EmptyTypes) != null) // for complex types w/ no CustomName
            {
                iEnumerable = CreateComplexTypeList<T>();
            }
            else
            {
                iEnumerable = CreateValueTypeList<T>();  // for simpleTypes
            }

            DebugLineWrite();

            return iEnumerable;
        }

        public IDictionary<TKey, TValue> SeedList<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            DebugSeedType($"Dictionary<{typeof(TKey).Name}, {typeof(TValue).Name}>");

            dictionary = CreateDictionaryList<TKey, TValue>();

            DebugLineWrite();

            return dictionary;
        }

        private List<T> CreateValueTypeList<T>()
        {
            // CustomName can be set but returns CurrentProperty.Name unless the MetaProperty is null
            // This allows you to pass in a PropertyName to match rules on in single type lists like List<string>.

            var seedList = new List<T>();

            Rule rule = generator.Rules.GetRuleByTypeAndName(typeof(T), generator.CustomName);

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                dynamic seedValue = rule.ApplyRule(generator);
                seedList.Add(seedValue);
            }
            return seedList;
        }

        private Dictionary<TKey, TValue> CreateDictionaryList<TKey, TValue>()

        {
            string keyName, valueName;
            var dictionary = new Dictionary<TKey, TValue>();
            bool isFirstRow = true;

            GetDictionaryNames(out keyName, out valueName);

            Rule keyRule = generator.Rules.GetRuleByTypeAndName(typeof(TKey), keyName);
            Rule valueTypeRule = generator.Rules.GetRuleByTypeAndName(typeof(TValue), valueName);

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                var singleProperty = new SingleProperty<TKey>().GetMetaProperties()[0];
                singleProperty.Name = keyName;
                generator.CurrentProperty = singleProperty;

                TKey seedKey = keyRule.ApplyRule(generator);
                TValue seedValue;


                if (typeof(TValue).GetConstructor(Type.EmptyTypes) != null)
                {
                    seedValue = CreateComplexClassRow<TValue>(generator, isFirstRow);
                }
                else
                {
                    seedValue = valueTypeRule.ApplyRule(generator);
                }

                dictionary.Add(seedKey, seedValue);

                isFirstRow = false;
            }
            return dictionary;
        }

        private void GetDictionaryNames(out string keyName, out string valueName)
        {
            string customName = generator.CustomName ?? "";
            keyName = customName;
            valueName = "";
            if (customName.IndexOf(",") > 0)
            {
                if (generator.CustomName.IndexOf(",") > 1)
                {
                    throw new Exception("Generator.CustomName cannot have more than one comma.");
                }
                var strArray = generator.CustomName.Split(',');
                keyName = strArray[0];
                valueName = strArray[1];
            }
        }

        private List<T> CreateComplexTypeList<T>() 
        {
            var seedList = new List<T>();
            bool isFirstRow = true;

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                var newRow = CreateComplexClassRow<T>(generator, isFirstRow);
                seedList.Add(newRow);

                isFirstRow = false;
            }
            return seedList;
        }

        private T CreateComplexClassRow<T> ( IGenerator generator, bool isFirstRow)  
        {
            var metaProperties = typeof(T).GetMetaProperties();

            dynamic newItem = Activator.CreateInstance(typeof (T)); // OR new T();

            for (int i = 0; i <= metaProperties.Count - 1; i++)
            {
                var property = metaProperties[i];
                generator.CurrentProperty = property;

                if (!property.CanWrite)
                    continue;

                var rule = GetRuleForProperty(generator, i, isFirstRow);

                SetPropertyValue(newItem, property, rule, generator);
            }
            generator.CurrentRowValues.Clear();  // Dictionary of other values in this seed row. Cleared after loop.

            return newItem;
        }

        private Rule GetRuleForProperty (IGenerator generator, int propInt, bool isFirstRow)
        {
            Rule rule;
            var property = generator.CurrentProperty;
            ExpandoObject cache = generator.Cache;

            if (isFirstRow) // Cache rules for first row
            {
                rule = generator.Rules.GetRuleByTypeAndName(property.PropertyType, property.Name);

                cache.AddItemByName("CachedRules." + propInt, rule);

                DebugWrite($"Property: {property.Name }({property.PropertyType}) using rule: {rule?.RuleName ?? "No matching Rule"}.");
            }
            else
            {
                rule = cache.GetByItemName<Rule>("CachedRules." + propInt); 
            }
            return rule;
        }

        private void SetPropertyValue (dynamic newItem, MetaProperty property, Rule rule, IGenerator generator)
        {
            if (property != null && rule != null)
            {
                dynamic seedValue = rule.ApplyRule(generator);
                property.SetInstanceValue(seedValue, newItem);
                generator.CurrentRowValues.Add(property.Name, seedValue); // Possibly use: CacheExtensions.AddItemByName(generator.Cache, "RowValues." + property.Name, seedValue);
            }
        }

        // Utilities

        private void DebugSeedType(string name)
        {
            DebugLineWrite();
            DebugWrite("Begin Seed Creation for Type: " + name);
            DebugLineWrite();
        }

        private void DebugLineWrite()
        {
            DebugWrite("-----------------------------------------------------");
        }

        private void DebugWrite(string str)
        {
            if (generator.Debugging)
            {
                Debug.WriteLine(str);
            }
        }

    }

    public class SingleProperty<T>
    {
        // This class is a work around that GetMetaProperties() 
        // may not return anything if there is a single property
        // but we need a PropertyInfo to construct a MetaProperty
        // Fix in NewLibrary at some point.
        public T Type { get; }
    }

}
