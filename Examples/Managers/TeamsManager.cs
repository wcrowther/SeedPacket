using Examples.Generators;
using Examples.Interfaces;
using Examples.Models;
using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Hosting;

namespace Examples.Managers
{
    public class TeamsManager : ITeamsManager
    {
        public FootballInfo GetGamesInfo(Random random = null, DateTime? openingDate = null)
        {
            var footballInfo = new FootballInfo(openingDate);
            string footballSource = HostingEnvironment.MapPath("/SourceFiles/FootballSource.xml");
            
            var stopwatch = Stopwatch.StartNew();

            var gen = new FootballGenerator (footballInfo.OpeningSunday, footballSource, random);
            // var games = new List<FootballGame>().Seed(gen); 

            stopwatch.Stop();

            footballInfo.Teams = gen.Teams; //gen.Cache.FootballTeams; 
            footballInfo.Games = gen.Games;  //games.ToList();        

            footballInfo.ElapsedTime    = stopwatch.Elapsed.TotalSeconds.ToString("#.0000");
            Debug.WriteLine($"ElapsedTime: { footballInfo.ElapsedTime }");

            return footballInfo;
        }
    }
}


// ========================================================================
// NO LONGER USED BUT AN INTERESTING SAMPLE
// ========================================================================
//
//private Dictionary<string, List<FootballGame>> GroupGamesByTeam(FootballGenerator gen, IEnumerable<FootballGame> games)
//{
//    if (games.Count() == 0)
//        throw new Exception("Games not generated correctly.");
//
//    var teamGames = new Dictionary<string, List<FootballGame>>();
//
//    foreach (FootballTeam team in gen.Cache.FootballTeams)
//    {
//        teamGames.Add($"{team.Location} {team.Name}", games
//                      .Where(w => w.HomeTeam.Id == team.Id || w.AwayTeam.Id == team.Id)
//                      .OrderBy(o => o.SeasonWeek)
//                      .ToList());
//    }
//    return teamGames;
//}
//
//private Dictionary<string, List<FootballGame>> GroupGamesByWeek(IEnumerable<FootballGame> games)
//{
//    if (games.Count() == 0)
//        throw new Exception("Games not generated correctly.");
//
//    return games.OrderBy(o => o.SeasonWeek)
//                .GroupBy(g => g.SeasonWeek)
//                .ToDictionary(g => g.Key.ToString(), g => g.ToList());
//}
