using NUnit.Framework;
using SeedPacket;
using SeedPacket.DataSources;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tests.SeedPacket.Core.Model;
using static Tests.SeedPacket.Core.Common;

namespace Tests.SeedPacket
{
    [TestFixture]
    public class SeedCoreTests
    {
        private string pathToTestXmlFile;
        private string pathToTestJsonFile;
        private string xmlFile = @"SimpleSeedSource.xml";
        private string jsonFile = @"JsonSeedSource.json";

        private string testValidXml;
        private string testEmptyXml;
        private string testValidJson;
        private string testEmptyJson;

        [SetUp]
        public void Setup()
        {

			pathToTestXmlFile = Path.Combine(GetApplicationRoot() + "\\Source\\", xmlFile);
            pathToTestJsonFile = Path.Combine(GetApplicationRoot() + "\\Source\\", jsonFile);

            testValidXml = GetValidXml();
            testEmptyXml = GetEmptyXml();
            testValidJson = GetValidJson();
            testEmptyJson = GetEmptyJson();
        }
        
        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_Gets_10_Items_By_Default()
        {
            var iEnumerable = new List<Item>();
            var basicGenerator = new BasicGenerator();
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("ItemName10", list[9].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_Can_Have_A_Negative_SeedBegin()
        {
            var iEnumerable = new List<Item>();
            var basicGenerator = new BasicGenerator() { SeedBegin = -10};
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            // 21 elements because 10 negative, 10 positive, 1 zero
            Assert.AreEqual(21, list.Count());
            Assert.AreEqual(10, list[20].ItemId);
            Assert.AreEqual("ItemName10", list[20].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_For_SimpleValue_Strings()
        {
            var iEnumerable = new List<string>();
            var basicGenerator = new BasicGenerator() { };
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("10", list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<string>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_For_SimpleValue_Ints()
        {
            var iEnumerable = new List<int>();
            var basicGenerator = new BasicGenerator() { };
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<int>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_For_SimpleValue_DateTime()
        {
            var iEnumerable = new List<DateTime>();
            var basicGenerator = new BasicGenerator() { };
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(DateTime.Parse("1/11/2020"), list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<DateTime>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_With_SeedEnd_GreaterThan_SeedBegin()
        {
            var iEnumerable = new List<Item>();
            var basicGenerator = new BasicGenerator() { SeedBegin = 2, SeedEnd = 1 };
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            // No elements
            Assert.AreEqual(0, list.Count());
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_With_SeedBegin_EqualTo_SeedEnd()
        {
            var iEnumerable = new List<Item>();
            var basicGenerator = new BasicGenerator() { SeedBegin = 1, SeedEnd = 1 };
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            // Should produce 1 row
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void SeedCore_SeedList_Default_With_Different_Counts_Should_Return_Same_At_Ordinal()
        {
            int ordinal = 2; // Ordinal in both lists

            var iEnumerable = new List<Item>();
            var gen = new MultiGenerator(){ SeedBegin = 1, SeedEnd = 10};
            var list1 = new SeedCore(gen).SeedList(iEnumerable).ToList();

            var iEnumerable2 = new List<Item>();
            var gen2 = new MultiGenerator(){ SeedBegin = 1, SeedEnd = 30};
            var list2 = new SeedCore(gen2).SeedList(iEnumerable2).ToList();

            Assert.AreEqual(list1[ordinal].ItemId, list2[ordinal].ItemId);
            Assert.AreEqual(list1[ordinal].ItemName, list2[ordinal].ItemName);
            Assert.AreEqual(list1[ordinal].Number, list2[ordinal].Number);
            //Assert.AreEqual(list1[ordinal].Created, list2[ordinal].Created);
        }

        [Test]
        public void SeedCore_SeedList_Uses_Default_Generator_If_None_Passed_In_Constructor ()
        {
            var iEnumerable = new List<Item>();
            var seedCore = new SeedCore();
            var list = seedCore.SeedList(iEnumerable).ToList();

            // Uses MultilGenerator by default
            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("gear", list[9].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_Passed_In_Xml_From_File ()
        {
            var list = new List<Item>();
            var multiGenerator =  new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var seedCore = new SeedCore(multiGenerator);
            list = seedCore.SeedList(list).ToList();

            // Uses MultiGenerator by default
            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(1, list[0].ItemId);
            Assert.AreEqual("thingamajig", list[0].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_Passed_In_Xml_From_File_With_Unknown_Object()
        {
            var iEnumerable = new List<Unknown>();
            var multiGenerator =  new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var seedCore = new SeedCore(multiGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            // Uses MultiGenerator by default
            Assert.AreEqual(10, list.Count());

            // 'Name' does not match any specific rules but the basic String rule
            // and  Name is not in the xml sourcedata so is propertyName + rownumber
            Assert.AreEqual("Name1", list[0].Name);
        }
       
        // Test XmlDataSource with MultiGenerator

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_XmlDataSource_DefaultData()
        {
            var iEnumerable = new List<Item>();
            var xmlDataSource = new XmlDataSource();
            var multiGenerator = new MultiGenerator(xmlDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("thingamabob", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_XmlDataSource_Parse()
        {
            var iEnumerable = new List<Item>();
            var xmlDataSource = new XmlDataSource();
            xmlDataSource.Load(pathToTestXmlFile);
            var multiGenerator = new MultiGenerator(xmlDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("thingamajig", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_XmlDataSource_Load()
        {
            var iEnumerable = new List<Item>();
            var xmlDataSource = new XmlDataSource();
            xmlDataSource.Parse(testValidXml);
            var multiGenerator = new MultiGenerator(xmlDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("gadget", list[0].ItemName);
        }

        // Test JsonDataSource with MultiGenerator

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_JsonDataSource_DefaultData()
        {
            var iEnumerable = new List<Item>();
            var jsonDataSource = new JsonDataSource();
            var multiGenerator = new MultiGenerator(jsonDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("thingamabob", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_JsonDataSource_Parse()
        {
            var iEnumerable = new List<Item>();
            var jsonDataSource = new JsonDataSource();
            jsonDataSource.Load(pathToTestJsonFile);
            var multiGenerator = new MultiGenerator(jsonDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("thingamajig", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_JsonDataSource_Load()
        {
            var iEnumerable = new List<Item>();
            var jsonDataSource = new JsonDataSource();
            jsonDataSource.Parse(testValidJson);
            var multiGenerator = new MultiGenerator(jsonDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("gadget", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_With_MultiGenerator_Using_JsonDataSource()
        {
            var iEnumerable = new List<Item>();
            var jsonDataSource = new JsonDataSource();
            jsonDataSource.Load(pathToTestJsonFile);
            var multiGenerator = new MultiGenerator(jsonDataSource);
            var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("thingamajig", list[0].ItemName);
        }

        [Test]
        public void SeedCore_SeedList_Seed_Dictionary()
        {
            IDictionary<int, Item> list = new Dictionary<int, Item>();
            var gen = new MultiGenerator() { SeedBegin = 1, SeedEnd = 10 };
            list = new SeedCore(gen).SeedList(list);

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(1, list[1].ItemId);
            Assert.AreEqual("thingamabob", list[1].ItemName);
            Assert.IsInstanceOf<Item>(list[1]);
        }

        [Test]
        public void SeedCore_SeedList_Seed_Dictionary_With_CustomName()
        {
            int count = 10;
            IDictionary<string, Item> list = new Dictionary<string, Item>();
            var gen = new MultiGenerator(pathToTestJsonFile) { SeedBegin = 1, SeedEnd = count, CustomName = "City" };
            list = new SeedCore(gen).SeedList(list);

            Assert.AreEqual(count, list.Count());
            // NOTE How 10 rows of a Dictionary using the CustomName 'City' works, 
            // but CustomName 'County' throws an error. This is because the 'County'
            // rule is randomly picked from the json datasource and the 4th default 
            // "County" is a duplicate -> which is not allowed for an index...
            // Use a 
        }

        [Test]
        public void SeedCore_SeedList_Of_String_With_Defaults_Using_CustomName()
        {
            int count   = 8;
            var list    = new List<string>();
            var gen     = new MultiGenerator() { SeedBegin = 1, SeedEnd = count, CustomName = "FirstName" };
            list        = new SeedCore(gen).SeedList(list).ToList();

            Assert.AreEqual(count, list.Count());

            Assert.AreEqual("Tiffany",      list[0]);
            Assert.AreEqual("Robert",       list[1]);
            Assert.AreEqual("Nicholas",     list[2]);
            Assert.AreEqual("Jacqueline",   list[3]);
            Assert.AreEqual("Mary",         list[4]);
            Assert.AreEqual("Treymayne",    list[5]);
            Assert.AreEqual("Omar",         list[6]);
            Assert.AreEqual("Courtney",     list[7]);
        }

        [Test]
        public void SeedCore_SeedList_Of_String_From_JSON_Using_CustomName()
        {
            int count = 8;
            var list = new List<string>();
            var gen = new MultiGenerator(pathToTestJsonFile) { SeedBegin = 1, SeedEnd = count, CustomName = "City" };
            list = new SeedCore(gen).SeedList(list).ToList();

            Assert.AreEqual(count, list.Count());

            Assert.AreEqual("Brighton",     list[0]);
            Assert.AreEqual("Lincolnton",   list[1]);
            Assert.AreEqual("WestLake",     list[2]);
            Assert.AreEqual("Gotham",       list[3]);
            Assert.AreEqual("Jefferson",    list[4]);
            Assert.AreEqual("Sierra",       list[5]);
            Assert.AreEqual("Kingston",     list[6]);
            Assert.AreEqual("Oakleaf",      list[7]);
        }


        [Test]
        public void SeedCore_NameTuple_Test()
        {
            var dict = new Dictionary<(int ConfId, int DivId, int TeamId), string>
            {
                { (ConfId: 1, DivId: 1, TeamId: 1), "Atlanta Falcons" },
                { (1,1,2), "New Orleans Saints" },
                { (1,1,3), "Tampa Bay Buccaneers"},
                { (2,2,1), "Chicago Bears" },
                { (2,3,1), "New England Patriots"}
            };

            var keys = dict.Keys.ToList();

            Assert.AreEqual(5, dict.Count());
            Assert.AreEqual(5, keys.Count());
            Assert.AreEqual(3, dict.Keys.Where(w => w.ConfId == 1).Count());
            Assert.AreEqual(3, dict.Keys.Where(w => w.Item1 == 1).Count());
            Assert.AreEqual("Atlanta Falcons", dict.Where(w => w.Key.ConfId == 1).ElementAt(0).Value);
        }

        //[Test]
        //public void SeedCore_SeedOne()
        //{
        //    var list = new Dictionary<int, Item>();
        //    var item = new Item();
        //    var gen = new MultiGenerator() { SeedBegin = 1, SeedEnd = 10 };
        //    var one = new SeedCore(gen).SeedList(item);

        //    Assert.AreEqual(1, one.ItemId);
        //    Assert.AreEqual("thingamabob", one.ItemName);
        //    Assert.AreEqual(1, one.Number);
        //    Assert.AreEqual(DateTime.Parse("2021-08-15 07:00:00.000"), one.Created);
        //}

        // List<string>
        // List<int>
        // List<DateTime>
        // List<struct<int, string>>
        // List<Dictionary<int,T>>

        /* =====================================================================================
         * PRIVATE METHODS
         * ================================================================================== */

        private string GetValidXml()
        {
            return @"<Root>
                        <FirstNames>
                            <FirstName>Bob</FirstName>
                            <FirstName>Will</FirstName>
                            <FirstName>John</FirstName>
                            <FirstName>Joe</FirstName>
                        </FirstNames>
                        <ProductNames>
		                    <ProductName>dooHickey</ProductName>
		                    <ProductName>gadget</ProductName>
		                    <ProductName>widget</ProductName>
		                    <ProductName>thingamajig</ProductName>
                        </ProductNames>
                    </Root>";
        }

        private string GetEmptyXml()
        {
            return @"<Root></Root>";
        }

        private string GetValidJson()
        {
            return @"{ ""Root"": 
                         {
                           ""FirstNames"": {
                              ""FirstName"" :
                              [
                                ""John"",
                                ""Patricia"",
                                ""Michael"",
                                ""Susan""
                              ]},
                           ""ProductNames"": {
                              ""ProductName"":
                              [
                                ""dooHickey"",
                                ""gadget"",
                                ""widget"",
                                ""thingamajig""
                              ]}
                         }
                    }";
        }

        private string GetEmptyJson()
        {
            return @"{""Root"": {}}";
        }
    }
}
