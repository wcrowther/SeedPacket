using NUnit.Framework;
using SeedPacket.Extensions;
using SeedPacket.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeedPacket.Tests.Models;
using static SeedPacket.Tests.Common;
using SeedPacket.Tests.Model;

namespace SeedPacket.Tests
{
	[TestFixture]
    public class SeedExtensionsTests
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
        public void SeedExtensions_SeedList_With_BasicGenerator_Gets_10_Items_By_Default()
        {
            var iEnumerable = new List<Item>().Seed(new BasicGenerator());

            var list = iEnumerable.ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("ItemName10", list[9].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_Can_Have_A_Negative_SeedBegin()
        {
            var gen = new BasicGenerator() { SeedBegin = -10 };
            var iEnumerable = new List<Item>().Seed(gen);

            var list = iEnumerable.ToList();

            // 21 elements because 10 negative, 10 positive, 1 zero
            Assert.AreEqual(21, list.Count());
            Assert.AreEqual(10, list[20].ItemId);
            Assert.AreEqual("ItemName10", list[20].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_For_SimpleValue_Strings()
        {
            var list = new List<string>().Seed(new BasicGenerator()).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual("10", list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<string>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_For_SimpleValue_Ints()
        {
            var list = new List<int>().Seed(new BasicGenerator()).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<int>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_For_SimpleValue_DateTime()
        {
            var list = new List<DateTime>().Seed(new BasicGenerator()).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(DateTime.Parse("1/11/2020"), list[9]); // not 10 because list is zero-based
            Assert.IsInstanceOf<DateTime>(list[0]);
        }

        //[Test]
        //public void SeedExtensions_SeedList_With_MultiGenerator_For_SimpleValue_Strings_With_Custom()
        //{
        //    var list = new List<string>().Seed(new BasicGenerator()).ToList();

        //    Assert.AreEqual(10, list.Count());
        //    Assert.AreEqual("10", list[9]); // not 10 because list is zero-based
        //    Assert.IsInstanceOf<string>(list[0]);
        //}

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_With_SeedEnd_GreaterThan_SeedBegin()
        {
            var list = new List<DateTime>().Seed(new BasicGenerator() { SeedBegin = 2, SeedEnd = 1 }).ToList();

            // No elements
            Assert.AreEqual(0, list.Count());
        }

        [Test]
        public void SeedExtensions_SeedList_With_BasicGenerator_With_SeedBegin_EqualTo_SeedEnd()
        {
            var list = new List<DateTime>().Seed(new BasicGenerator() { SeedBegin = 1, SeedEnd = 1 }).ToList();

            // Should produce 1 row
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void SeedExtensions_SeedList_Default_With_Different_Counts_Should_Return_Same_At_Ordinal()
        {
            int ordinal = 2; // Ordinal in both lists

            var list1 = new List<Item>().Seed(1, 10).ToList();
            var list2 = new List<Item>().Seed(1, 30).ToList();

            Assert.AreEqual(list1[ordinal].ItemId, list2[ordinal].ItemId);
            Assert.AreEqual(list1[ordinal].ItemName, list2[ordinal].ItemName);
            Assert.AreEqual(list1[ordinal].Number, list2[ordinal].Number);
            //Assert.AreEqual(list1[ordinal].Created, list2[ordinal].Created);
        }


        [Test]
        public void SeedExtensions_SeedList_Uses_Default_Generator_If_None_Passed_In_Constructor()
        {
            var list = new List<Item>().Seed().ToList();

            // Uses MultilGenerator by default
            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("gear", list[9].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_MultiGenerator_Using_Passed_In_Xml_From_File()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var list = new List<Item>().Seed(gen).ToList();

            // Uses MultiGenerator by default
            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(1, list[0].ItemId);
            Assert.AreEqual("thingamajig", list[0].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
        }

        [Test]
        public void SeedExtensions_SeedList_With_MultiGenerator_Using_Passed_In_Xml_From_File_With_Unknown_Object()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var list = new List<Unknown>().Seed(gen).ToList();

            // Uses MultiGenerator by default
            Assert.AreEqual(10, list.Count());

            // 'Name' does not match any specific rules but the basic String rule
            // and  Name is not in the xml sourcedata so is propertyName + rownumber
            Assert.AreEqual("Name1", list[0].Name);
        }

        [Test]
        public void SeedExtensions_SeedList_Seed_Dictionary()
        {
            var gen = new MultiGenerator() { SeedBegin = 1, SeedEnd = 10 };
            var list = new Dictionary<int, Item>().Seed(gen);

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(1, list[1].ItemId);
            Assert.AreEqual("thingamabob", list[1].ItemName);
            Assert.IsInstanceOf<Item>(list[1]);
        }

        [Test]
        public void SeedCore_SeedList_Seed_AccountName_Example()
        {
            var list = new List<Account>().Seed().ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(1, list[0].AccountId);
            Assert.AreEqual("Nakatomi Inc", list[0].AccountName);
        }

        //[Test]
        //public void SeedExtensions_SeedList_Integers()
        //{
        //    // var gen = new MultiGenerator();

        //    var integers = new List<int>().Seed(10000,customPropertyName: "random").ToArray();

        //    Assert.AreEqual(1234, integers[8888]);
        //}

        /* =====================================================================================
        *  PRIVATE METHODS
        *  =================================================================================== */

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
