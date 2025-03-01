using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SeedPacket.Extensions;
using SeedPacket.Tests.Model;
using SeedPacket.Interfaces;
using System.Diagnostics;
using WildHare.Extensions;
using NUnit.Framework.Internal;
using System.Reflection.Emit;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using Microsoft.Testing.Platform.OutputDevice;


namespace SeedPacket.Tests
{
    [TestFixture]
    public class SeedFunctionsTests
    {

        private string pathToTestXmlFile;
        private string pathToTestJsonFile;
		private readonly string xmlFile = @"Source\SimpleSeedSource.xml";
		private readonly string jsonFile = @"Source\JsonSeedSource.json";

		[SetUp]
        public void Setup()
        {
			var commonRoot = GetApplicationRoot();
			pathToTestXmlFile = Path.Combine( commonRoot, xmlFile);
			pathToTestJsonFile = Path.Combine(commonRoot, jsonFile); 
		}

		public static string GetApplicationRoot()
		{
			// OLD VERSION: var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
		}

		[Test]
        public void SeedFunctions_DiceRoll_Basic_Roll()
        {
            // Uses default seed for Random so values
            // are constants which results in 4

            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen);
            Assert.AreEqual(4, diceRoll);

            // Iterate to the next RowRandom which results in 1
            gen.GetNextRowRandom();
            diceRoll = Funcs.DiceRoll(gen);
            Assert.AreEqual(3, diceRoll);

            // Iterate to the next RowRandom which results in 1
            gen.GetNextRowRandom();
            diceRoll = Funcs.DiceRoll(gen);
            Assert.AreEqual(1, diceRoll);

            // Iterate to the next RowRandom 3
            gen.GetNextRowRandom();
            diceRoll = Funcs.DiceRoll(gen);
            Assert.AreEqual(5, diceRoll);
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
            var gen = new MultiGenerator(baseRandom: new Random(1234));
            string loremText = Funcs.RandomLoremText(gen, 1, 1);

            Assert.AreEqual("Duis aute irure dolor in reprehenderit in voluptate velit " +
				"esse cillum dolore eu fugiat nulla pariatur.", loremText);
        }

        [Test]
        public void SeedFunctions_RandomText_With_MultiDataSource()
        {
            var gen = new MultiGenerator(baseRandom: new Random(45633));
            string loremText = Funcs.RandomLoremText(gen);

            Assert.AreEqual("Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
				"ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse " +
				"cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.", loremText);
        }

        [Test]
        public void SeedFunctions_RandomText_With_Defaults()
        {
            var gen = new MultiGenerator();
            string loremText = Funcs.RandomLoremText(gen);

            Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor " +
				"incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation " +
				"ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit " +
				"in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint " +
				"occaecat cupidatat non proident.", loremText);
        }


		[Test]
		public void SeedFunctions_RandomBodyCopy_With_Defaults()
		{
			var gen = new MultiGenerator();
			var bodyCopyList = Funcs.RandomBodyCopy(gen);

			Assert.AreEqual(8, bodyCopyList.Count);

			Assert.AreEqual("Sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, " +
				"consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim " +
				"ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
				"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore " +
				"eu fugiat nulla pariatur.", bodyCopyList[0]);

			Assert.AreEqual("Excepteur sint occaecat cupidatat non proident. Sunt in culpa qui officia deserunt mollit " +
				"anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor " +
				"incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation " +
				"ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit " +
				"in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", bodyCopyList[3]);

			Assert.AreEqual("Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea " +
				"commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu " +
				"fugiat nulla pariatur.", bodyCopyList[7]);
		}


		[Test]
		public void SeedFunctions_RandomBodyCopy_With_Params()
		{
			var gen = new MultiGenerator(baseRandom: new Random(5678));
			var bodyCopyList = Funcs.RandomBodyCopy(gen, 1, 4, 1, 4);

			Assert.AreEqual(2, bodyCopyList.Count);

			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do " +
				"eiusmod tempor incididunt ut labore et dolore magna aliqua.", bodyCopyList[0]);

			Assert.AreEqual("Duis aute irure dolor in reprehenderit in voluptate velit esse " +
				"cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat " +
				"non proident. Sunt in culpa qui officia deserunt mollit " +
				"anim id est laborum.", bodyCopyList[1]);
		}

		[Test]
		public void SeedFunctions_RandomBodyCopy_AsString()
		{
			var gen = new MultiGenerator(baseRandom: new Random(5678));
			var bodyCopyList = Funcs.RandomBodyCopy(gen, 1, 4, 1, 4);

			string bodyCopy = bodyCopyList.AsString(" ");

			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do " +
				"eiusmod tempor incididunt ut labore et dolore magna aliqua. Duis aute irure dolor " +
				"in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
				"Excepteur sint occaecat cupidatat non proident. Sunt in culpa qui officia " +
				"deserunt mollit anim id est laborum.", bodyCopy);
		}

		[Test]
		public void SeedFunctions_GetObjectNext_FromJson_Throws_Exception()
		{
            void ExceptionIfInvalidObjectName()
            {
                var gen = new MultiGenerator(sourceFilepath: pathToTestJsonFile);
                var item = Funcs.GetObjectNext<Item>(gen, "InvalidName");
            }

            Assert.Throws<ArgumentNullException>(() => ExceptionIfInvalidObjectName());
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

            gen.RowNumber++;
            var item2 = Funcs.GetObjectNext<Item>(gen, "Item");

            Assert.IsNotNull(item2);
            Assert.AreEqual(2, item2.ItemId);
            Assert.AreEqual("TestName2", item2.ItemName);
            Assert.AreEqual(200, item2.Number);
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

        [Test]
        public void Test_RunDiceRoll()
        {
            var gen = new BasicGenerator(baseRandom: new Random(3432));

            // Uses custom seed for Random so diceroll is 6 so add bonus
            int diceRoll = Funcs.DiceRoll(gen);

            diceRoll =   AddDiceBonusesToRoll(gen, 0, diceRoll, 0);
            Debug.WriteLine("Final DiceRoll: " + diceRoll);

            Assert.AreEqual(4, diceRoll);
        }

        [TestCase(3432, 9,  Description = "")]
        [TestCase(3433, 3,  Description = "")]
        [TestCase(3434, 7,  Description = "")]
        [TestCase(3435, 2,  Description = "")]
        [TestCase(3436, 7,  Description = "")]
        [TestCase(3437, 1,  Description = "")]
        [TestCase(3438, 4,  Description = "")]
        [TestCase(3439, 7,  Description = "")]
        [TestCase(3440, 3,  Description = "")]
        [TestCase(3441, 6,  Description = "")]
        [TestCase(3442, 2,  Description = "")]
        [TestCase(3443, 29, Description = "")]
        [TestCase(3444, 1,  Description = "")]
        [TestCase(3445, 4,  Description = "")]
        [TestCase(3446, 48, Description = "")]
        [TestCase(3447, 3,  Description = "")]
        [TestCase(3448, 22, Description = "")]
        [TestCase(3449, 2,  Description = "")]
        [TestCase(3450, 17, Description = "")]
        [TestCase(3451, 1,  Description = "")]
        [TestCase(3452, 4,  Description = "")]
        [TestCase(3453, 55, Description = "")]
        [TestCase(3454, 3,  Description = "")]
        [TestCase(3456, 2,  Description = "")]
        [TestCase(3457, 51, Description = "")]
        [TestCase(3458, 1,  Description = "")]
        [TestCase(3459, 4,  Description = "")]
        [TestCase(3460, 9,  Description = "")]
        [TestCase(3461, 3,  Description = "")]
        [TestCase(3462, 9,  Description = "")]
        [TestCase(3463, 2,  Description = "")]
        [TestCase(3464, 7,  Description = "")]
        [TestCase(3465, 1,  Description = "")]

        public void Test_Multiple_RunDiceRoll(int seed, int result)
        {
            var gen = new BasicGenerator(baseRandom: new Random(seed));

            Debug.WriteLine("=".Repeat(30));

            // Uses custom seed for Random so diceroll will be 6 and add bonus
            int diceRoll =   AddDiceBonusesToRoll(gen, 0, 0, 0);

            Debug.WriteLine($"Final DiceRoll: {diceRoll}");

            Assert.AreEqual(result, diceRoll);
        }



        // ==============================================================================================

        private int AddDiceBonusesToRoll(IGenerator gen, int total, int roll, int level, bool subtract = false)
        {
            if (total >= 100)
                return total;

            if(level == 0)
                roll = Funcs.DiceRoll(gen);

            total = total + roll;

            // Add BONUS if needed
            if (roll > 1)
            {
                if (roll >= 5-level)
                {
                    gen.GetNextRowRandom();
                    int diceRoll = Funcs.DiceRoll(gen);
                    total = AddDiceBonusesToRoll(gen, total, diceRoll, level + 1);
                }
            }
            else // ie: 1
            {
                // gen.GetNextRowRandom();
                // int diceRoll = -1 * Funcs.DiceRoll(gen);
                // total = AddDiceBonusesToRoll(gen, total, diceRoll, level + 1, true);
            }

            return total;
        }

        private static string GetEmptyXml()
        {
            return @"<Root></Root>";
        }

        // ==============================================================================================

    }
}
