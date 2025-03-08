using Microsoft.AspNetCore.Mvc;
using SeedPacket.Examples.Logic.Helpers;
using SeedPacket.Examples.Logic.Interfaces;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;

namespace SeedPacket.Examples.Controllers
{
    public class DataController(ITeamsManager teamManager, IExampleManager exampleManager) : Controller
    {
		public JsonResult GetFootballInfo(int seed, int year, DateTime? customSunday = null)
        {
            var openingSunday = new DateTime(year, 1, 1).SecondSundayInSeptember(); // Usual start of season

            var info = teamManager.GetGamesInfo(new Random(seed), customSunday ?? openingSunday);

            return new JsonResult(info);
        }

        public IActionResult GetResultRows(int? rows = 20, int? seed = 1234)
        {
            var genRows = rows > 9999 ? 20 : rows;
            var generator = new MultiGenerator() { BaseRandom = new Random(seed.Value) };
            var users = new List<User>().Seed(1, genRows, generator);

            return PartialView("_Result_Simple", users);
        }

		public IActionResult GetAdvancedResultRows(int? rows = 10, int? seed = 1234)
		{
			var exampleList = exampleManager.GetExampleList(rows, seed);

			return PartialView("_Result_Advanced", exampleList);
		}
	}
}
