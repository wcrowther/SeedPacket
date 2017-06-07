using SeedPacket;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Extensions
{
    public static class CustomSeedExtensions
    {
        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, int seedEnd = 100, string propertyName = "")
        {
            var gen = new MultiGenerator("~/SourceFiles/xmlSeedSourcePlus.xml", dataInputType: DataInputType.XmlFile)
            {
                SeedEnd = seedEnd,
                BaseRandom = new Random(34561),
                CurrentPropertyName = propertyName,
                RowNumber = 444
            };
            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }

        public static T SeedOne<T>(this T type) where T : new()
        {
            return new List<T>().Seed(seedEnd: 1).Single();
        }
    }
}


