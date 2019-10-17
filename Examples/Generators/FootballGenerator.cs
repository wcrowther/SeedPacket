using Examples.Models;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System.Collections.Generic;

namespace Examples.Generators
{
    public class FootballGenerator : MultiGenerator
    {
        public FootballGenerator( string sourceFilepath =  null) 
            : base(sourceFilepath, dataInputType: DataInputType.XmlFile)
        {   }

        protected override void GetRules(RulesSet ruleSet)
        {
            Rules.AddBasicRules();
            Rules.AddCommonRules();
            FootballRules();
        }

        public void FootballRules()
        {
            var footballRules = new List<Rule>()
            {
                new Rule(typeof(FootballTeam), "",   g => GetTeam(g), "Get Football Team"),
                new Rule(typeof(FootballGame), "",   g => GetGame(g), "Get Football Game w/ 2 FootballTeams")
            };
            Rules.AddRange(footballRules, true);

            var teams = new List<FootballTeam>().Seed(1, 32, this, "Team");

            Cache.FootballTeams = teams;
        }

        private dynamic GetGame(IGenerator g)
        {
            var game = new FootballGame
            {
                HomeTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(g, Cache.FootballTeams, true),
                AwayTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(g, Cache.FootballTeams, true)
            };
            return game;
        }

        public FootballTeam GetTeam(IGenerator gen)
        {
            return gen.GetObjectNext<FootballTeam>("Team");
        }
    }
}


