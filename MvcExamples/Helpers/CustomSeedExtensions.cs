using System;
using System.Collections.Generic;
using SeedPacket.Generators;
using SeedPacket;
using SeedPacket.Interfaces;

namespace MvcExamples.Helpers
{
    public static class SeedExtensions
    {
        public static IEnumerable<T> Seed<T> (this IEnumerable<T> iEnumerable, int seedBegin = 1, int seedEnd = 10) where T : new()
        {
            var rules = new CustomGenerator("~/Helpers/XmlGeneratorSource.xml")
            {
                Debugging = true,
                SeedBegin = seedBegin,
                SeedEnd = seedEnd,
                BaseDateTime = DateTime.Parse("1/1/2020"),
                BaseRandom = new Random(44444)
            };
            var seedPacket = new SeedCore(rules);
            return seedPacket.SeedList(iEnumerable);
        }

        public class CustomGenerator : MultiGenerator
        {
            public CustomGenerator (string sourceFilepath = null, RulesSet rulesSet = RulesSet.Advanced) 
                : base(sourceFilepath, rulesSet: rulesSet )
            {

                // Base multigenerator defaults to Advanced RuleSet
                Rules.AddRange(new List<Rule>{
                    new Rule (typeof(string), "action%", g => NextElement(g, "Action"),                           "Custom Action",    "Fills any string fields named Action"),
                    new Rule (typeof(DateTime), "", g => g.BaseDateTime.AddHours(g.RowRandom.Next(-17521, 17521)),  "Custom DateTime",  "Random DateTime. BaseDateTime +- 2 years by hour. Overrides 'Basic DateTime'")
                });
            }
        }
    }
}
