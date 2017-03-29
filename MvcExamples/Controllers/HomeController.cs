using MvcExamples.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcExamples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<User>().Seed(20);

            return View(users);
        }

        public ActionResult GetResultRows(int? rows, int seed)
        {
            var generator = new MultiGenerator() { BaseRandom = new Random(seed)};
            var users = new List<User>().Seed(1, rows ?? 20, generator);

            return PartialView("_Result_SimpleExample", users);
        }

        public ActionResult Simple()
        {
            return View();
        }

        public ActionResult Dictionary()
        {
            return View();
        }
    }
}