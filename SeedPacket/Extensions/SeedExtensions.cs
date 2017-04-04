using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;

namespace SeedPacket.Extensions
{
    public static class SeedExtensions 
    {
        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, int count) where T : new()
        {
            return Seed(iEnumerable, 1, count);
        }

        public static IEnumerable<T> Seed<T>(this IEnumerable<T> iEnumerable, IGenerator generator) where T : new()
        {
            return Seed(iEnumerable, 1, 10, generator);
        }

        public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin, int seedEnd, string filePath) where T : new()
        {
            var seedCore = new SeedCore(
                    new MultiGenerator(filePath) { SeedBegin = seedBegin, SeedEnd = seedEnd }
                );
            return seedCore.SeedList(iEnumerable);
        }

        public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin = 1, int seedEnd = 10, IGenerator generator = null) where T : new()
        {
            var gen = generator ?? new MultiGenerator();
            gen.SeedBegin = seedBegin;
            gen.SeedEnd = seedEnd;

            return new SeedCore(gen).SeedList(iEnumerable);
        }
    }
} 

 
