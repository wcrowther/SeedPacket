using SeedPacket;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Generators;
using System.IO;
using System.Web.Hosting;

namespace Examples.Extensions
{
    public static class CustomSeedExtensions
    {
        private static readonly int defaultSeed = 3456;
        private static readonly string xmlSourcePath;

        static CustomSeedExtensions()
        {
            xmlSourcePath = HostingEnvironment.MapPath("~/SourceFiles/xmlSeedSourcePlus.xml");
        }

        public static List<T> Seed<T>(this IEnumerable<T> iEnumerable, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
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

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>(this IDictionary<TKey, TValue> iDictionary, int? seedEnd = null, int? seedBegin = null, int? randomSeed = null, string customPropertyName = null)
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

        public static T SeedOne<T>(this T type, int? randomSeed = null) where T : new()
        {
            return new List<T>().Seed(seedEnd: 1, randomSeed: randomSeed).Single();
        }

        // MOVE TO WILDHARE
        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek, bool includeCurrentDate = false)
        {
            if (includeCurrentDate && date.DayOfWeek == dayOfWeek)
                return date;

            return date.AddDays(7 - (int)date.DayOfWeek);
        }
    }
}
