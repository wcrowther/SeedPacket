using NUnit.Framework;
using SeedPacket.Exceptions;
using System.IO;
using static SeedPacket.Tests.Common;


namespace SeedPacket.Tests
{
    [TestFixture]
    public class SimpleSeedTests
    {
        private string pathToTestXmlFile;
        private readonly string xmlFile = @"SimpleSeedSource.xml";
        private readonly string testEmptyXml = @"<SimpleSeedTests></SimpleSeedTests>";
        private readonly string testValidXml = @"<SimpleSeedTests>
                                                    <FirstName>Bob</FirstName>
                                                    <FirstName>Will</FirstName>
                                                    <FirstName>John</FirstName>
                                                    <FirstName>Joe</FirstName>
                                                </SimpleSeedTests>";
        [SetUp]
        public void Setup()
        {
            pathToTestXmlFile = Path.Combine(GetApplicationRoot() + "\\Source\\", xmlFile);
        }

        [Test]
        public void SimpleSeed_Throws_With_Incorrect_Xml_File_Name ()
        {
            Assert.Throws<InvalidFilePathException>(
                () => new SimpleSeed("NonExistingFile.xml")
            );
        }

        [Test]
        public void SimpleSeed_Throws_With_Invalid_XmlString ()
        {
            Assert.Throws<InvalidSourceException>(
                () => new SimpleSeed(sourcestring: "not Xml")
            );
        }

        [Test]
        public void SimpleSeed_Throws_When_Both_XmlFilePath_And_XmlString_Are_Supplied ()
        {
            Assert.Throws<MultipleSourceException>(
                () =>  new SimpleSeed(pathToTestXmlFile, testEmptyXml)
            );
        }

        [Test]
        public void SimpleSeed_Returns_Item_From_Inject_When_SourceData_Uses_Embedded_Defaults ()
        {
            // 'John' is the first item in 'inject' list in DEFAULT
            // XML embedded resource from /Generators/XmlGeneratorSource.xml

            var simpleSeed = new SimpleSeed();
            Assert.AreEqual("John", simpleSeed.Next("FirstName", 1));
        }

        [Test]
        public void SimpleSeed_Returns_Item_From_Randomize_When_SourceData_Uses_Embedded_Defaults ()
        {
            // 'Mildred' is the item picked by the 'random' seed in DEFAULT
            // XML embedded resource from /Generators/XmlGeneratorSource.xml

            var simpleSeed = new SimpleSeed();
            Assert.AreEqual("Mildred", simpleSeed.Randomize("FirstName"));
        }

        [Test]
        public void SimpleSeed_Inject_Returns_Empty_String_When_Indentifier_Does_Not_Match_Element ()
        {
            string replacementString = "replacement";
            var simpleSeed = new SimpleSeed();
            Assert.AreEqual("", simpleSeed.Next("NonExistingElement", 1));
            Assert.AreEqual(replacementString, simpleSeed.Next("NonExistingElement", 1, replacementString));
        }

        [Test]
        public void SimpleSeed_Can_Retrieve_Xml_From_File ()
        {
            var simpleSeed = new SimpleSeed(pathToTestXmlFile);
            Assert.AreEqual("Susan", simpleSeed.Randomize("FirstName"));
            Assert.AreEqual("Patricia", simpleSeed.Randomize("FirstName"));
            Assert.AreEqual("William", simpleSeed.Randomize("FirstName"));
        }

        [Test]
        public void SimpleSeed_Can_Retrieve_Xml_From_XmlString ()
        {
            var simpleSeed = new SimpleSeed(sourcestring: testValidXml);
            Assert.AreEqual("John", simpleSeed.Randomize("FirstName"));
            Assert.AreEqual("Bob", simpleSeed.Randomize("FirstName"));
            Assert.AreEqual("John", simpleSeed.Randomize("FirstName"));
        }

    }
}
