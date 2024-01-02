using SeedPacket.Examples.Logic.Models;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Examples.Logic.Helpers
{
    public static class FootballExtensions 
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

            for (int i = 0; i < games.Count; i++)
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
                        game.GameDate =  GetDateByWeekAndLocation(g, game.HomeTeam, gameWeek); //g.BaseDateTime.AddDays((gameWeek - 1) * 7); 
                        game.HomeTeam.Schedule.RemoveAll(r => r.SeasonWeek == gameWeek);
                        game.AwayTeam.Schedule.RemoveAll(r => r.SeasonWeek == gameWeek);
                    }
                }
            }
            return games;
        }

        private static DateTime GetDateByWeekAndLocation(IGenerator g, FootballTeam homeTeam, int gameWeek)
        {
            var date = g.BaseDateTime.AddDays((gameWeek - 1) * 7);

            int offsetTime;
            switch (homeTeam.TimeZone)
            {
                case "EDT": offsetTime = 0; break;
                case "CDT": offsetTime = 1; break;
                case "MDT": offsetTime = 2; break;
                case "MST": offsetTime = 2; break;
                case "PDT": offsetTime = 3; break;
                default: throw new InvalidTimeZoneException($"{homeTeam.Location} {homeTeam.Name}");
            }

            return date.AddHours(offsetTime);
        }

        public static List<FootballGame> AddByeWeeks(this List<FootballGame> games, IGenerator g)
        {
            // Currently makes bye weeks from the 2nd week to the 16th week then takes the remainng
            // 6 teams and adds them randomly. This could easily change based on anothe pattern. 
            // Teams are taken out of the list so they can only have one bye week a season

            var byeWeekGames = games.ToList();

            for (int i = 2; i <= 16; i++)
            {
                AddByeWeek(games, g, byeWeekGames, i);
            }

            while (byeWeekGames.Count > 0) // Add bye weeks games from remaining teams
            {
                var seasonWeek = byeWeekGames.First().SeasonWeek;
                AddByeWeek(games, g, byeWeekGames, seasonWeek);
            }
            return games;
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

            for (int i = 0; i < orderedGames.Count; i++)
            {
                orderedGames[i].GameId = i + 1;
            }
            return orderedGames;
        }

        public static DateTime SecondSundayInSeptember(this DateTime date)
        {
            return new DateTime(date.Year, 9, 1, 13, 0, 0).NextDayOfWeek(DayOfWeek.Sunday, true).AddDays(7);
        }

        // =================================================================================

        private static void AddByeWeek(List<FootballGame> games, IGenerator g, List<FootballGame> byeWeekGames, int i)
        {
            var gamesInWeek = byeWeekGames.Where(w => w.SeasonWeek == i).ToList();
            var gameFromWeek = gamesInWeek.TakeRandomOne(g.RowRandom);

            if (gameFromWeek != null)
            {
                // Add new Week 17 ga
                games.Add(new FootballGame
                {
                    SeasonWeek = 17,
                    GameDate = g.BaseDateTime.AddDays((17 - 1) * 7),
                    HomeTeam = gameFromWeek.HomeTeam,
                    AwayTeam = gameFromWeek.AwayTeam,
                    GameType = gameFromWeek.GameType,
                    SeasonStartYear = gameFromWeek.SeasonStartYear
                });

                // Make game from week a Bye for both teams
                gameFromWeek.GameType = GameType.Bye;

                // Remove these teams as you can only have one bye week in a season.
                byeWeekGames.RemoveAll(r => r.HomeTeam.Id == gameFromWeek.HomeTeam.Id || r.AwayTeam.Id == gameFromWeek.HomeTeam.Id);
                byeWeekGames.RemoveAll(r => r.HomeTeam.Id == gameFromWeek.AwayTeam.Id || r.AwayTeam.Id == gameFromWeek.AwayTeam.Id);
            }
        }

    }
}
