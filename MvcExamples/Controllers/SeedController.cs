using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcExamples.Controllers
{
    public class SeedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Extending()
        {
            return View("Extending");
        }

        public ActionResult SeedTests()
        {
            return View();
        }

        public ActionResult SeedExamples()
        {
            return View();
        }
    }
}