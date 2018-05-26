using SeedPacket;
using System.Collections.Generic;
using System.Linq;
using Examples.Generators;
using System;

namespace Examples.FootballExtensions
{
    public static class FootballSeedExtensions
    {
        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
        {
            var gen = new FootballGenerator("~/SourceFiles/FootballSource.xml")
            {
                CustomName = customPropertyName,
                SeedBegin = seedBegin ?? 1,
                SeedEnd = seedEnd ?? 32,
                BaseRandom = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random()
            };

            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
        {
            var gen = new FootballGenerator("~/SourceFiles/FootballSource.xml")
            {
                CustomName = customPropertyName,
                SeedBegin = seedBegin ?? 1,
                SeedEnd = seedEnd ?? 32,
                BaseRandom = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random()
            };

            return new SeedCore(gen).SeedList(iDictionary);
        }
    }
}