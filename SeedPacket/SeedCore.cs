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

        public  IEnumerable<T> SeedList<T> (IEnumerable<T> iEnumerable) where T : new()
        {
            var cachedRules = new Dictionary<int, Rule>();
            var seedList = new List<T>();
            bool isFirstRow = true;

            if (generator.Debugging) {
                Debug.WriteLine("-----------------------------------------------------");
                Debug.WriteLine("Begin Seed Creation for Type: " + typeof(T).Name);
                Debug.WriteLine("-----------------------------------------------------");
            }

            for (int rowNumber = generator.SeedBegin; rowNumber <= generator.SeedEnd; rowNumber++)
            {
                var newRow = CreateRow<T>(generator, rowNumber, cachedRules, isFirstRow);
                seedList.Add(newRow);
                isFirstRow = false;
            }

            if (generator.Debugging)
            {
                Debug.WriteLine("-----------------------------------------------------");
            }

            iEnumerable = seedList;

            return iEnumerable;
        }

        private static T CreateRow<T> ( IGenerator generator, int rowNumber,
                                        Dictionary<int, Rule> cachedRules, bool isFirstRow) where T : new()
        {
            generator.GetNextRowRandom();

            // Create each seed item
            dynamic newItem = Activator.CreateInstance(typeof (T)); //OR new T(); // 
            var metaType = typeof(T).GetMetaModel();
            var metaProperties = metaType.GetMetaProperties();

            for (int propertyNumber = 0; propertyNumber <= metaProperties.Count - 1; propertyNumber++)
            {
                SetSeedPropertyValue<T>(newItem, metaProperties, propertyNumber, isFirstRow, generator, cachedRules, rowNumber);
            }

            // Temp dictionary of other values in this seed. Cleared after loop.
            generator.CurrentRowValues.Clear();

            return newItem;
        }

        private static void SetSeedPropertyValue<T> (dynamic newItem, List<MetaProperty> metaProperties, int propertyNumber, bool isFirstRow,
                                                     IGenerator generator, Dictionary<int, Rule> cachedRules, int rowNumber) where T : new()
        {
            Rule rule = null;
            MetaProperty property = metaProperties[propertyNumber];
            generator.CurrentProperty = property;  // Put property in generator current property

            if (!property.CanWrite)
                return;

            if (isFirstRow) // Cache rules for first row
            {
                rule = generator.Rules.GetRuleByTypeAndName(property.PropertyType, property.Name);
                cachedRules.Add(propertyNumber, rule);

                // Show rule used in console
                string ruleName = (rule.Is() ? rule.RuleName.ifBlank() : "No matching Rule");
                string ruleDebug = string.Format("Property: {0}({1}) using rule: {2}.", property.Name, property.PropertyType, ruleName);
                Debug.WriteLine(ruleDebug); 
            }
            else
            {
                rule = cachedRules[propertyNumber];
                // Debug.WriteLine("using cachedRule " + rule.RuleName);
            }

            if (rule.Is())
            {
                generator.RowNumber = rowNumber;
                dynamic seedValue = rule.ApplyRule(generator);
                property.SetInstanceValue(seedValue, newItem);
                generator.CurrentRowValues.Add(property.Name, seedValue);
            }
        }
    }
}
