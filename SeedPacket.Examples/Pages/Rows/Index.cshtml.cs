using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

namespace SeedPacket.Examples.Pages;

public class RowsModel(IWebHostEnvironment webHostEnvironment) : PageModel
{
    private const string xmlFilePath = @"\Logic\SourceFiles\xmlSeedSourcePlus.xml";

	public List<Account> Accounts { get; set; } = new List<Account>();

    public long ElapsedTime { get; set; }
        
    public void OnGet()
    {
        var stopwatch = Stopwatch.StartNew();

        Accounts = GetAccounts(1000); // Create 1000 accounts with Invoices with InvoiceItems

        stopwatch.Stop();
        ElapsedTime = stopwatch.ElapsedMilliseconds;
    }

    // =======================================================================================
    // RANDOM =>> 1000 records * 5 invoices * 15 invoiceItems in ~630 ms
    // NEXT   =>> 1000 random records * 5 'next' invoices * 15 'next' invoiceItems in ~350 ms
    // =======================================================================================

    private List<Account> GetAccounts(int rowCount)
    {
		string contentRoot				= webHostEnvironment.ContentRootPath;
		string xmlSeedSourcePlusPath	= $@"{contentRoot}{xmlFilePath}";
		var baseRandom					= new Random(2342789);
        int baseAccountId				= 3464, baseInvoiceId = 12799, baseItemId = 1098887;

		var basicRules = new List<Rule>()
		{
			new (typeof(int),				"AccountId",	g => baseAccountId + g.RowNumber, "AccountId"),
			new (typeof(DateTime),			"Create%",		g => g.BaseDateTime.AddDays(g.RowRandom.Next(-30, 1)), "DateTimeInLastMonth"),
			new (typeof(string),			"Description%",	g => Funcs.GetElementNext(g, "Description"), "Description", "Gets Description from custom XML file"),
			new (typeof(List<InvoiceItem>), "",				g => Funcs.GetListFromCacheNext<InvoiceItem>(g, "InvoiceItems", 1, 8), "getInvoiceItems"),
			new (typeof(List<Invoice>),		"",				g => Funcs.GetListFromCacheNext<Invoice>(g, "Invoices", 0, 5), "getInvoices")  // GetAccountInvoices(g)
		};

		var gen = new MultiGenerator(xmlSeedSourcePlusPath, baseRandom: baseRandom);
		gen.Rules.AddRange(basicRules, true);

		gen.Cache.InvoiceItems = new List<InvoiceItem>().Seed(baseItemId, baseItemId + (rowCount * 20), gen);
        gen.Cache.Invoices     = new List<Invoice>().Seed(baseInvoiceId, baseInvoiceId + (rowCount * 4), gen);

        return new List<Account>().Seed(1, rowCount, gen).ToList();
    }

    // See note below
    private static List<Invoice> GetAccountInvoices(IGenerator g)
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

// ---------------------------------------------------------------------------------------------------------
// The Private GetAccountInvoices() method above is an example where a rule is broken out into a separate function
// ---------------------------------------------------------------------------------------------------------
// 1) Adding extra logic to make the accountId in the invoice match the actual accountId in parent
// 2) Also useful if debugging is needed as the code is easier to isolate when in a separate function
// 3) Without needing the accountId match it can be simplified to:
//    Funcs.GetListFromCacheNext<Invoice>(g, "Invoices", 0, 5)
