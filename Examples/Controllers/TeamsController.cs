using Examples.Models;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
//using Examples.FootballExtensions;
using SeedPacket.Extensions;
using System.Reflection;
using Examples.Generators;
using System.Web.Hosting;
using Examples.Extensions;

namespace Website.Controllers
{
    public class TeamsController : Controller
    {
        public ActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();

            var info = GetGamesInfo();

            stopwatch.Stop();

            ViewBag.ElapsedTime = stopwatch.Elapsed.TotalSeconds.ToString("#.0000");

            return View(info);
        }
        private FootballInfo GetGamesInfo() //randomSeed: 34234, 
        {
            string footballSource = HostingEnvironment.MapPath("/SourceFiles/FootballSource.xml"); 
            var gen = new FootballGenerator(DateTime.Now.Next(DayOfWeek.Sunday), footballSource);

            var games = new List<FootballGame>().Seed(gen);

            if (games.Count() == 0)
                throw new Exception("Games not generated correctly.");

            // Group by SeasonWeek (if exists)
            var weeks = games.OrderBy(o => o?.HomeTeam.ConfId ?? 0)
                             .ThenBy(t => t?.HomeTeam.DivId ?? 0)
                             .GroupBy(g => g.SeasonWeek)
                             .ToDictionary(g => g.Key, g => g.ToList());

            var teamGames = new Dictionary<string, List<FootballGame>>();

            foreach (FootballTeam team in gen.Cache.FootballTeams)
            {
                teamGames.Add($"{team.Location} {team.Name}", 
                              games.Where(w => w.HomeTeam.Id == team.Id || w.AwayTeam.Id == team.Id).ToList() );
            }

            return new FootballInfo()
            {
                TeamGames = teamGames,
                FootballWeeks = weeks
            };
        }

    }
}
