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
    public class ExtendingController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}