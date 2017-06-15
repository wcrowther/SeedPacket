using NUnit.Framework;
using SeedPacket;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.CSharp;
using Website.Models;

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
                Rules =  {
                    new Rule(typeof(List<Invoice>), "", g => Funcs.GetCacheItemsRandom<Invoice>(g, "Invoices", 3, 3), "GetRandomInvoices")
                }
            };
            generator.Cache.Invoices = new List<Invoice>().Seed(1, 300, generator);

            var accountList =  new List<Account>().Seed(1,  50, generator).ToList(); ;

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
                    new Rule(typeof(List<InvoiceItem>), "",     g => Funcs.GetCacheItemsNext<InvoiceItem>(g, "InvoiceItems", 3, 3), "GetRandomInvoices"),
                    new Rule(typeof(List<Invoice>), "",         g => Funcs.GetCacheItemsRandom<Invoice>(g, "Invoices", 2, 2), "GetRandomInvoiceItems")
                }
            };

            generator.Cache.InvoiceItems = new List<InvoiceItem>().Seed(1, (rowCount * 10), generator); //
            generator.Cache.Invoices = new List<Invoice>().Seed(1, (rowCount * 3), generator); // 

            var accountList =  new List<Account>().Seed(1, rowCount, generator).ToList();

            Assert.AreEqual(10, accountList.Count());                              // 10 Accounts
            Assert.AreEqual(20, accountList.SelectMany(a => a.Invoices).Count() );  // 10 Accounts x 2 Invoices each = 20 Invoices taken
            Assert.AreEqual(10, generator.Cache.Invoices.Count);                 // 30 Invoice created - 20 taken = 10 left in cache
            Assert.AreEqual(60, accountList.SelectMany(a => a.Invoices.SelectMany(i => i.InvoiceItems)).Count());  // 10 x 2 x 3 = 60 InvoiceItems
            Assert.AreEqual(10, generator.Cache.InvoiceItems.Count);                 // 100 InvoiceItems created - 90 taken in invoices = 10 left in cache
        }

        [Test]
        public void TestGetByItemName()
        {
            var generator = new BasicGenerator();
            generator.Cache.Invoice = new Invoice {
                InvoiceId = 1,
                AccountId = 7890,
                InvoiceItems = new List<InvoiceItem> {
                    new InvoiceItem{ InvoiceItemId = 1234 , Amount = 99.99M },
                    new InvoiceItem{ InvoiceItemId = 5678 , Amount = 2.22M }
                }
            };
            ExpandoObject cache = generator.Cache;

            Assert.AreEqual(1, cache.GetByItemName<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890, cache.GetByItemName<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, cache.GetByItemName<Invoice>("Invoice").InvoiceItems[0].Amount);
            Assert.AreEqual(5678, cache.GetByItemName<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);
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
            cache.RemoveItemByName(name);

            Assert.AreEqual(null, cache.GetByItemName<Invoice>(name));
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
            cache.AddItemByName(name, invoice);

            Assert.AreEqual(1, cache.GetByItemName<Invoice>(name).InvoiceId);
        }
    }
}
