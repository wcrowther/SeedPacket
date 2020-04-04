//using Examples.FootballExtensions;
using Examples.Helpers;
using Examples.Interfaces;
using Examples.Managers;
using System;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class TeamsController : Controller
    {

        private readonly ITeamsManager _teamsManager;

        public TeamsController()
        {
            _teamsManager = new TeamsManager();
        }

        public ActionResult Index()
        {
            var info = _teamsManager.GetGamesInfo(new Random(123) );

            return View(info);
        }

        [HttpPost]
        [Route("Teams/GetRefreshedGames")]
        public ActionResult GetRefreshedGames(int seed, DateTime openingSunday)
        {
            //var _openingDate = DateTime.Parse(openingDate);
            var info = _teamsManager.GetGamesInfo(new Random(seed), openingSunday);

            return PartialView("_Games", info);
        }
    }
}
