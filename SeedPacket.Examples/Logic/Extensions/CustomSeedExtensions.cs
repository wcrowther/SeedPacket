using Microsoft.Extensions.Configuration;
using SeedPacket.Examples.Helpers;
using SeedPacket.Examples.Logic.Generators;
using SeedPacket.Examples.Logic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using static WildHare.Extensions.Xtra.XtraExtensions;

namespace SeedPacket.Examples.Logic.Extensions
{
    public static class CustomSeedExtensions
    {
        private static readonly int defaultSeed = 34567;
        private static readonly string sourcePath = AppSettings.Current.CustomXmlSourceFile;
            //$@"{GetApplicationRoot()}\Logic\SourceFiles\xmlSeedSourcePlus.xml";

        public static List<T> Seed<T>(  this IEnumerable<T> iEnumerable, 
                                        int? seedEnd = null, int? seedBegin = null, 
                                        int? randomSeed = null, string customPropertyName = null)
        {
            var gen = CustomGenerator(sourcePath, seedEnd, seedBegin, randomSeed, customPropertyName);

            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }

        public static IDictionary<TKey, TValue> Seed<TKey, TValue>
                                        (   this IDictionary<TKey, TValue> iDictionary,
                                            int? seedEnd = null, int? seedBegin = null,
                                            int? randomSeed = null, string customPropertyName = null)
        {
            var gen = CustomGenerator(sourcePath, seedEnd, seedBegin, randomSeed, customPropertyName);

            return new SeedCore(gen).SeedList(iDictionary);
        }

        private static CustomGenerator CustomGenerator( string xmlSourcePath,
                                                        int? seedEnd, int? seedBegin,
                                                        int? randomSeed, string customPropertyName)
        {
            return new CustomGenerator(xmlSourcePath, dataInputType: DataInputType.XmlFile)
            {
                SeedBegin       = seedBegin ?? 1,
                SeedEnd         = seedEnd ?? 10,
                BaseRandom      = new Random(randomSeed ?? defaultSeed),
                BaseDateTime    = DateTime.Now,
                CustomName      = customPropertyName
            };
        }

        public static T SeedOne<T>(this T type, int? randomSeed = null, string customPropertyName = null) where T : new()
        {
            var gen = new CustomGenerator()
            {
                SeedBegin = 1,
                SeedEnd = 1,
                BaseRandom = new Random(randomSeed ?? defaultSeed),
                CustomName = customPropertyName
            };

            return new SeedCore(gen).SeedList(new List<T>()).First();
        }
    }
}
