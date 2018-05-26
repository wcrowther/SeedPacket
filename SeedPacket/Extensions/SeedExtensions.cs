

using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;
using System.Collections;
using System;

namespace SeedPacket.Extensions
{
    public static class SeedExtensions
    {
        // ====================================================================================================
        // Seed IEnumerable Versions
        // ====================================================================================================

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, int count, Random random = null)
        {
            return Seed(iEnumerable, 1, count, baseRandom: random);
        }

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, IGenerator generator)
        {
            return new SeedCore(generator).SeedList(iEnumerable);
        }

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, int seedBegin, int seedEnd, string filePath, string customPropertyName = null)
        {
            var seedCore = new SeedCore(
                new MultiGenerator(filePath)
                {
                    SeedBegin = seedBegin,
                    SeedEnd = seedEnd,
                    CustomName = customPropertyName
                }
            );
            return seedCore.SeedList(iEnumerable);
        }

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, int? seedBegin = null, int? seedEnd = null, IGenerator generator = null, string customPropertyName = null, Random baseRandom = null)
        {
            if (generator != null && baseRandom != null)
                throw new Exception("SeedPacket: As you ares passing in an generator to the Seed method, you must set the baseRandom on the IGenerator itself.");

            var gen = generator ?? new MultiGenerator(baseRandom: baseRandom);
            gen.SeedBegin = seedBegin ?? gen.SeedBegin;
            gen.SeedEnd = seedEnd ?? gen.SeedEnd;
            gen.CustomName = customPropertyName;

            return new SeedCore(gen).SeedList(iEnumerable);
        }

        // ====================================================================================================
        // Seed IDictionary Versions
        // ====================================================================================================

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int count, Random random = null)
        {
            return Seed(iDictionary, 1, count, baseRandom: random);
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, IGenerator generator)
        {
            return new SeedCore(generator).SeedList(iDictionary);
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int seedBegin, int seedEnd, string filePath, string customPropertyName = null)
        {
            var seedCore = new SeedCore(
                new MultiGenerator(filePath)
                {
                    SeedBegin = seedBegin,
                    SeedEnd = seedEnd,
                    CustomName = customPropertyName
                }
            );
            return seedCore.SeedList(iDictionary);
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int? seedBegin = null, int? seedEnd = null, IGenerator generator = null, string currentPropertyName = null, Random baseRandom = null)
        {
            if (generator != null && baseRandom != null)
                throw new Exception("SeedPacket: As you ares passing in an generator to the Seed method, you must set the baseRandom on the IGenerator itself.");

            var gen = generator ?? new MultiGenerator(baseRandom: baseRandom);
            gen.SeedBegin = seedBegin ?? 1;
            gen.SeedEnd = seedEnd ?? 10;
            gen.CustomName = currentPropertyName;

            return new SeedCore(gen).SeedList(iDictionary);
        }
    }
}


