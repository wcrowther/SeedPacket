using Examples.Models;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class AjaxController : Controller
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

            return PartialView("_AjaxExample1A", users);
        }
    }
}