using NUnit.Framework;
using SeedPacket.DataSources;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Tests.SeedPacket.Core.Model;
using WildHare.Extensions;

namespace Tests.SeedPacket.Core
{
	[TestFixture]
    public class SeedFunctionsTests
    {

        private string pathToTestXmlFile;
        private string pathToTestJsonFile;
		private string xmlFile = @"\Source\SimpleSeedSource.xml";
		private string jsonFile = @"\Source\JsonSeedSource.json";

		[SetUp]
        public void Setup()
        {
			var root = IOExtensions.GetApplicationRoot();
			var commonRoot = GetApplicationRoot();
			pathToTestXmlFile = xmlFile.ToMapPath();
			pathToTestJsonFile = jsonFile.ToMapPath();
		}

		public static string GetApplicationRoot()
		{
			var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
		}

		[Test]
        public void SeedFunctions_DiceRoll_Basic_Roll()
        {
            // Uses default seed for Random so value 4 is constant
            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen);

            Assert.AreEqual(4, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_20_Sided_Dice()
        {
            // Uses default seed for Random so value 13 is constant

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 20);

            Assert.AreEqual(13, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_5d6()
        {
            // Uses default seed for Random so value 4 is constant

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 6, 5);

            Assert.AreEqual(22, diceRoll);
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
    }
}
