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
    public class ExamplesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}