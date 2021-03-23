using NUnit.Framework;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Examples.Models;
using WildHare.Extensions;
using System;
using SeedPacket.Interfaces;

namespace Tests.SeedPacket
{
    [TestFixture]
    public class CacheExtensionsTests
    {
        [Test]
        public void CacheExtensions_Basic()
        {
            var generator = new MultiGenerator()
            {
                Rules =
                {
                    new Rule(typeof(List<Invoice>), "", g => Funcs.GetListFromCacheRandom<Invoice>(g, "Invoices", 3, 3), "GetRandomInvoices")
                }
            };
            generator.Cache.Invoices = new List<Invoice>().Seed(1, 300, generator);

            var accountList =  new List<Account>().Seed(1,  50, generator).ToList();

            Assert.AreEqual(150, accountList.SelectMany(a => a.Invoices).Count() ); 
            Assert.AreEqual(150, generator.Cache.Invoices.Count); 
        }

        [Test]
        public void CacheExtensions_Advanced()
        {
            var rowCount = 10;
            var generator = new MultiGenerator()
            {
                Rules =  {
                    new Rule(typeof(List<InvoiceItem>), "",     g => Funcs.GetListFromCacheNext<InvoiceItem>(g, "InvoiceItems", 3, 3), "GetRandomInvoiceItems"),
                    new Rule(typeof(List<Invoice>), "",         g => GetInvoices(g, 2, 2), "GetRandomInvoices")
                }
            };

            generator.Cache.InvoiceItems = new List<InvoiceItem>().Seed(1, (rowCount * 10), generator); //
            generator.Cache.Invoices     = new List<Invoice>().Seed(1, (rowCount * 3), generator); // 

            var accountList =  new List<Account>().Seed(1, rowCount, generator).ToList();

            Assert.AreEqual(10, accountList.Count());                              // 10 Accounts
            Assert.AreEqual(20, accountList.SelectMany(a => a.Invoices).Count() );  // 10 Accounts x 2 Invoices each = 20 Invoices taken
            Assert.AreEqual(10, generator.Cache.Invoices.Count);                 // 30 Invoice created - 20 taken = 10 left in cache
            Assert.AreEqual(60, accountList.SelectMany(a => a.Invoices.SelectMany(i => i.InvoiceItems)).Count());  // 10 x 2 x 3 = 60 InvoiceItems
            Assert.AreEqual(10, generator.Cache.InvoiceItems.Count);                 // 100 InvoiceItems created - 90 taken in invoices = 10 left in cache
        }


        // For greater realism Invoice AccountIds are made to match the account they are in
        // NOTE that AccountId must already exist for this to work
        private static List<Invoice> GetInvoices(IGenerator g, int min, int max)
        {
            int accountId = Convert.ToInt32(g?.CurrentRowValues["AccountId"]);
            var invoices = Funcs.GetListFromCacheNext<Invoice>(g, "Invoices", min, max);
            if (invoices != null)
                invoices.ForEach(ii => ii.AccountId = accountId);

            return invoices;
        }

        [Test]
        public void TestGetByItemName()
        {
            var generator = new BasicGenerator();
            generator.Cache.Invoice = new Invoice {
                InvoiceId = 1,
                AccountId = 7890,
                InvoiceItems = new List<InvoiceItem> {
                    new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M },
                    new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M }
                }
            };

            Assert.AreEqual(1, generator.Cache.Invoice.InvoiceId);

            ExpandoObject cache = generator.Cache;

            Assert.AreEqual(1, cache.Get<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890, cache.Get<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, cache.Get<Invoice>("Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678, cache.Get<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);
        }

        [Test]
        public void TestRemoveByItemName()
        {
            var generator = new BasicGenerator();
            var name = "Invoice";
            generator.Cache.Invoice = new Invoice
            {
                InvoiceId = 1
            };
            ExpandoObject cache = generator.Cache;

            Assert.IsNotNull(cache.Get<Invoice>(name));

            cache.Remove(name);

            Assert.IsNull(cache.Get<Invoice>(name));
        }

        [Test]
        public void TestAddByItemName()
        {
            var generator = new BasicGenerator();
            string name = "Invoice";
            var invoice = new Invoice
            {
                InvoiceId = 1
            };

            ExpandoObject cache = generator.Cache;
            cache.Add(name, invoice);

            Assert.AreEqual(1, cache.Get<Invoice>(name).InvoiceId);
        }
    }
}
