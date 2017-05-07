﻿using Website.Models;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class ExamplesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        protected override void HandleUnknownAction (string actionName)
        {
            this.View(actionName).ExecuteResult(this.ControllerContext);
        }
    }
}