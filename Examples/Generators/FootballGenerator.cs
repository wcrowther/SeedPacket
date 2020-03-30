using Examples.Extensions;
using Examples.Helpers;
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

namespace Examples.Generators
{
    public class FootballGenerator : MultiGenerator
    {
        public FootballGenerator( DateTime baseDateTime, string sourceFilepath, Random baseRandom = null) 
                    : base(sourceFilepath, dataInputType: DataInputType.XmlFile, baseDateTime: baseDateTime, baseRandom: baseRandom)
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

        public void FootballRules()
        {
            var footballRules = new List<Rule>()
            {
                new Rule(typeof(FootballGame), "", g => GetCachedGame(g), "Get Football Game w/ 2 FootballTeams"),
                new Rule(typeof(FootballTeam), "", g => GetTeam(g), "Get Football Team")
            };
            Rules.AddRange(footballRules, true);

            Cache.FootballTeams = new List<FootballTeam>().Seed(1, 32, this, "Team");
            Cache.SeasonGames = GetAllGames(this);

            SeedEnd = Cache.SeasonGames.Count; // set this to however many games in the season.
        }

        public FootballTeam GetTeam(IGenerator gen)
        {
            return gen.GetObjectNext<FootballTeam>("Team");
        }

        private FootballGame GetCachedGame(IGenerator g)
        {
            return Funcs.GetOneFromCacheRandom<FootballGame>(g, Cache.SeasonGames, true);
        }

        private List<FootballGame> GetAllGames(IGenerator g)
        {
            ExpandoObject cache = g.Cache;

            var teams = cache.Get<List<FootballTeam>>("FootballTeams").ToList();
            var games = new List<FootballGame>();

            //games.AddRange(GenerateOutOfConferenceGames(teams));
            //games.AddRange(GenerateInConferenceGames(1, teams)); // confId 1: AFC
            //games.AddRange(GenerateInConferenceGames(2, teams)); // confId 2: NFC
            games.AddRange(GenerateDivisionGames(teams));

            return games;
        }

        protected IEnumerable<FootballGame> GenerateDivisionGames(List<FootballTeam> teams)
        {
            // ===========================================================================================
            // In Conference within Division each team plays the others at home & away
            // 6 games per team for 96 total games for the league.
            // ===========================================================================================

            var divisionGames =
                    from home in teams
                    from away in teams
                    where home.Name != away.Name
                          && home.DivId == away.DivId
                          && home.ConfId == away.ConfId
                         //&& home.ConfId == 2
                         // && home.DivId == 1
                    select new FootballGame
                    {
                        HomeTeam = home,
                        AwayTeam = away,
                        GameType = GameType.Division,
                        SeasonStartYear = BaseDateTime.Year
                    };

            return divisionGames.ToList().AssignGameDates(GameType.Division, this);
        }

        protected IEnumerable<FootballGame> GenerateInConferenceGames(int confId, List<FootballTeam> teams)
        {
            var games = new List<FootballGame>();

            int offset = BaseDateTime.Year;
            var divIds = new List<int> { 1, 2, 3, 4 };
            var offsetIds = divIds.TakeNext(4, offset).ToArray();

            // ===========================================================================================
            // In Conference division 1 plays 2, 3 plays 4 with a yearly offset
            // 4 games per team for 64 total games for the league.
            // ===========================================================================================

            games.AddRange(GetInConferenceGames(confId, teams, offsetIds[0], offsetIds[1]) );

            games.AddRange(GetInConferenceGames(confId, teams, offsetIds[2], offsetIds[3]) );

            // ===========================================================================================
            // In Conference Division Extra  - example:
            // If division 1 plays 2 then the extra games are division 1 playing 3 and 4.
            // Teams play the equally ranked teams from the year before.
            // 2 games per team for 32 total games for the league.
            // ===========================================================================================

            games.AddRange(GenerateExtraInConferenceGames(teams, confId, offsetIds));

            return games;
        }


        protected IEnumerable<FootballGame> GenerateExtraInConferenceGames(List<FootballTeam> teams, int confId, int[] offsetIds)
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

            return games.AssignGameDates(GameType.ExtraConference, this);
        }


        protected IEnumerable<FootballGame> GenerateOutOfConferenceGames(List<FootballTeam> teams)
        {
            var games = new List<FootballGame>();

            int offset = BaseDateTime.Year + 4;

            var divIds = new List<int> { 1, 2, 3, 4 };                      // For Conference 1
            var offsetIds = divIds.TakeNext(4, offset, false).ToArray();    // For Conference 2

            // ===========================================================================================
            // Out of Conference - a division plays division from other conference with a yearly offset
            // 4 games per team for 64 total games for the league.
            // ===========================================================================================

            foreach (int id in divIds)
            {
                games.AddRange(GetOutOfConferenceGames(teams, divIds[id - 1], offsetIds[id - 1]));
            }

            return games.AssignGameDates(GameType.OutOfConference, this);
        }

        // ===========================================================================================
        // PRIVATE METHODS
        // ===========================================================================================

        private IEnumerable<FootballGame> GetInConferenceGames(int confId, List<FootballTeam> teams, int firstDiv, int secondDiv)
        {
            var games = teams
                    .Where(w => w.ConfId == confId && w.DivId == firstDiv)
                    .OrderByDescending(o => o.TeamId)
                    .SelectMany((t1, index1) => teams
                    .Where(w => w.ConfId == confId && w.DivId == secondDiv)
                    .Select((t2, index2) =>
                new FootballGame
                {
                    HomeTeam = EqualizeVenue(index1, index2, t1, t2),
                    AwayTeam = EqualizeVenue(index1, index2, t2, t1),
                    GameId = index1,
                    GameType = GameType.InConference,
                }
            ));

            return games.ToList().AssignGameDates(GameType.InConference, this);
        }

        private IEnumerable<FootballGame> GetExtraInConferenceGames(List<FootballTeam> teams, int confId, int homeDivId, int awayDivId)
        {
            return from home in teams.Where(w => w.ConfId == confId && w.DivId == homeDivId)
                   from away in teams.Where(w => w.ConfId == confId && w.DivId == awayDivId)
                   where home.Rank == away.Rank
                   select new FootballGame
                   {
                       HomeTeam = home,
                       AwayTeam = away,
                       GameType = GameType.ExtraConference,
                       SeasonStartYear = BaseDateTime.Year
                   };
        }

        private IEnumerable<FootballGame> GetOutOfConferenceGames(List<FootballTeam> teams, int conf1DivId, int conf2DivId)
        {
            return teams
                    .Where(w => w.ConfId == 1 && w.DivId == conf1DivId)
                    .SelectMany((t1, index1) => teams
                    .Where(w => w.ConfId == 2 && w.DivId == conf2DivId)
                    .Select((t2, index2) =>
                new FootballGame
                {
                    HomeTeam = EqualizeVenue(index1, index2, t1, t2),
                    AwayTeam = EqualizeVenue(index1, index2, t2, t1),
                    GameId = index1,
                    GameType = GameType.OutOfConference
                }
            ));
        }

        private static FootballTeam EqualizeVenue(int index1, int index2, FootballTeam t1, FootballTeam t2, string note = "home")
        {
            return (index1 % 2 == 1) ? (index2 % 2 == 0 ? t1 : t2) : (index2 % 2 == 0 ? t2 : t1);
        }

        // =========================================================================================
        // Not currently used
        private FootballGame CreateGame()
        {
            var game = new FootballGame
            {
                HomeTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(this, Cache.FootballTeams, true),
                AwayTeam = Funcs.GetOneFromCacheRandom<FootballTeam>(this, Cache.FootballTeams, true)
            };
            return game;
        }

    }

}

//var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
//                     .ToDictionary(x => x.Key, x => x.ToList());


