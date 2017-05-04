﻿using NUnit.Framework;
using SeedPacket.Functions;
using SeedPacket.Generators;

namespace Tests.SeedPacket
{
    [TestFixture]
    public class SeedFunctionsTests
    {

        [Test]
        public void SeedFunctions_DiceRoll_Basic_Roll()
        {
            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen);

            Assert.AreEqual(4, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_20_Sided_Dice()
        {
            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 20);

            Assert.AreEqual(13, diceRoll);
        }

        [Test]
        public void SeedFunctions_DiceRoll_5d6()
        {
            var gen = new BasicGenerator();
            int diceRoll = Funcs.DiceRoll(gen, 6, 5);

            Assert.AreEqual(22, diceRoll);
        }

        [Test]
        public void SeedFunctions_RandomText_One_Sentence_One_Word()
        {
            var gen = new MultiGenerator();
            string loremText = Funcs.LocumsText(gen, 1, 1, 1, 1);

            Assert.AreEqual("Lorem.", loremText);
        }

        [Test]
        public void SeedFunctions_RandomText_With_Defaults()
        {
            var gen = new MultiGenerator();
            string loremText = Funcs.LocumsText(gen);

            Assert.AreEqual(lorem, loremText);
        }

        string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit sed do eisumod. Tempor incidicunt ut labore et dolore, magna aliqua ut enim ad minim veniam. Quis nostrud exercitation ullamco laboris nisi ut, aliquip ex ea commodo consequat duis aute irure dolor. In reprehenderit in voluptate velit, esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident sunt, in culpa qui officia deserunt mollit anim id est. Laborum lorem ipsum dolor sit amet consectetur adipiscing. Elit sed do eisumod tempor incidicunt ut.";
    }
}
