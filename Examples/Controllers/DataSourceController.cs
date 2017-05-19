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
    public class DataSourceController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<User>().Seed(20);

            return View(users);
        }

        public ActionResult GetResultRows(int? rows, int seed)
        {
            var generator = new MultiGenerator() { BaseRandom = new Random(seed) };
            var users = new List<User>().Seed(1, rows ?? 20, generator);

            return PartialView("_DataSourceExample1A", users);
        }
    }
}