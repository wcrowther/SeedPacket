using Examples.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class GeneratorController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<AppUser>().Seed(20);

            return View(users);
        }

        public ActionResult GetResultRows(int? rows, int seed)
        {
            var generator = new MultiGenerator() { BaseRandom = new Random(seed) };
            var users = new List<AppUser>().Seed(1, rows ?? 20, generator);

            return PartialView("_DataSourceExample1A", users);
        }
    }
}