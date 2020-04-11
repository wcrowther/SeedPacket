//using Examples.FootballExtensions;
using Examples.Helpers;
using Examples.Interfaces;
using Examples.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Helpers;
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
            var sundayList = new List<DateTime>().GetOpeningSundayList();

            return View(sundayList);
        }

        [HttpPost]
        [Route("Teams/GetGamesList")]
        public ActionResult GetGamesList(int seed, int year, DateTime? customSunday = null)
        {
            var openingSunday = new DateTime(year, 1, 1).SecondSundayInSeptember(); // Usual start of season

            var info = _teamsManager.GetGamesInfo(new Random(seed), customSunday ?? openingSunday);

            return Json(info);
        }
    }
}
