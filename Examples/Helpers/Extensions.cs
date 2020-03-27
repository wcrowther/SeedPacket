
using Examples.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Examples.Helpers
{
    public static class Extensions 
    {
        // NO LONGER USED - USING EqualizeVenue() INSTEAD
        public static IList<FootballGame> EqualizeHomeAndAway(this IList<FootballGame> games, int repeat = 3, string conference = "In")
        {
            for (int i = 1; i <= repeat; i++)
            {
                EqualizeGames(games, i, conference);
            }

            return games.Select(s => s).ToList();
        }

        private static void EqualizeGames(IList<FootballGame> games, int num, string conference)
        {
            foreach (var game in games)
            {
                string homeId = game.HomeTeam.Id;
                int homeTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);
                int awayTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);

                if (homeTeam_HomeGames != 2)
                {
                    Debug.WriteLine($"{conference} {num} - Switching Home({homeTeam_HomeGames}) " +
                                    $"{game.HomeTeam} to Away({homeTeam_HomeGames}) {game.AwayTeam}");

                    var home = game.HomeTeam;
                    var away = game.AwayTeam;
                    game.HomeTeam = away;
                    game.AwayTeam = home;
                }
            }
        }
    }
}
