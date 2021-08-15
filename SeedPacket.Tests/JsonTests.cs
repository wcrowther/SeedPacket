using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SeedPacket;
using SeedPacket.Exceptions;
using System.Collections.Generic;
using System.IO;
using SeedPacket.Tests.Models;
using WildHare.Extensions;
using static SeedPacket.Tests.Common;


namespace SeedPacket.Tests
{
    [TestFixture]
    public class JsonTests
    {
        private string pathToTestJsonFile;
        private string jsonFile = @"JsonSeedSource.json";
        private string jsonString;

        [SetUp]
        public void Setup()
        {

            pathToTestJsonFile = Path.Combine(GetApplicationRoot() + "\\Source\\", jsonFile);
            var fileInfo = new FileInfo(pathToTestJsonFile);
            jsonString = fileInfo.ReadFile();
        }


        // IN PROGRESS - SEE JsonHelper.cs
        [Test]
        public void JsonList_Test()
        {
            var list = new List<string>();

            // object obj = JsonHelper.Deserialize(jsonString);

            JObject json = JObject.Parse(jsonString);
            JToken jsonList = json["Root"];
            foreach (string fileName in jsonList)
            {
                list.Add(fileName);
            }

            Assert.AreEqual("FirstName", list[0]);
        }
    }
}
