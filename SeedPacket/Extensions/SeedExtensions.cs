using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;
using System.Collections;

namespace SeedPacket.Extensions
{
    public static class SeedExtensions 
    {
        // ====================================================================================================
        // Seed IEnumerable Versions
        // ====================================================================================================

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, int count)
        {
            return Seed(iEnumerable, 1, count);
        }

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, IGenerator generator)
        {
            return Seed(iEnumerable, 1, 10, generator);
        }

        public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin, int seedEnd, string filePath)
        {
            var seedCore = new SeedCore(
                    new MultiGenerator(filePath) { SeedBegin = seedBegin, SeedEnd = seedEnd }
                );
            return seedCore.SeedList(iEnumerable);
        }

        public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin = 1, int seedEnd = 10, IGenerator generator = null)
        {
            var gen = generator ?? new MultiGenerator();
            gen.SeedBegin = seedBegin;
            gen.SeedEnd = seedEnd;

            return new SeedCore(gen).SeedList(iEnumerable);
        }

        // ====================================================================================================
        // Seed IDictionary Versions
        // ====================================================================================================

        public static IEnumerable Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int count) 
        {
            return Seed(iDictionary, 1, count);
        }

        public static IEnumerable Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, IGenerator generator)
        {
            return Seed(iDictionary, 1, 10, generator);
        }

        public static IEnumerable Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int seedBegin, int seedEnd, string filePath)
        {
            var seedCore = new SeedCore(
                    new MultiGenerator(filePath) { SeedBegin = seedBegin, SeedEnd = seedEnd }
                );
            return seedCore.SeedList(iDictionary);
        }

        public static IEnumerable Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int seedBegin = 1, int seedEnd = 10, IGenerator generator = null)
        {
            var gen = generator ?? new MultiGenerator();
            gen.SeedBegin = seedBegin;
            gen.SeedEnd = seedEnd;

            return new SeedCore(gen).SeedList(iDictionary);
        }
    }
} 

 
