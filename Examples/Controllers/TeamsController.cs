using Examples.Models;
using SeedPacket.Functions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Examples.FootballExtensions;

namespace Website.Controllers
{
    public class TeamsController : Controller
    {
        public ActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();
            var footballGames = GetGames();

            stopwatch.Stop();
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(footballGames);
        }

        private List<FootballGame> GetGames() //randomSeed: 34234, 
        {
            var games = new List<FootballGame>().Seed(seedEnd: 16, customPropertyName: "Game").ToList();
            return games;
        }

    }
}