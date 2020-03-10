using SeedPacket;
using System.Collections.Generic;
using System.Linq;
using Examples.Generators;
using System;
using System.Web.Hosting;

namespace Examples.FootballExtensions
{
    public static class FootballSeedExtensions
    {
        static readonly string footballSourcePath = HostingEnvironment.MapPath("/SourceFiles/FootballSource.xml");

        public static List<T> SeedSeason<T>(this IEnumerable<T> iEnumerable, DateTime seasonStartDate, int? randomSeed = null)
        {

            var gen = new FootballGenerator(seasonStartDate, footballSourcePath)
            {
                CustomName = "Game",
                SeedBegin = 1,
                SeedEnd = 96,
                BaseRandom = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random()
            };

            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }
    }
}
