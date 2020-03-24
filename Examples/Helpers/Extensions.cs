
using Examples.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Examples.Helpers
{
    public static class Extensions 
    {
        public static IList<FootballGame> EqualizeHomeAndAway(this IList<FootballGame> games)
        {
            EqualizeGames(games, 1);
            EqualizeGames(games, 2);
            EqualizeGames(games, 3);

            return games.Select(s => s).ToList();
        }

        private static void EqualizeGames(IList<FootballGame> games, int num)
        {
            foreach (var game in games)
            {
                string homeId = game.HomeTeam.Id;
                int homeTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);
                int awayTeam_HomeGames = games.Count(w => w.HomeTeam.Id == homeId);

                if (homeTeam_HomeGames != 2)
                {
                    Debug.WriteLine($"{num} - Switching Home({homeTeam_HomeGames}) {game.HomeTeam} to Away({homeTeam_HomeGames}) {game.AwayTeam}");

                    var home = game.HomeTeam;
                    var away = game.AwayTeam;
                    game.HomeTeam = away;
                    game.AwayTeam = home;
                }
            }
        }
    }
}
