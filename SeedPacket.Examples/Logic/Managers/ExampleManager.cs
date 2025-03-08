using SeedPacket.Examples.Logic.Extensions;
using SeedPacket.Examples.Logic.Interfaces;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Generators;
using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using SeedPacket.Functions;
using Microsoft.AspNetCore.Hosting;

namespace SeedPacket.Examples.Logic.Managers;

public class ExampleManager(IWebHostEnvironment webHostEnvironment) : IExampleManager
{
	public List<Company> GetExampleList(int? rows = 10, int? seed = 1234)
	{
		var genRows = rows > 999 || rows < 0 ? 10 : rows;

		var root = webHostEnvironment.ContentRootPath;

		var generator = new MultiGenerator($@"{root}\Logic\SourceFiles\xmlSeedSourcePlus.xml")
		{
			Rules = 
			{
				new Rule(typeof(string), "ItemName",      g => Funcs.GetElementNext(g, "ProductName"), "ItemName"),
				new Rule(typeof(int), "SelectedProduct",  g => g.RowRandom.Next(0, 11),"Random product id"),
				new Rule(typeof(string), "Ceo",           g => Funcs.GetElementNext(g, "FirstName") + " " + Funcs.GetElementNext(g, "LastName"), "Random CEO Name"),
				new Rule(typeof(string),"Description%",   g => Funcs.GetElementNext(g, "Description"), "Description", "Gets Description from custom XML file" ),
				new Rule(typeof(List<Item>), "",          g => Funcs.GetListFromCacheNext<Item>(g, g.Cache.Items, 10, 10, false), "ItemList")
			},
			BaseDateTime	= DateTime.Today,
			BaseRandom		= new Random(seed ?? 1234)
		};

		generator.Cache.Items = new List<Item>().Seed(1, 10, generator);

		var examples = new List<Company>().Seed(100, 99 + genRows, generator).ToList();

		return examples;
	}
}
