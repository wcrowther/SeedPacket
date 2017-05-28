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
                    new Rule(typeof(List<Invoice>), "", g => Funcs.CacheItemsRandom<Invoice>(g, "Invoices", 3, 3), "GetRandomInvoices")
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
                    new Rule(typeof(List<InvoiceItem>), "",     g => Funcs.CacheItemsNext<InvoiceItem>(g, "InvoiceItems", 3, 3), "GetRandomInvoices"),
                    new Rule(typeof(List<Invoice>), "",         g => Funcs.CacheItemsRandom<Invoice>(g, "Invoices", 2, 2), "GetRandomInvoiceItems")
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

    }
}
