
using Examples.Models;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WildHare.Extensions;

namespace Examples.Helpers
{
    public static class Extensions 
    {

        public static List<FootballGame> AssignGameDates(this List<FootballGame> games, GameType gameType, IGenerator g, int maxTries = 20)
        {
            for (int i = 1; i <= maxTries; i++)
            {
                try
                {
                    GetGameDates(games, gameType, g, i);

                    break;
                }
                    catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                if (i == 20)
                {
                    throw new Exception($"Maximum tries = {maxTries} to try to find a valid solution for {gameType}. Please use a different seed value or increase maxTries.");
                }
            }
            return games;
        }

        public static List<FootballGame> GetGameDates(this List<FootballGame> games, GameType gameType, IGenerator g, int tries)
        {
            Debug.WriteLine($"Try {tries}. GetGameDates for {gameType}.");

            for (int i = 0; i < games.Count(); i++)
            {
                var game = games[i];

                if (game.SeasonWeek < 0 )
                {
                    var schedule1 = game.HomeTeam.Schedule.Where(w => w.GameType == gameType);
                    var schedule2 = game.AwayTeam.Schedule.Where(w => w.GameType == gameType);

                    var possibleSeasonWeeks =
                        (
                            from s1 in schedule1 
                            from s2 in schedule2
                            where s1.SeasonWeek == s2.SeasonWeek
                            select s1.SeasonWeek

                        ).ToList();

                    int gameWeek = possibleSeasonWeeks.TakeRandomOne(new Random(g.RowRandomNumber + tries));

                    if (gameWeek == 0)
                    {
                        //Debug.WriteLine($"{game.GameType,-15} : Not able to set a GameWeek for {game,-70}" +
                        //  $"{ "(" + game.HomeTeam.Name + ": " + string.Join(" ", game.HomeTeam.Schedule.Where(w => w.GameType == gameType).Select(s => s.SeasonWeek)) + ")",-30}" +
                        //  $"({game.AwayTeam.Name}: {string.Join(" ", game.AwayTeam.Schedule.Where(w => w.GameType == gameType).Select(s => s.SeasonWeek)) }) ");

                        throw new Exception($"Try {tries}. Not able schedule all the games for {gameType}.");
                    }
                    else
                    {
                        //Debug.WriteLine($"{game.GameType,-15} : Successfully setting GameWeek to {gameWeek} for {game,-70}" +
                        //  $"{ "(" + game.HomeTeam.Name + ": " + string.Join(" ", game.HomeTeam.Schedule.Where(w => w.GameType == gameType).Select(s => s.SeasonWeek)) + ")",-30}" +
                        //  $"({game.AwayTeam.Name}: {string.Join(" ", game.AwayTeam.Schedule.Where(w => w.GameType == gameType).Select(s => s.SeasonWeek)) }) ");

                        game.SeasonWeek = gameWeek;
                        game.GameDate = g.BaseDateTime.AddDays((gameWeek - 1) * 7);
                        game.HomeTeam.Schedule.RemoveAll(r => r.SeasonWeek == gameWeek);
                        game.AwayTeam.Schedule.RemoveAll(r => r.SeasonWeek == gameWeek);
                    }
                }
            }
            return games;
        }

        public static List<FootballGame> AddByeWeek(this List<FootballGame> games, IGenerator g)
        {
            var byeWeekGames = games.ToList();

            for (int i = 2; i <= 16; i++)
            {
                var gamesInWeek = byeWeekGames.Where(w => w.SeasonWeek == i).ToList();
                var gameFromWeek = gamesInWeek.TakeRandomOne(g.RowRandom);

                if (gameFromWeek != null)
                {
                    gameFromWeek.SeasonWeek = 17;
                    gameFromWeek.GameDate = g.BaseDateTime.AddDays((17 - 1) * 7);

                    // Remove these teams as you can only have one bye week in a season.
                    byeWeekGames.RemoveAll(r => r.HomeTeam.Id == gameFromWeek.HomeTeam.Id || r.AwayTeam.Id == gameFromWeek.HomeTeam.Id);
                    byeWeekGames.RemoveAll(r => r.HomeTeam.Id == gameFromWeek.AwayTeam.Id || r.AwayTeam.Id == gameFromWeek.AwayTeam.Id);
                }
            }
            return games;
        }

        public static List<ScheduleSlot> GetBaseSlots(this List<ScheduleSlot> list)
        {
            if (list.Count != 0)
                return list;

            list.AddRange(new List<ScheduleSlot>
                    {
                        new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 1 },
                        new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 2 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 3 },
                        new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 4 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 5 },
                        new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 6 },
                        new ScheduleSlot{ GameType = GameType.ExtraConference, SeasonWeek = 7 },
                        new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 8 },
                        new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 9 },
                        new ScheduleSlot{ GameType = GameType.ExtraConference, SeasonWeek = 10 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 11 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 12 },
                        new ScheduleSlot{ GameType = GameType.InConference, SeasonWeek = 13 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 14 },
                        new ScheduleSlot{ GameType = GameType.OutOfConference, SeasonWeek = 15 },
                        new ScheduleSlot{ GameType = GameType.Division, SeasonWeek = 16 },
                    });

            return list;
        }

        public static List<DateTime> GetOpeningSundayList(this List<DateTime> list)
        {
            // Years starting 10 years ago to 10 years from now
            var startDate = DateTime.Now.AddYears(-10);

            // List of Second Sundays in September + or - 10 years
            return Enumerable.Range(0, 20)
                             .Select(s => startDate.AddYears(s).SecondSundayInSeptember())
                             .ToList();
        }

        public static List<FootballGame> NumberedGames(this List<FootballGame> games)
        {
            var orderedGames = games.OrderBy(o => o.GameDate).ToList();

            for (int i = 0; i < orderedGames.Count(); i++)
            {
                orderedGames[i].GameId = i + 1;
            }
            return orderedGames;
        }

        public static DateTime SecondSundayInSeptember(this DateTime date)
        {
            if (date == null)
                throw new Exception("SecondSundayInSeptember - Date cannot be null");

            return new DateTime(date.Year, 9, 1).NextDayOfWeek(DayOfWeek.Sunday, true).AddDays(7);
        }

        // TODO - MOVE TO WILDHARE
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek, bool includeCurrentDate = false)
        {
            if (includeCurrentDate && date.DayOfWeek == dayOfWeek)
                return date;

            return date.AddDays(7 - (int)date.DayOfWeek);
        }

        // NOT SURE THIS IS WORKING...
        public static Random Skip(this Random random, int number )
        {
            if (number < 0) number = 0;

            for (int i = 1; i <= number; i++)
            {
                random.Next();
            }
            return random;
        }
    }
}


// NO LONGER USED - USING EqualizeVenue() INSTEAD
//public static IList<FootballGame> EqualizeHomeAndAway(this IList<FootballGame> games, int repeat = 3, string conference = "In")
//{
//    for (int i = 1; i <= repeat; i++)
//    {
//        EqualizeGames(games, i, conference);
//    }

//    return games.Select(s => s).ToList();
//}

//private static void EqualizeGames(IList<FootballGame> games, int num, string conference)
//{
//    foreach (var game in games)
//    {
//        string homeId = game.HomeTeam.Id;
//        int homeTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);
//        int awayTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);

//        if (homeTeam_HomeGames != 2)
//        {
//            Debug.WriteLine($"{conference} {num} - Switching Home({homeTeam_HomeGames}) " +
//                            $"{game.HomeTeam} to Away({homeTeam_HomeGames}) {game.AwayTeam}");

//            var home = game.HomeTeam;
//            var away = game.AwayTeam;
//            game.HomeTeam = away;
//            game.AwayTeam = home;
//        }
//    }
//}

//      return Enumerable.Range(1, 16)
//      .Select(n => new ScheduleSlot
//      {
//          SeasonWeek = n

//      }).ToList();
