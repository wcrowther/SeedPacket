//using Examples.FootballExtensions;
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
            var info = _teamsManager.GetGamesInfo(DateTime.Now, new Random(123));

            return View(info);
        }

        [Route("Teams/GetRefreshedGames/{seed:int}/{firstSunday:DateTime?}")]
        public ActionResult GetRefreshedGames(int seed, DateTime? firstSunday = null)
        {
            DateTime _firstSunday = firstSunday ?? DateTime.Now;

            var info = _teamsManager.GetGamesInfo(_firstSunday, new Random(seed));

            return PartialView("_Games", info);
        }
    }
}
