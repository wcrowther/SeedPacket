using NUnit.Framework;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.IO;
using System.Xml.Linq;
using Tests.SeedPacket.Models;
using WildHare.Extensions;
using static Tests.SeedPacket.Common;

namespace Tests.SeedPacket
{
    [TestFixture]
    public class SeedFunctionsTests
    {

        private string pathToTestXmlFile;
        private string pathToTestJsonFile;
        private string pathToTestCsvFile;
        private readonly string xmlFile = @"SimpleSeedSource.xml";
        private readonly string jsonFile = @"JsonSeedSource.json";
        private readonly string csvFile = @"CsvSeedSource.csv";

        [SetUp]
        public void Setup()
        {
			pathToTestXmlFile = Path.Combine(GetApplicationRoot() + "\\Source\\", xmlFile);
            pathToTestJsonFile = Path.Combine(GetApplicationRoot() + "\\Source\\", jsonFile);
            pathToTestCsvFile = Path.Combine(GetApplicationRoot() + "\\Source\\", csvFile);
        }

        [Test]
        public void SeedFunctions_DiceRoll_Basic_Roll()
        {
            // Uses default seed for Random so value 4 is constant
            int expected = 4;

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen);

            Assert.AreEqual(expected, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_20_Sided_Dice()
        {
            // Uses default seed for Random so value 13 is constant
            int expected = 13;

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 20);

            Assert.AreEqual(expected, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_5d6()
        {
            // Uses default seed for Random so value 22 is constant
            int expected = 22;

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 6, 5);

            Assert.AreEqual(expected, diceRoll);
        }

        [Test]
        public void SeedFunctions_RandomText_One_Sentence_One_Word()
        {
            var gen = new MultiGenerator();
            string loremText = Funcs.RandomLoremText(gen, 1, 1, 1, 1);

            Assert.AreEqual("Lorem.", loremText);
        }

        [Test]
        public void SeedFunctions_RandomText_With_MultiDataSource_Empty_Data()
        {
            var gen = new MultiGenerator(sourceString: GetEmptyXml());
            string loremText = Funcs.RandomLoremText(gen, 5, 5, 1, 1);

            Assert.AreEqual("Lorem lorem lorem lorem lorem.", loremText);
        }

        [Test]
        public void SeedFunctions_RandomText_With_Defaults()
        {
            var gen = new MultiGenerator();
            string loremText = Funcs.RandomLoremText(gen);
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit sed do eisumod. Tempor incidicunt ut labore et dolore, magna aliqua ut enim ad minim veniam. Quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat duis aute irure dolor. In reprehenderit in voluptate velit, esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est. Laborum lorem ipsum dolor sit amet consectetur adipiscing. Elit sed do eisumod tempor incidicunt ut.";


            Assert.AreEqual(lorem, loremText);
        }

        [Test]
        public void SeedFunctions_GetObjectNext_FromJson()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestJsonFile);
            var item = Funcs.GetObjectNext<Item>(gen, "Item");

            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ItemId);
            Assert.AreEqual("TestName1", item.ItemName);
            Assert.AreEqual(100, item.Number);
            //Assert.IsNull(item.Created);

            gen.RowNumber++;
            var item2 = Funcs.GetObjectNext<Item>(gen, "Item");

            Assert.IsNotNull(item2);
            Assert.AreEqual(2, item2.ItemId);
            Assert.AreEqual("TestName2", item2.ItemName);
            Assert.AreEqual(200, item2.Number);
            //Assert.AreEqual(DateTime.Parse("2018-02-02 18:25:43.511"), item2.Created);
        }

        [Test]
        public void SeedFunctions_GetObjectNext_FromXML()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var item = Funcs.GetObjectNext<Item>(gen, "Item");

            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ItemId);
            Assert.AreEqual("TestName1", item.ItemName);
            Assert.AreEqual(100, item.Number);
            Assert.IsNull(item.Created);

            var item2 = Funcs.GetObjectNext<Item>(gen, "Item", 1);

            Assert.IsNotNull(item2);
            Assert.AreEqual(2, item2.ItemId);
            Assert.AreEqual("TestName2", item2.ItemName);
            Assert.AreEqual(200, item2.Number);
            Assert.AreEqual(DateTime.Parse("2018-02-02 18:25:43.511"), item2.Created);

            var item3 = Funcs.GetObjectNext<Item>(gen, "Item", 2);

            Assert.IsNotNull(item3);
            Assert.AreEqual(3, item3.ItemId);
            Assert.AreEqual("TestName3", item3.ItemName);
            Assert.AreEqual(300, item3.Number);
            Assert.IsNull(item3.Created);
        }

        [Test]
        public void SeedFunctions_GetElementNext_FromCSV()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestCsvFile);
            var element = Funcs.GetElementNext(gen, "LastName");

            Assert.IsNotNull(element);

        }

        [Test]
        public void SeedFunctions_GetObjectNext_FromCSV()
        {
            var gen = new MultiGenerator(sourceFilepath: pathToTestCsvFile);
            var item = Funcs.GetObjectNext<Item>(gen, "Item");


            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ItemId);
            Assert.AreEqual("TestName1", item.ItemName);
            Assert.AreEqual(100, item.Number);
            Assert.IsNull(item.Created);

            var item2 = Funcs.GetObjectNext<Item>(gen, "Item", 1);

            Assert.IsNotNull(item2);
            Assert.AreEqual(2, item2.ItemId);
            Assert.AreEqual("TestName2", item2.ItemName);
            Assert.AreEqual(200, item2.Number);
            Assert.AreEqual(DateTime.Parse("2018-02-02 13:25:43.511"), item2.Created);

            var item3 = Funcs.GetObjectNext<Item>(gen, "Item", 2);

            Assert.IsNotNull(item3);
            Assert.AreEqual(3, item3.ItemId);
            Assert.AreEqual("TestName3", item3.ItemName);
            Assert.AreEqual(300, item3.Number);
            Assert.AreEqual(DateTime.Parse("2018-03-03 13:25:43.511"), item3.Created);
        }

        [Test]
        public void Test_Convert_ChangeType()
        {
            var item = new Item();
            var value = 1;
            var itemId_property = item.GetMetaProperties()[0]; // ItemId is int
            itemId_property.SetInstanceValue(value);

            Assert.AreEqual(value, item.ItemId);
        }

        [Test]
        public void SeedFunctions_GetObjectNext_FromXML_Using_ToObject()
        {

            var xElementString = "<Item ItemId=\"1\" ItemName=\"TestName1\" Number=\"100\" Created=\"\"></Item>";
            var xElement = XElement.Parse(xElementString);
            var item = xElement.ToObject<Item>();

            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.ItemId);
            Assert.AreEqual("TestName1", item.ItemName);
            Assert.AreEqual(100, item.Number);
            Assert.IsNull(item.Created);
        }

        private string GetEmptyXml()
        {
            return @"<Root></Root>";
        }

        [Test]
        public void SeedFunctions_GetElementRandom_Dynamic()
        {
            // Always returns the same value because by default the generator
            // uses the same seed value and gets 10,000 from the xmlFile.

            var gen = new MultiGenerator(sourceFilepath: pathToTestXmlFile);
            var number = gen.GetElementRandom("Numbers", TypeCode.Int64);

            Assert.AreEqual(10,000 , number);
        }
    }
}
