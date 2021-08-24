using Microsoft.AspNetCore.Mvc;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Controllers
{
    public class DataController : Controller
    {
        public IActionResult Index()
        {
            return View("_Result_SimpleExample");
        }

        public IActionResult GetResultRows(int? rows = 20, int? seed = 1234)
        {
            var genRows = rows > 9999 ? 20 : rows;
            var generator = new MultiGenerator() { BaseRandom = new Random(seed.Value) };
            var users = new List<User>().Seed(1, genRows, generator);

            return PartialView("_Result_SimpleExample", users);
        }
    }
}
