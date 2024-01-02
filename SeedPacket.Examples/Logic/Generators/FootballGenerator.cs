using SeedPacket.Examples.Logic.Exceptions;
using SeedPacket.Examples.Logic.Extensions;
using SeedPacket.Examples.Logic.Helpers;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Examples.Logic.Generators
{
    public class FootballGenerator : MultiGenerator
    {
        public FootballGenerator(DateTime baseDateTime, string sourceFilepath, Random baseRandom = null)
                    : base(sourceFilepath, dataInputType: DataInputType.XmlFile, baseDateTime: baseDateTime, baseRandom: baseRandom)
        {
            // FirstSunday of Football Season
            if (BaseDateTime.DayOfWeek != DayOfWeek.Sunday)
                throw new Exception("BaseDateTime must be set to the first Sunday of the season.");
        }

        private List<ScheduleSlot> ScheduleSlots = GetScheduleSlots();

        public List<FootballTeam> Teams { get; set; }

        public List<FootballGame> Games { get; set; }

        protected override void GetRules(RulesSet ruleSet)
        {
            AddFootballRules();
        }

        private void AddFootballRules()
        {
            var footballRules = new List<Rule>()
            {
                new Rule(typeof(FootballGame), "", g => GetGame(g), "Get Football Game w/ 2 FootballTeams"),
                new Rule(typeof(FootballTeam), "", g => GetTeam(g), "Get Football Team")
            };
            Rules.AddRange(footballRules, true);

            Teams = new List<FootballTeam>().Seed(1, 32, this, "Team").ToList();
            Games = GetAllGames();
        }

        private FootballTeam GetTeam(IGenerator gen)
        {
            var team = gen.GetObjectNext<FootballTeam>("Team");
            team.Schedule = ScheduleSlots.ToList(); // ToList makes it a copy...

            return team;
        }

        private FootballGame GetGame(IGenerator gen)
        {
            return Games.TakeNextOne();
        }

        private List<FootballGame> GetAllGames()
        {
            var games = new List<FootballGame>();

            games.AddRange(GenerateDivisionGames(Teams));
            games.AddRange(GenerateInConferenceGames(Teams));
            games.AddRange(GenerateOutOfConferenceGames(Teams));
            games.AddByeWeeks(this);
            games = games.NumberedGames();

            return games;
        }

        public IEnumerable<FootballGame> GenerateDivisionGames(List<FootballTeam> teams)
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
                    select new FootballGame
                    {
                        HomeTeam = home,
                        AwayTeam = away,
                        GameType = GameType.Division,
                        SeasonStartYear = BaseDateTime.Year,
                    };

            return divisionGames.ToList().AssignGameDates(GameType.Division, this);
        }

        public IEnumerable<FootballGame> GenerateInConferenceGames(List<FootballTeam> teams)
        {
            var games = new List<FootballGame>();

            games.AddRange(GetInConferenceGames_ForEachConference(1, teams)); // confId 1: AFC
            games.AddRange(GetInConferenceGames_ForEachConference(2, teams)); // confId 2: NFC

            return games;
        }

        public IEnumerable<FootballGame> GenerateOutOfConferenceGames(List<FootballTeam> teams)
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
                games.AddRange(GetOutOfConferenceGames_DivisionVsDivision(teams, divIds[id - 1], offsetIds[id - 1]));
            }

            return games.AssignGameDates(GameType.OutOfConference, this);
        }


        // ===========================================================================================
        // PRIVATE METHODS
        // ===========================================================================================

        private static List<ScheduleSlot> GetScheduleSlots(string weekSequence = null)
        {
            var list = new List<ScheduleSlot>();

            string defaultSequence = "i,d,i,d,o,e,d,o,e,i,d,i,d,o,d,o";  //"o,i,d,i,d,o,e,i,o,e,d,d,i,d,o,d";
            string sequence = weekSequence.IsNullOrEmpty() ? defaultSequence : weekSequence;
            var shortTypes = sequence.Split(',').Select(s => s.Trim()).ToArray();

            if (shortTypes.Length != 16)
            {
                throw new InvalidWeekSequenceLength();
            }

            if (shortTypes.Where(w => w == "d").Count() != 6 || shortTypes.Where(w => w == "i").Count() != 4 ||
                shortTypes.Where(w => w == "e").Count() != 2 || shortTypes.Where(w => w == "o").Count() != 4)
            {
                throw new InvalidWeekSequence();
            }

            for (int i = 1; i <= 16; i++)
            {
                var gameType = shortTypes[i-1].ToLower() switch
                {
                    "d" => GameType.Division,
                    "i" => GameType.InConference,
                    "e" => GameType.ExtraInConference,
                    "o" => GameType.OutOfConference,
                    _   => throw new InvalidShortGameType(),
                };
                list.Add(new ScheduleSlot { GameType = gameType, SeasonWeek = i });
            }
            return list;
        }

        private IEnumerable<FootballGame> GetInConferenceGames_ForEachConference(int confId, List<FootballTeam> teams)
        {
            Debug.WriteLine($"GetInConferenceGames_ForEachConference for {confId}.");

            var games = new List<FootballGame>();

            int offset = BaseDateTime.Year;
            var divIds = new List<int> { 1, 2, 3, 4 };
            var offsetIds = divIds.TakeNext(4, offset).ToArray();

            // ===========================================================================================
            // In Conference division 1 plays 2, 3 plays 4 with a yearly offset
            // 4 games per team for 64 total games for the league.
            // ===========================================================================================

            games.AddRange(GetInConferenceGames_DivisionVsDivision(confId, teams, offsetIds[0], offsetIds[1]));

            games.AddRange(GetInConferenceGames_DivisionVsDivision(confId, teams, offsetIds[2], offsetIds[3]));

            // ===========================================================================================
            // In Conference Division Extra  - example:
            // If division 1 plays 2 then the extra games are division 1 playing 3 and 4.
            // Teams play the equally ranked teams from the year before.
            // 2 games per team for 32 total games for the league.
            // ===========================================================================================

            games.AddRange(GetExtraInConferenceGames(teams, confId, offsetIds));

            return games;
        }

        protected IEnumerable<FootballGame> GetExtraInConferenceGames(List<FootballTeam> teams, int confId, int[] offsetIds)
        {
            var games = new List<FootballGame>();

            //----------------------------------------------------------------------------------------
            // firstDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var firstThirdGames = GetExtraInConferenceGames_DivisionVsDivision(teams, confId, offsetIds[0], offsetIds[2]);
            games.AddRange(firstThirdGames);

            var firstFourthGames = GetExtraInConferenceGames_DivisionVsDivision(teams, confId, offsetIds[3], offsetIds[0]);
            games.AddRange(firstFourthGames);

            //----------------------------------------------------------------------------------------
            // secondDiv teams play one game each against thirdDiv, fourthDiv with same ranking 1,2,3,4

            var secondThirdGames = GetExtraInConferenceGames_DivisionVsDivision(teams, confId, offsetIds[2], offsetIds[1]);
            games.AddRange(secondThirdGames);

            var secondFourthGames = GetExtraInConferenceGames_DivisionVsDivision(teams, confId, offsetIds[1], offsetIds[3]);
            games.AddRange(secondFourthGames);

            //----------------------------------------------------------------------------------------

            return games.AssignGameDates(GameType.ExtraInConference, this);
        }

        private IEnumerable<FootballGame> GetInConferenceGames_DivisionVsDivision(int confId, List<FootballTeam> teams, int firstDiv, int secondDiv)
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
                    GameType = GameType.InConference,
                }
            ));

            return games.ToList().AssignGameDates(GameType.InConference, this);
        }

        private IEnumerable<FootballGame> GetExtraInConferenceGames_DivisionVsDivision(List<FootballTeam> teams, int confId, int homeDivId, int awayDivId)
        {
            return from home in teams.Where(w => w.ConfId == confId && w.DivId == homeDivId)
                   from away in teams.Where(w => w.ConfId == confId && w.DivId == awayDivId)
                   where home.Rank == away.Rank
                   select new FootballGame
                   {
                       HomeTeam = home,
                       AwayTeam = away,
                       GameType = GameType.ExtraInConference,
                       SeasonStartYear = BaseDateTime.Year
                   };
        }

        private static IEnumerable<FootballGame> GetOutOfConferenceGames_DivisionVsDivision(List<FootballTeam> teams, int conf1DivId, int conf2DivId)
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
        // =========================================================================================

        private FootballGame CreateGame()
        {
            var game = new FootballGame
            {
                HomeTeam = Teams.TakeRandomOne(RowRandom, true),
                AwayTeam = Teams.TakeRandomOne(RowRandom, true)
            };
            return game;
        }
    }
}

// ====================================================
// CODE NO LONGER USED (BUT POTENTIALLY USEFUL)
// ====================================================


//var divisions = teams.GroupBy(x => new { x.ConfId, x.DivId })
//                     .ToDictionary(x => x.Key, x => x.ToList());


//private int[] weekSequence = { 1, 2 };
//
//public int[] WeekSequence
//{
//    get { return weekSequence; }
//    set { ValidateWeekSequence(value); }
//}
//
//private void ValidateWeekSequence(int[] value)
//{
//    int[] val = value.OrderBy(o => o).ToArray();
//    int[] test = Enumerable.Range(1, 16).ToArray();
//    bool invalid = !test.SequenceEqual(val);
//
//    if (invalid)
//        throw new Exception("Not able to set WeekSequence as it is invalid. It must contain " +
//            "ints 1-16 in any order. Game 17 is currently determined by the Bye week.");
//
//    weekSequence = value;
//}


//public static List<ScheduleSlot> GetScheduleSlots()
//{
//    var list = new List<ScheduleSlot>
//            {
//                new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 1 },
//                new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 2 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 3 },
//                new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 4 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 5 },
//                new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 6 },
//                new ScheduleSlot{ GameType = GameType.ExtraInConference, SeasonWeek = 7 },
//                new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 8 },
//                new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 9 },
//                new ScheduleSlot{ GameType = GameType.ExtraInConference, SeasonWeek = 10 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 11 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 12 },
//                new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 13 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 14 },
//                new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 15 },
//                new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 16 },
//            };

//    return list;
//}


