using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SeedPacket.Examples.Logic.Extensions;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SeedPacket.Examples.Pages
{ 
    public class RowsModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string xmlSeedSourcePlusPath;
        private const string xmlFilePath = @"\Logic\SourceFiles\xmlSeedSourcePlus.xml";

        public RowsModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

            SetXmlSeedSourcePath();
        }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public long ElapsedTime { get; set; }
            
        public void OnGet()
        {
            var stopwatch = Stopwatch.StartNew();

            Accounts = GetAccounts(1000); // Create 1000 accounts with Invoices with InvoiceItems

            stopwatch.Stop();
            ElapsedTime = stopwatch.ElapsedMilliseconds;
        }

        private void SetXmlSeedSourcePath()
        { 
            string contentRoot = _webHostEnvironment.ContentRootPath;
            xmlSeedSourcePlusPath = $@"{contentRoot}{xmlFilePath}"; 
        }

        // =======================================================================================
        // RANDOM =>> 1000 records * 5 invoices * 15 invoiceItems in ~630 ms
        // NEXT   =>> 1000 random records * 5 'next' invoices * 15 'next' invoiceItems in ~350 ms
        // =======================================================================================

        private List<Account> GetAccounts(int rowCount)
        {
            var baseRandom = new Random(2342789);
            int baseAccountId = 3464;
            int baseInvoiceId = 12799;
            int baseItemId = 1098887;

            var generator = new MultiGenerator(xmlSeedSourcePlusPath, baseRandom: baseRandom)
            {
                Rules =
                {
                    new Rule(typeof(int), "AccountId",      g => baseAccountId + g.RowNumber, "AccountId"  ),
                    new Rule(typeof(DateTime), "Create%",   g => g.BaseDateTime.AddDays (g.RowRandom.Next(-30, 1)), "DateTimeInLastMonth"  ),
                    new Rule(typeof(string),"Description%", g => Funcs.GetElementRandom(g, "Description"), "Description", "Gets Description from custom XML file" ),
                    new Rule(typeof(List<InvoiceItem>), "", g => Funcs.GetListFromCacheNext<InvoiceItem>(g, "InvoiceItems", 1, 8), "getInvoiceItems"),
                    new Rule(typeof(List<Invoice>), "",     g => GetInvoices(g), "getInvoices")
                }
            };
            generator.Cache.InvoiceItems = new List<InvoiceItem>().Seed(baseItemId, baseItemId + (rowCount * 20), generator);
            generator.Cache.Invoices     = new List<Invoice>().Seed(baseInvoiceId, baseInvoiceId + (rowCount * 4), generator);

            return new List<Account>().Seed(1, rowCount, generator).ToList();
        }

        private static List<Invoice> GetInvoices(IGenerator g)
        {
            int accountId = Convert.ToInt32(g?.CurrentRowValues["AccountId"]);
            var invoices  = Funcs.GetListFromCacheNext<Invoice>(g, "Invoices", 0, 5);

            if (invoices != null)
            {
                invoices.ForEach(a => a.AccountId = accountId);
            }

            return invoices;
        }
    }
}


// The Private GetInvoices() method above is an example where rule is broken out into a separate function
// 1) For adding extra logic like making the accountId in the invoice the actual accountId in parent
// 2) Useful if debugging is needed as the code is much easier to isolate when in a separate function.