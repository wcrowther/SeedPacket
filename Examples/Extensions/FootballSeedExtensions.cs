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
        // CURRENTLY NOT BEING USED // ARE CURRENTLY USING JUST A NEW FOOTBALLGENERATOR
        // THIS IS FOR WRAPPING ALL THIS LOGIC IF DESIRED.

        static readonly string footballSourcePath = HostingEnvironment.MapPath("/SourceFiles/FootballSource.xml");

        public static List<T> SeedSeason<T>(this IEnumerable<T> iEnumerable, DateTime seasonStartDate, int? randomSeed = null)
        {

            var gen = new FootballGenerator_Old(seasonStartDate, footballSourcePath)
            {
                CustomName = "Game",
                SeedBegin = 1,
                SeedEnd = 64,
                BaseRandom = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random(),
                BaseDateTime = seasonStartDate
            };

            return new SeedCore(gen).SeedList(iEnumerable).ToList();
        }
    }
}
