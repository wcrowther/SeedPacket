using NewLibrary.ForObject;
using NewLibrary.ForString;
using NewLibrary.ForType;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
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

        public IEnumerable<T> SeedList<T> (IEnumerable<T> iEnumerable) where T : new()
        {
            var seedList = new List<T>();
            var cachedRules = new Dictionary<int, Rule>();
            bool isFirstRow = true;

            DebugWrite("-----------------------------------------------------");
            DebugWrite("Begin Seed Creation for Type: " + typeof(T).Name);
            DebugWrite("-----------------------------------------------------");

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                generator.RowNumber = rowNumber;
                generator.GetNextRowRandom();

                var newRow = CreateRow<T>(generator, cachedRules, isFirstRow);

                seedList.Add(newRow);
                isFirstRow = false;
            }

            DebugWrite("-----------------------------------------------------");

            iEnumerable = seedList;
            return iEnumerable;
        }

        private T CreateRow<T> ( IGenerator generator, Dictionary<int, Rule> cachedRules, bool isFirstRow) where T : new()
        {
            var metaType = typeof(T).GetMetaModel();
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

                DebugWrite($"Property: {property.Name }({property.PropertyType}) using rule: {rule.RuleName ?? "No matching Rule"}.");
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

        private void DebugWrite (string str)
        {
            if (generator.Debugging)
            {
                Debug.WriteLine(str);
            }
        }
    }
}
