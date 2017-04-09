using MvcExamples.Models;
using SeedPacket;
using NewLibrary.ForString;
using System;
using System.Linq;
using System.Web.Mvc;
using static System.Configuration.ConfigurationManager;
using System.Collections.Generic;

namespace MvcExamples.Controllers
{
    public class SimpleSeedController : Controller
    {
        public ActionResult Index()
        {
            // data generated in the view to show that capability
            return View();
        }

        public ActionResult EightExamples()
        {
            return GeneratedEightExamples();
        }

        private ActionResult GeneratedEightExamples()
        {
            var lists = new List<List<Item>>();

            var seed0 = new SimpleSeed();
            var list0 = Enumerable.Range(1, 5)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed0.Next("FirstName", n)} {seed0.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list0);

            var seed1 = new SimpleSeed("~/SourceFiles/jsonseedsource2.json", datainputtype: DataInputType.JsonFile);
            var list1 = Enumerable.Range(1, 8)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed1.Randomize("FirstName")} {seed1.Randomize("LastName")}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list1);

            var seed2 = new SimpleSeed("~/SourceFiles/seedsource.xml", datainputtype: DataInputType.XmlFile);
            var list2 = Enumerable.Range(1, 3)
                .Select(n => new Item
                {
                    ItemId = seed2.Next("Id", n).toInt(),
                    ItemName = $"{seed2.Next("FirstName", n)} {seed2.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list2);

            var seed3 = new SimpleSeed(sourcestring: GetDemoJson(), datainputtype: DataInputType.JsonString);
            var list3 = Enumerable.Range(100, 3)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed3.Next("FirstName", n)} {seed3.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list3);

            var seed4 = new SimpleSeed(sourcestring: GetDemoXml(), datainputtype: DataInputType.XmlString);
            var list4 = Enumerable.Range(20000, 4)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed4.Next("FirstName", n)} {seed4.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list4);

            var seed5 = new SimpleSeed("~/SourceFiles/jsonseedsource.json");
            var list5 = Enumerable.Range(2, 4)
                .Select(n => new Item
                {
                    ItemId = seed5.Next("Id", n).toInt(),
                    ItemName = $"{seed5.Next("FirstName", n)} {seed5.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list5);

            var seed6 = new SimpleSeed(sourcestring: GetDemoJson());
            var list6 = Enumerable.Range(10, 5)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed6.Next("FirstName", n)} {seed6.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list6);

            var seed7 = new SimpleSeed("~/SourceFiles/seedsource.xml");
            var list7 = Enumerable.Range(1, 5)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed7.Next("FirstName", n)} {seed7.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list7);

            var seed8 = new SimpleSeed(null, GetDemoXml());
            var list8 = Enumerable.Range(1, 3)
                .Select(n => new Item
                {
                    ItemId = n,
                    ItemName = $"{seed8.Next("FirstName", n)} {seed8.Next("LastName", n)}",
                    Created = DateTime.Now.AddHours(n)
                }).ToList();

            lists.Add(list8);

            return View(lists);
        }

        private string GetDemoJson()
        {
            return @"{""FirstName"": [ ""John"",""Patricia"",""Michael"",""Susan"",""William"",""Mary"",""Robert"",""Tremayne"",""Zeb"" ],
                      ""LastName"": [ ""Smith"",""Johnson"",""Williams"",""Jones"",""Brown"",""Davis"",""Miller"",""Wilson"" ]}";
        }

        private string GetDemoXml()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                            <Root>
                            <FirstNames>
                                <FirstName>Bob</FirstName>
                                <FirstName>Betty</FirstName>
                                <FirstName>Brenda</FirstName>
                                <FirstName>Bjorn</FirstName>
                            </FirstNames>
                            <LastNames>
                                <LastName>Starr</LastName>
                                <LastName>Simpson</LastName>
                                <LastName>Stark</LastName>
                                <LastName>Sky</LastName>
                            </LastNames>
                            </Root>";
            return xml;
        }
    }
}