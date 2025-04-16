using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeedPacket.Examples.Logic.Managers;
using SeedPacket.Examples.Logic.Models;
using System.Collections.Generic;

namespace SeedPacket.Examples.Pages;

public class IndexModel(IWebHostEnvironment webHostEnvironment) : PageModel
{
	private readonly IWebHostEnvironment _env = webHostEnvironment;

	public List<User> UserList { get; set; } = [];

	public List<Company> ExampleList { get; set; } = [];

	public void OnGet()
    {
		UserList	= new ExampleManager(_env).GetExampleUsersList();
		ExampleList = new ExampleManager(_env).GetExampleList();
	}
}
