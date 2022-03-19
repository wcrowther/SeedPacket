using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Generators
{
    public class CustomGenerator : MultiGenerator
    {
        public CustomGenerator( string sourceFilepath = null, 
                                string sourceString = null, 
                                DataInputType dataInputType = DataInputType.Auto,
                                RulesSet rulesSet = RulesSet.Advanced
            ) 
            : base(sourceFilepath, sourceString, dataInputType, rulesSet)
        {
        }

        // Can create custom sets of Rules
        protected override void GetRules(RulesSet ruleSet)
        {
            switch (ruleSet)
            {
                case RulesSet.None:
                    // No rules loaded. Add rules manually
                    break;

                case RulesSet.Basic:
                    Rules.AddBasicRules();
                    break;

                case RulesSet.Common:
                    Rules.AddBasicRules();
                    Rules.AddCommonRules();
                    break;

                case RulesSet.Advanced:  // <-- Used by default
                    Rules.AddBasicRules();
                    Rules.AddCommonRules();
                    AddAdvancedRules(Rules);
                    break;

                case RulesSet.UnitTest:
                    Rules.AddBasicRules();
                        // --> Can Add or change Rules here
                    break;

                case RulesSet.Custom:
                    Rules.AddBasicRules();
                    Rules.AddCommonRules();
                        // --> Can Add or change Rules here
                    break;

                default:
                    throw new NotImplementedException("That is not a valid RulesSet.");
            }
        }
        
        // Example. Obviously this could be more extensive...
        public static void AddAdvancedRules(IRules rules, bool overwrite = true)
        {
            var advanceRules = new List<Rule>
            {
                new Rule(typeof(DateTime),  "Create%",      g => g.BaseDateTime.AddDays(g.RowRandom.Next(-30, 1)),                  "DateTimeInLastMonth"),
                new Rule(typeof(string),    "Description%", g => g.GetElementRandom("Description"),                                 "Description", "Gets Description from custom XML file"),
                new Rule(typeof(string),    "Ceo",          g => GetCeoName(g), "Random CEO Name"),
            };

            rules.AddRange(advanceRules, overwrite);
        }

        // Can break out rules into separate methods for easier debugging or if Rule is complex.
        private static dynamic GetCeoName(IGenerator g)
        {
            return $"{g.GetElementNext("FirstName")} {g.GetElementNext("LastName")}";
        }
    }
}


