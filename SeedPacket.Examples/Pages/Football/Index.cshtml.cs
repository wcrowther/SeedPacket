using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using SeedPacket.Examples.Logic.Helpers;
using SeedPacket.Examples.Logic.Interfaces;
using System;

namespace SeedPacket.Examples.Pages;

public class FootballModel : PageModel
{
    private readonly ITeamsManager _teamsManager;


    public FootballModel(ITeamsManager teamsManager)
    {
        _teamsManager = teamsManager;
    }

    public void OnGet()
    {
        // Not currently used
    }
}
