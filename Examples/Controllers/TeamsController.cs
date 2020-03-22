using Examples.Models;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Examples.FootballExtensions;
using System.Reflection;

namespace Website.Controllers
{
    public class TeamsController : Controller
    {
        public ActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();

            var footballGames = GetGamesGroupedByWeek();

            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(footballGames);
        }

        private Dictionary<int,List<FootballGame>> GetGamesGroupedByWeek() //randomSeed: 34234, 
        {
            // FootballExtensions.SeedSeason generates all the games for a football season.
            var games = new List<FootballGame>().SeedSeason(DateTime.Now).ToList();

            if (games.Count() == 0)
                throw new Exception("Games not generated correctly.");

            // Group by SeasonWeek (if exists)
            var weeks = games.OrderBy(o => o.HomeTeam.ConfId)
                             .ThenBy(t => t.HomeTeam.DivId)
                             .GroupBy(g => g.SeasonWeek)
                             .ToDictionary(g => g.Key, g => g.ToList());

            return weeks;
        }

    }
}
