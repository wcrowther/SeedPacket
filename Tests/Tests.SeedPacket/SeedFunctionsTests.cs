using NUnit.Framework;
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
    }
}
