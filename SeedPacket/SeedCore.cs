using NewLibrary.ForObject;
using NewLibrary.ForString;
using NewLibrary.ForType;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedPacket
{
    public class SeedCore
    {
        private IGenerator generator; 

        public SeedCore (IGenerator generator = null) {
            this.generator = generator ?? new MultiGenerator(); 
        }

        public IEnumerable<T> SeedList<T> (IEnumerable<T> iEnumerable) 
        {
            DebugSeedType(typeof(T).Name);

            if (typeof(T).GetConstructor(Type.EmptyTypes) != null)
            {
                iEnumerable = CreateComplexTypeList<T>();
            }
            else
            {
                iEnumerable = CreateValueTypeList<T>();
            }

            DebugLineWrite();

            return iEnumerable;
        }

        public Dictionary<TKey, TValue> SeedList<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            DebugSeedType($"Dictionary<{typeof(TKey).Name}, {typeof(TValue).Name}>");

            dictionary = CreateDictionaryList<TKey, TValue>();

            DebugLineWrite();

            return dictionary;
        }

        private List<T> CreateValueTypeList<T>()
        {
            var seedList = new List<T>();
            Rule rule = generator.Rules.GetRuleByTypeAndName(typeof(T), "");

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
            var dictionary = new Dictionary<TKey, TValue>();
            var metaModel = typeof(TValue).GetMetaModel();
            var cachedRules = new Dictionary<int, Rule>();
            Rule keyRule = generator.Rules.GetRuleByTypeAndName(typeof(TKey), "");
            Rule simpleTypeRule = generator.Rules.GetRuleByTypeAndName(typeof(TValue), "");

            bool isFirstRow = true;

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                TKey seedKey = keyRule.ApplyRule(generator);
                TValue seedValue;

                if (typeof(TValue).GetConstructor(Type.EmptyTypes) != null)
                {
                    seedValue = CreateComplexClassRow<TValue>(generator, metaModel, cachedRules, isFirstRow);
                }
                else
                {
                    seedValue = simpleTypeRule.ApplyRule(generator);
                }

                dictionary.Add(seedKey, seedValue);

                isFirstRow = false;
            }

            return dictionary;
        }

        private List<T> CreateComplexTypeList<T>() 
        {
            var seedList = new List<T>();
            var metaModel = typeof(T).GetMetaModel();
            var cachedRules = new Dictionary<int, Rule>();
            bool isFirstRow = true;

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                var newRow = CreateComplexClassRow<T>(generator, metaModel, cachedRules, isFirstRow);
                seedList.Add(newRow);

                isFirstRow = false;
            }
            return seedList;
        }

        private T CreateComplexClassRow<T> ( IGenerator generator, MetaModel metaType, Dictionary<int, Rule> cachedRules, bool isFirstRow)  
        {
            var metaProperties = metaType.GetMetaProperties();

            dynamic newItem = Activator.CreateInstance(typeof (T)); // OR new T();

            for (int i = 0; i <= metaProperties.Count - 1; i++)
            {
                var property = metaProperties[i];
                generator.CurrentProperty = property;

                if (!property.CanWrite)
                    continue;

                var rule = GetRuleForProperty(generator, i, cachedRules, isFirstRow);

                SetPropertyValue(newItem, property, rule, generator);
            }
            generator.CurrentRowValues.Clear();  // Dictionary of other values in this seed row. Cleared after loop.

            return newItem;
        }

        private Rule GetRuleForProperty (IGenerator generator, int propInt, Dictionary<int, Rule> cachedRules, bool isFirstRow)
        {
            Rule rule;
            var property = generator.CurrentProperty;

            if (isFirstRow) // Cache rules for first row
            {
                rule = generator.Rules.GetRuleByTypeAndName(property.PropertyType, property.Name);
                cachedRules.Add(propInt, rule);

                DebugWrite($"Property: {property.Name }({property.PropertyType}) using rule: {rule?.RuleName ?? "No matching Rule"}.");
            }
            else
            {
                rule = cachedRules[propInt];
            }
            return rule;
        }

        private void SetPropertyValue (dynamic newItem, MetaProperty property, Rule rule, IGenerator generator)
        {
            if (property != null && rule != null)
            {
                dynamic seedValue = rule.ApplyRule(generator);
                property.SetInstanceValue(seedValue, newItem);
                generator.CurrentRowValues.Add(property.Name, seedValue);
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
}
