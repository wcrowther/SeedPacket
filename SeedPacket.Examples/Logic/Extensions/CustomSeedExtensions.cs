using SeedPacket.Examples.Logic.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Examples.Logic.Extensions
{
    public static class CustomSeedExtensions
    {
        private static readonly int defaultSeed = 3456;

        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, string xmlSourcePath, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
        {
            var gen = new CustomGenerator(xmlSourcePath, dataInputType: DataInputType.XmlFile)
            {
                SeedBegin = seedBegin ?? 1,
                SeedEnd = seedEnd ?? 10,
                BaseRandom = new Random(randomSeed ?? defaultSeed),
                BaseDateTime = DateTime.Now,
                CustomName = customPropertyName
            };
            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, string xmlSourcePath, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
        {
            var gen = new CustomGenerator(xmlSourcePath, dataInputType: DataInputType.XmlFile)
            {
                SeedBegin = seedBegin ?? 1,
                SeedEnd = seedEnd ?? 10,
                BaseRandom = new Random(randomSeed ?? defaultSeed),
                BaseDateTime = DateTime.Now,
                CustomName = customPropertyName
            };

            return new SeedCore(gen).SeedList(iDictionary);
        }
    }
}
