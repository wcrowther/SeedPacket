using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SeedPacket.Examples.Logic.Managers;
using SeedPacket.Examples.Logic.Models;
using SeedPacket.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket.Examples.Pages;

public class IndexModel(IWebHostEnvironment webHostEnvironment) : PageModel
{
	private readonly IWebHostEnvironment _env = webHostEnvironment;

	public List<User> UserList { get; set; } = new List<User>().Seed(20).ToList();

	public List<Company> ExampleList { get; set; } = [];

	public void OnGet()
    {
		ExampleList = new ExampleManager(_env).GetExampleList();
	}
}
