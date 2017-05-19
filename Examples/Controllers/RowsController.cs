using Examples.Models;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class RowsController : Controller
    {

        public ActionResult Index()
        {
            var stopwatch = Stopwatch.StartNew();
            var accounts = GetAccounts(1000); // Create 1000 accounts with Invoices with InvoiceItems

            stopwatch.Stop();
            ViewBag.ElapsedTime = "Elapsed Milliseconds: " + stopwatch.ElapsedMilliseconds;

            return View(accounts);
        }

        // =======================================================================================
        // RANDOM =>> 1000 records * 5 invoices * 15 invoiceItems in ~630 ms
        // NEXT   =>> 1000 random records * 5 'next' invoices * 15 'next' invoiceItems in ~350 ms
        // =======================================================================================
        private List<Account> GetAccounts(int rowCount)
        {
            var generator = new MultiGenerator("~/SourceFiles/xmlSeedSourcePlus.xml")
            {
                Rules =  {
                new Rule(typeof(DateTime), "Create%",   g => g.BaseDateTime.AddDays (g.RowRandom.Next(-30, 1)), "DateTimeInLastMonth"  ),
                new Rule(typeof(List<InvoiceItem>), "", g => Funcs.CacheItemsNext<InvoiceItem>(g, "InvoiceItems", 1, 8), "GetNextInvoiceItems"  ),
                new Rule(typeof(List<Invoice>), "",     g => Funcs.CacheItemsNext<Invoice>(g, g.Cache.Invoices, 1, 5), "GetRandomInvoices", ""),
                new Rule(typeof(string),"Description%", g => Funcs.ElementRandom(g, "Description"), "Description", "Gets Description from custom XML file" )
            }
            };
            generator.Cache.InvoiceItems = new List<InvoiceItem>().Seed(12345, 12345 + (rowCount * 15), generator);
            generator.Cache.Invoices = new List<Invoice>().Seed(10234, 10234 + (rowCount * 5), generator);

            return new List<Account>().Seed(1, rowCount, generator).ToList(); ;
        }
    }
}