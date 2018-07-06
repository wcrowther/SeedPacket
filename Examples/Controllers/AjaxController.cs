using Examples.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class AjaxController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<AppUser>().Seed(20, new Random(1234));

            return View(users);
        }

        public ActionResult GetResultRows(int? rows, int seed)
        {
            var generator = new MultiGenerator() { BaseRandom = new Random(seed) };
            var users = new List<AppUser>().Seed(1, rows ?? 20, generator);

            return PartialView("_AjaxExample1", users);
        }
    }
}