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
        private List<FootballTeam> footballTeams;
        private List<FootballGame> seasonGames;

        public FootballGenerator(DateTime baseDateTime, string sourceFilepath)
                    : base(sourceFilepath, dataInputType: DataInputType.XmlFile, baseDateTime: baseDateTime)
        {
            // FirstSunday of Football Season
            if (BaseDateTime.DayOfWeek != DayOfWeek.Sunday)
                throw new Exception("BaseDateTime must be set to the first Sunday of the season.");
        }

        protected override void GetRules(RulesSet ruleSet)
        {
            Rules.AddBasicRules();
            Rules.AddCommonRules();
            FootballRules();
        }

        GetOneFromCacheRandom<FootballTeam>

        public void FootballRules()
        {
            var footballRules = new List<Rule>()
            {
                new Rule(typeof(FootballGame), "",   g => GetObjectNext(g), "Get Football Game w/ 2 FootballTeams"),
                new Rule(typeof(FootballTeam), "",   g => GetTeam(g), "Get Football Team")
            };
            Rules.AddRange(footballRules);

            footballTeams = new List<FootballTeam>().Seed(1, 32, this, "Team").ToList();
            seasonGames = GetAllGames();

            SeedEnd = seasonGames.Count; // set this to however many games in the season.
        }

        private List<FootballGame> GetAllGames()
        {
            var games = new List<FootballGame>();

            games.AddRange(GenerateDivisionGames());
            games.AddRange(GenerateInConferenceGames(1)); // confId 1: AFC
            games.AddRange(GenerateInConferenceGames(2)); // confId 2: NFC
            games.AddRange(GenerateOutOfConferenceGames());

            return games;
        }

        private List<FootballGame> GenerateDivisionGames()
        {
            var divisionGames =
                    from home in footballTeams
                    from away in footballTeams
                    where home.Name != away.Name
                          && home.DivId == away.DivId
                          && home.ConfId == away.ConfId
                    select new FootballGame
                    {
                        HomeTeam = home,
                        AwayTeam = away,
                        GameType = GameType.Division,
                        SeasonStartYear = this.BaseDateTime.Year,
                        GameDate = this.BaseDateTime
                    };

            return divisionGames.ToList();
        }

        private List<FootballGame> GenerateInConferenceGames(int confId)
        {
            var games = new List<FootballGame>();

            int offset = this.BaseDateTime.Year;
            var divIds = new List<int> { 1, 2, 3, 4 };
            var offsetIds = divIds.TakeNext(4, offset).ToArray();

            // =====================================================================

            var firstSecondGames = footballTeams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[0])
                    .OrderByDescending(o => o.TeamId)
                    .SelectMany(t1 => footballTeams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[1])
                    .Select((t2, index) =>
                new FootballGame
                {
                    HomeTeam = (index % 2 == 0) ? t1 : t2,
                    AwayTeam = (index % 2 == 0) ? t2 : t1,
                    GameId = index,
                    SeasonStartYear = BaseDateTime.Year,
                    GameType = GameType.InConference
                }
            ));

            games.AddRange(firstSecondGames.ToList().EqualizeHomeAndAway());

            // =====================================================================

            var thirdFourthGames = footballTeams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[2])
                    .SelectMany(t1 => footballTeams
                    .Where(w => w.ConfId == confId && w.DivId == offsetIds[3])
                    .Select((t2, index) =>
               new FootballGame
               {
                   HomeTeam = t1,
                   AwayTeam = t2,
                   GameId = index,
                   SeasonStartYear = BaseDateTime.Year,
                   GameType = GameType.InConference
               }
            ));

            games.AddRange(thirdFourthGames.ToList().EqualizeHomeAndAway());

            // =====================================================================

            var extraInGames = GenerateExtraInConferenceGames(confId, offsetIds);

            games.AddRange(extraInGames);

            return games;
        }

        private List<FootballGame> GenerateExtraInConferenceGames(int confId, int[] offsetIds)
        {
            var games = new List<FootballGame>();

            //----------------------------------------------------------------------------------------
            // firstDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var firstThirdGames = GetExtraInConferenceGames(confId, offsetIds[0], offsetIds[2]);
            games.AddRange(firstThirdGames);

            var firstFourthGames = GetExtraInConferenceGames(confId, offsetIds[3], offsetIds[0]);
            games.AddRange(firstFourthGames);

            //----------------------------------------------------------------------------------------
            // secondDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var secondThirdGames = GetExtraInConferenceGames(confId, offsetIds[2], offsetIds[1]);
            games.AddRange(secondThirdGames);

            var secondFourthGames = GetExtraInConferenceGames(confId, offsetIds[1], offsetIds[3]);
            games.AddRange(secondFourthGames);

            //----------------------------------------------------------------------------------------

            return games;
        }

        private IEnumerable<FootballGame> GetExtraInConferenceGames(int confId, int homeDivId, int awayDivId)
        {
            return from home in footballTeams.Where(w => w.ConfId == confId && w.DivId == homeDivId)
                   from away in footballTeams.Where(w => w.ConfId == confId && w.DivId == awayDivId)
                   where home.Rank == away.Rank
                   select new FootballGame
                   {
                       HomeTeam = home,
                       AwayTeam = away,
                       GameType = GameType.ExtraConference,
                       SeasonStartYear = BaseDateTime.Year
                   };
        }

        private List<FootballGame> GenerateOutOfConferenceGames()
        {
            var games = new List<FootballGame>();

            int offset = BaseDateTime.Year + 4;

            var divIds = new List<int> { 1, 2, 3, 4 };                      // For Conference 1
            var offsetIds = divIds.TakeNext(4, offset, false).ToArray();    // For Conference 2

            foreach (int id in divIds)
            {
                games.AddRange(GetOutOfConferenceGames(divIds[id - 1], offsetIds[id -1]));
            }

            return games;
        }

        private List<FootballGame> GetOutOfConferenceGames(int conf1DivId, int conf2DivId)
        {
            return footballTeams
                    .Where(w => w.ConfId == 1 && w.DivId == conf1DivId)
                    .SelectMany((t1, index1) => footballTeams
                    .Where(w => w.ConfId == 2 && w.DivId == conf2DivId)
                    .Select((t2, index2) =>
                new FootballGame
                {
                    HomeTeam = EqualizeVenue(index1, index2, t1, t2),
                    AwayTeam = EqualizeVenue(index1, index2, t2, t1),
                    GameId = index1,
                    SeasonStartYear = BaseDateTime.Year,
                    GameType = GameType.OutOfConference
                }
            )).ToList();
        }

        private  FootballTeam EqualizeVenue(int index1, int index2, FootballTeam t1, FootballTeam t2, string note = "home")
        {
            return (index1 % 2 == 1) ? (index2 % 2 == 0 ? t1 : t2) : (index2 % 2 == 0 ? t2 : t1);
        }

        //private FootballGame GetCachedGame(IGenerator g)
        //{
        //    return Funcs.GetOneFromCacheRandom<FootballGame>(g, Cache.SeasonGames, true);
        //}

        //private FootballGame CreateGame(IGenerator g)
        //{
        //    var game = new FootballGame
        //    {
        //        HomeTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(g, Cache.FootballTeams, true),
        //        AwayTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(g, Cache.FootballTeams, true)
        //    };
        //    return game;
        //}

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


