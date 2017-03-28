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
        public void SeedCore_SeedList_Uses_Default_Generator_If_None_Passed_In_Constructor ()
        {
            var iEnumerable = new List<Item>();
            var seedCore = new SeedCore();
            var list = seedCore.SeedList(iEnumerable).ToList();

            // Uses MultilGenerator by default
            Assert.AreEqual(10, list.Count());
            Assert.AreEqual(10, list[9].ItemId);
            Assert.AreEqual("gadget", list[9].ItemName);
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
            Assert.AreEqual("dooHickey", list[0].ItemName);
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
