using Examples.Models;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;
using Examples.Extensions;
using Examples.Helpers;

namespace Examples.Generators
{
    public class FootballGenerator : MultiGenerator
    {
        DateTime _seasonStartDate;

        public FootballGenerator( DateTime seasonStartDate, string sourceFilepath = null) 
            : base(sourceFilepath, dataInputType: DataInputType.XmlFile)
        {
            _seasonStartDate = seasonStartDate;
        }

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
                new Rule(typeof(FootballGame), "",   g => GetCachedGame(g), "Get Football Game w/ 2 FootballTeams"),
                new Rule(typeof(FootballTeam), "",   g => GetTeam(g), "Get Football Team")
            };
            Rules.AddRange(footballRules, true);

            Cache.FootballTeams = new List<FootballTeam>().Seed(1, 32, this, "Team");
            Cache.SeasonGames = GetAllGames(this, _seasonStartDate);
        }

        private List<FootballGame> GetAllGames(IGenerator g, DateTime startDate)
        {
            ExpandoObject cache = g.Cache;
            var teams = cache.Get<List<FootballTeam>>("FootballTeams").ToList();
            var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
                                 .ToDictionary(x => x.Key, x => x.ToList());

            var games = new List<FootballGame>();
            games.AddRange(GetDivisionGames(g, teams));
            games.AddRange(GetInConferenceGames(g, teams, startDate));
            //games.AddRange(GetExtraInConferenceGames(g, teams, startDate));
            //games.AddRange(GetOutOfConferenceGames(g, teams, startDate));
            //games.AddRange(GetByeWeekGames(g, teams, startDate));

            return games;
        }

        private List<FootballGame> GetDivisionGames(IGenerator g, List<FootballTeam> teams)
        {
            var divisionGames =
                    from home in teams
                    from away in teams
                    where home.Name != away.Name
                        && home.DivId == away.DivId
                        && home.ConfId == away.ConfId
                    select new FootballGame
                    {
                        HomeTeam = home,
                        AwayTeam = away,
                        GameType = GameType.Division,                      
                    };

            return divisionGames.ToList();
        }

        private List<FootballGame> GetInConferenceGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        {
            var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
                                 .ToDictionary(x => x.Key, x => x.ToList());

            var numbers = new List<int> { 1, 2, 3, 4 };
            int offset = seasonStart.Year;

            int firstDiv = numbers.TakeNext().Single();
            int secondDiv = numbers.TakeNext(offset: offset).Single();
            int thirdDiv = numbers.TakeNext().Single();
            int fourthDiv = numbers.TakeNext().Single();

            return new List<FootballGame>();
        }


        //private List<FootballGame> OldGetInConferenceGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        //{
        //    // ElementIn() will wrap back to the beginning if index gets out of the range.
        //    int[] divIds = { 1, 2, 3, 4 };

        //    var inConferenceGames =
        //            from home in teams
        //            from away in teams
        //            where  home.DivId == divIds.ElementIn(away.DivId + 1)
        //                   && home.ConfId == away.ConfId
        //                   && home.ConfId == 1
        //                   && home.DivId == 2
        //            select new FootballGame
        //            {
        //                HomeTeam = home,
        //                AwayTeam = away,
        //                GameType = GameType.InConference
        //            };

        //   return inConferenceGames.DistinctBy(game => new { game.HomeTeam, game.AwayTeam } ).ToList();
        //}

        private List<FootballGame> GetExtraInConferenceGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        {
            return new List<FootballGame>();
        }

        private List<FootballGame> GetOutOfConferenceGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        {
            return new List<FootballGame>();
        }

        private List<FootballGame> GetByeWeekGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        {
            return new List<FootballGame>();
        }

        private FootballGame GetCachedGame(IGenerator g)
        {
            return Funcs.GetOneFromCacheRandom<FootballGame>(g, Cache.SeasonGames, true) ?? new FootballGame();
        }

        private FootballGame CreateGame(IGenerator g)
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

    public enum GameType
    {
        Division,
        InConference,
        ExtraConference,
        OutOfConference,
        Bye
    }
}


