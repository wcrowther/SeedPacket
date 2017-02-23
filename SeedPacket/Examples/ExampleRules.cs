using System.Collections.Generic;
using NewLibrary.ForType;
using System.Collections;
using System.Diagnostics;
using System;
using SeedPacket.Interfaces;
using SeedPacket.Generators;
using SeedPacket.Extensions;
using System.Linq;

namespace SeedPacket.Examples
{
    public class ExampleRules
    {
        // Demo Extension Methods
        public static object AlternatingFormat(IGenerator generator)
        {
            string seed = generator.RowNumber.ToString();
            string format1 = generator.Property.Name + "/" + seed;
            string format2 = generator.Property.Name + "_" + seed;

            return (generator.RowNumber % 2 == 0) ? format1 : format2;
        }

        public static string RandomItem(IGenerator generator)
        {
            var names = new[] { "dooHickey", "gadget", "widget", "thingamajig", "gizmo", "whatchamacallit", "doodad", "gimmick", "thingamabob", "whatnot" };
            int index = generator.RowRandom.Next(names.Length); 

            return names[index];
        }

        public static decimal? OccasionalNull(IGenerator generator)
        {
            int diceroll = generator.RowRandom.Next(1, 7);  // 1 to 6
            return (diceroll == 6) ? null : (decimal?) generator.RowNumber;
        }

        public static decimal? NullableDecimal(IGenerator generator)
        {
            int diceroll = generator.RowRandom.Next(1, 7);
            return (diceroll == 6) ? null : (decimal?) generator.RowNumber;
        
        }

        // Match on a List<T> of a specific type
        // var newRule = new Rule(typeof(List<Item>), "", g => ExampleRules.AddItems<Item>(g), "Nested List", "Calls nested seed function on list.");
        public static IEnumerable<T> AddItems<T>(IGenerator generator, int count = 6) where T : new()
        {
            int itemsToAdd = generator.RowRandom.Next(1, count + 1); // adds items 1 to 6

            return new List<T>().Seed(1, itemsToAdd);
        }

        // Match on an interface. This creates five items if IEnumerable (ignoring string).
        // RULE:  new Rule(typeof(IEnumerable<Item>),"", g => ExampleRules.GenerateList(g), "Advanced List", ""),
        //public static IEnumerable GenerateList (IGenerator generator, int number = 5)
        //{
        //    if (generator.Debug)
        //    {
        //        Debug.WriteLine(string.Format("=== BEGIN GenerateList for Row {0} ===", generator.RowNumber));
        //    }
        //    dynamic newItem = Activator.CreateInstance(generator.Property.PropertyType);

        //    // Use a new Rules based on the current row's RowRandom so that values are different in this loop
        //    var nestedGenerator = new MultiGenerator(BaseRandom: generator.RowRandom);

        //    // NOTE: Extension method does not work on dynamics at runtime so use normal function call
        //    var list = SeedExtensions.Seed(newItem, number, generator: nestedGenerator);

        //    return list;
        //}
   }
} 

 
