using System;
using NUnit.Framework;
using System.Linq;
using SeedPacket;
using SeedPacket.Exceptions;
using System.IO;
using static Tests.SeedPacket.Common;
using SeedPacket.Generators;
using Tests.SeedPacket.Model;
using System.Collections.Generic;

namespace Tests.SeedPacket
{
    [TestFixture]
    public class SeedCoreTests
    {
        private string pathToTestXmlFile;
        private string xmlFile = @"SimpleSeedSource.xml";
        private string testEmptyXml = @"<SimpleSeedTests></SimpleSeedTests>";
        private string testValidXml = @"<SimpleSeedTests>
                                            <FirstNames>
                                                <FirstName>Bob</FirstName>
                                                <FirstName>Will</FirstName>
                                                <FirstName>John</FirstName>
                                                <FirstName>Joe</FirstName>
                                            </FirstNames>
                                        </SimpleSeedTests>";
        [SetUp]
        public void Setup ()
        {
            pathToTestXmlFile = Path.Combine(GetTestDirectory(), xmlFile);
        }

        [Test]
        public void SeedCore_SeedList_With_BasicGenerator_Gets_10_Items_By_Default ()
        {
            var iEnumerable = new List<Item>();
            var basicGenerator =  new BasicGenerator();
            var seedCore = new SeedCore(basicGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("ItemName10", list[9].ItemName);
            Assert.IsInstanceOf<Item>(list[0]);
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
            Assert.AreEqual(list1[ordinal].Created, list2[ordinal].Created);
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
            var iEnumerable = new List<Item>();
            var multiGenerator =  new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var seedCore = new SeedCore(multiGenerator);
            var list = seedCore.SeedList(iEnumerable).ToList();

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
    }
}
