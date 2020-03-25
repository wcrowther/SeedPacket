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
using System.Diagnostics;

namespace Examples.Generators
{
    public class FootballGenerator : MultiGenerator
    {

        public FootballGenerator( DateTime baseDateTime, string sourceFilepath) 
                    : base(sourceFilepath, dataInputType: DataInputType.XmlFile, baseDateTime: baseDateTime)
        {
            Debug.WriteLine("Creating FootballGenerator");
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
            Cache.SeasonGames = GetAllGames(this);

            SeedEnd = Cache.SeasonGames.Count; // set this to however many games in the season.
        }

        private List<FootballGame> GetAllGames(IGenerator g)
        {
            ExpandoObject cache = g.Cache;
            var teams = cache.Get<List<FootballTeam>>("FootballTeams").ToList();
            var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
                                 .ToDictionary(x => x.Key, x => x.ToList());

            var games = new List<FootballGame>();
            games.AddRange(GenerateDivisionGames(g, teams)); 
            games.AddRange(GenerateInConferenceGames(g, 1, teams)); // confId 1: AFC
            games.AddRange(GenerateInConferenceGames(g, 2, teams)); // confId 2: NFC
            //games.AddRange(GetOutOfConferenceGames(g, teams, startDate));

            return games;
        }

        private List<FootballGame> GenerateDivisionGames(IGenerator g, List<FootballTeam> teams)
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

        private List<FootballGame> GenerateInConferenceGames(IGenerator g, int confId, List<FootballTeam> teams)
        {
            var firstSunday = g.BaseDateTime;
            if (firstSunday.DayOfWeek != DayOfWeek.Sunday)
                throw new Exception("BaseDateTime must be set to the first Sunday of the season.");

            var games = new List<FootballGame>();

            int offset = firstSunday.Year; 
            var divIds = new List<int> { 1, 2, 3, 4 };
            var offsetIds = divIds.TakeNext(4, offset).ToArray();

            Debug.WriteLine($"{(confId == 1 ? "AFC":"NFC")}({confId}) Year({firstSunday.Year}) {offsetIds[0]} {offsetIds[1]} {offsetIds[2]} {offsetIds[3]}");

            // =====================================================================

            var firstSecondGames = teams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[0])
                    .OrderByDescending(o => o.TeamId)
                    .SelectMany(t1 => teams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[1])
                    .Select((t2, index) =>
                new FootballGame
                {
                    HomeTeam = (index % 2 == 0) ? t1 : t2,
                    AwayTeam = (index % 2 == 0) ? t2 : t1,
                    GameId = index,
                    SeasonStartYear = firstSunday.Year,
                    GameType = GameType.InConference
                }
            ));
            games.AddRange(firstSecondGames.ToList().EqualizeHomeAndAway());

            // =====================================================================

            var thirdFourthGames = teams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[2])
                    .SelectMany(t1 => teams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[3])
                    .Select((t2, index) =>
               new FootballGame
               {
                   HomeTeam = t1,
                   AwayTeam = t2,
                   GameId = index,
                   SeasonStartYear = firstSunday.Year,
                   GameType = GameType.InConference
               }
            ));

            games.AddRange(thirdFourthGames.ToList().EqualizeHomeAndAway());

            // =====================================================================

            var extraInGames = GenerateExtraInConferenceGames(teams, confId, offsetIds);

            games.AddRange(extraInGames);

            return games;
        }

        private  List<FootballGame> GenerateExtraInConferenceGames(List<FootballTeam> teams, int confId, int[] offsetIds)
        {
            var games = new List<FootballGame>();

            //----------------------------------------------------------------------------------------
            // firstDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var firstThirdGames = GetExtraInConferenceGames(teams, confId, offsetIds[0], offsetIds[2]);
            games.AddRange(firstThirdGames);

            var firstFourthGames = GetExtraInConferenceGames(teams, confId, offsetIds[3], offsetIds[0]);
            games.AddRange(firstFourthGames);

            //----------------------------------------------------------------------------------------
            // secondDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var secondThirdGames = GetExtraInConferenceGames(teams, confId, offsetIds[2], offsetIds[1]);
            games.AddRange(secondThirdGames);

            var secondFourthGames = GetExtraInConferenceGames(teams, confId, offsetIds[1], offsetIds[3]);
            games.AddRange(secondFourthGames);

            //----------------------------------------------------------------------------------------

            return games;
        }

        private  IEnumerable<FootballGame> GetExtraInConferenceGames(List<FootballTeam> teams, int confId, int homeDivId, int awayDivId)
        {
            return from home in teams.Where(w => w.ConfId == confId && w.DivId == homeDivId)
                   from away in teams.Where(w => w.ConfId == confId && w.DivId == awayDivId)
                   where home.Rank == away.Rank
                   select new FootballGame
                   {
                       HomeTeam = home,
                       AwayTeam = away,
                       GameType = GameType.ExtraConference
                   };
        }

        private List<FootballGame> GenerateOutOfConferenceGames(IGenerator g, List<FootballTeam> teams, DateTime seasonStart)
        {
            return new List<FootballGame>();
        }

        private FootballGame GetCachedGame(IGenerator g)
        {
            return Funcs.GetOneFromCacheRandom<FootballGame>(g, Cache.SeasonGames, true);
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

//var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
//                     .ToDictionary(x => x.Key, x => x.ToList());


