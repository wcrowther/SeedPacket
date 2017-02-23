using MvcExamples.Models;
using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcExamples.Controllers
{
    public class RulesEngineController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<User>().Seed(15);

            return View(users);
        }
    }
}