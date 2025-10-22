using NUnit.Framework;
using SeedPacket.DataSources;
using SeedPacket.Generators;
using SeedPacket.Tests.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SeedPacket.Tests.Common;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace SeedPacket.Tests;

[TestFixture]
public class SeedAdvancedTests
{
    private string pathToTestJsonFile;
    private const string jsonFile = @"JsonSeedSource.json";
    private string testValidJson;
    private string testEmptyJson;

    [SetUp]
    public void Setup()
    {
        pathToTestJsonFile = Path.Combine(GetApplicationRoot() + "\\Source\\", jsonFile);
        testValidJson = GetValidJson();
        testEmptyJson = GetEmptyJson();
    }

	[Test]
	public void Advanced_SeedList_For_Nested_Type()
	{
		var iEnumerable = new List<AdvancedItem>();
		var jsonDataSource = new JsonDataSource();
		jsonDataSource.Parse(testValidJson);
		var multiGenerator = new MultiGenerator(jsonDataSource);
		var list = new SeedCore(multiGenerator).SeedList(iEnumerable).ToList();

		Assert.AreEqual(10, list.Count);
		Assert.AreEqual("gadget", list[0].Item.ItemName);
	}

	/* ===================================================================== */

	private static string GetValidJson()
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

    private static string GetEmptyJson()
        {
            return @"{""Root"": {}}";
        }
}
