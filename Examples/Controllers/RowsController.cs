using Examples.Models;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
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
            ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

            return View(accounts);
        }

        // =======================================================================================
        // RANDOM =>> 1000 records * 5 invoices * 15 invoiceItems in ~630 ms
        // NEXT   =>> 1000 random records * 5 'next' invoices * 15 'next' invoiceItems in ~350 ms
        // =======================================================================================
        private List<Account> GetAccounts(int rowCount)
        {
            int baseAccountId = 3464;
            var generator = new MultiGenerator("~/SourceFiles/xmlSeedSourcePlus.xml")
            {
                Rules =  {
                    new Rule(typeof(int), "AccountId",      g => baseAccountId + g.RowNumber, "AccountId"  ),
                    new Rule(typeof(DateTime), "Create%",   g => g.BaseDateTime.AddDays (g.RowRandom.Next(-30, 1)), "DateTimeInLastMonth"  ),
                    new Rule(typeof(string),"Description%", g => Funcs.GetElementRandom(g, "Description"), "Description", "Gets Description from custom XML file" ),
                    new Rule(typeof(List<InvoiceItem>), "", g => Funcs.GetCacheItemsNext<InvoiceItem>(g, "InvoiceItems", 1, 8), "getInvoiceItems"),
                    new Rule(typeof(List<Invoice>), "",     g => Funcs.GetCacheItemsRandom<Invoice>(g, "Invoices", 1, 5), "getInvoices") 
                }
            };
            generator.Cache.InvoiceItems = new List<InvoiceItem>().Seed(10000, 10000 + (rowCount * 20), generator); 
            generator.Cache.Invoices = new List<Invoice>().Seed(2000, 2000 + (rowCount * 4), generator); 

            return new List<Account>().Seed(1, rowCount, generator).ToList(); 
        }

        // Example where rule is broken out into a separate function (use: "g => GetInvoices(g)" to replace "Invoices" above. ):
        // 1) For adding extra logic like making the accountId in the invoice the actual accountId in parent, if that is required
        // 2) Useful if debugging is needed. Code is much easier to isolate when in a separate function...

        private static List<Invoice> GetInvoices(IGenerator g)
        {
            int accountId = Convert.ToInt32(g?.CurrentRowValues["AccountId"]);
            var invoices = Funcs.GetCacheItemsNext<Invoice>(g, "Invoices", 1, 8);
            if (invoices != null)
                invoices.ForEach(ii => ii.AccountId = accountId);

            return invoices;
        }
    }
}