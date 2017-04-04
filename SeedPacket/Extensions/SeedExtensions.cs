using SeedPacket.Interfaces;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Dynamic;

namespace SeedPacket.Extensions
{
    public static class SeedExtensions 
    {
        public static IList<T> Seed<T>(this IList<T> iList, int count) where T : new()
        {
            return Seed(iList, 1, count);
        }

        public static IList<T> Seed<T>(this IList<T> iList, IGenerator generator) where T : new()
        {
            return Seed(iList, 1, 10, generator);
        }

        public static IList<T> Seed<T> (this IList<T> iList, int seedBegin, int seedEnd, string filePath) where T : new()
        {
            var seedCore = new SeedCore(
                    new MultiGenerator(filePath) { SeedBegin = seedBegin, SeedEnd = seedEnd }
                );
            return seedCore.SeedList(iList);
        }

        public static IList<T> Seed<T> (this IList<T> iList, int seedBegin = 1, int seedEnd = 10, IGenerator generator = null) where T : new()
        {
            var gen = generator ?? new MultiGenerator();
            gen.SeedBegin = seedBegin;
            gen.SeedEnd = seedEnd;

            return new SeedCore(gen).SeedList(iList);
        }
    }
} 

 
