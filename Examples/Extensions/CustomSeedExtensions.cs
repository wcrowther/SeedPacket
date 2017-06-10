using SeedPacket;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Generators;

namespace Examples.Extensions
{
    public static class CustomSeedExtensions
    {
        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string propertyName = null)
        {
            var gen = new CustomGenerator("~/SourceFiles/xmlSeedSourcePlus.xml", dataInputType: DataInputType.XmlFile)
            {
                SeedBegin = seedBegin ?? 1,
                SeedEnd = seedEnd ?? 10,
                BaseRandom = new Random(randomSeed ?? 3456),
                BaseDateTime = DateTime.Now,
                CurrentPropertyName = propertyName
            };
            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }

        public static T SeedOne<T>(this T type, int? randomSeed = null) where T : new()
        {
            return new List<T>().Seed(seedEnd: 1, randomSeed: randomSeed).Single();
        }
    }
}