using Website.Models;
using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class DocumentationController : Controller
    {
        public ActionResult Index()
        {
            var users = new List<User>().Seed(15);

            return View(users);
        }
    }
}