using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using SeedPacket.Examples.Logic.Helpers;
using SeedPacket.Examples.Logic.Interfaces;
using System;

namespace SeedPacket.Examples.Pages
{
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

        
        // TODO Revisit this later. I want to understand how to do AJAX (and Partial page) from web page.
        // Currently I have a DataController that returns the Partial page.
        //
        // public JsonResult OnGetFootballInfo(int seed, int year, DateTime? customSunday = null)
        // {
        //     var openingSunday = new DateTime(year, 1, 1).SecondSundayInSeptember(); // Usual start of season
           
        //     var info = _teamsManager.GetGamesInfo(new Random(seed), customSunday ?? openingSunday);
           
        //     return new JsonResult(info);
        // }
    }
}


// ===========================================================================================================
// Partial page refresh
// ===========================================================================================================
// https ://www.learnrazorpages.com/razor-pages/ajax/partial-update
// ===========================================================================================================
//
// 1) Update @page to add "{handler?}" -- allows cleaner handler URLs
// 2) Call like (jquery):
//     $('#load').on('click', function() {
//        $('#grid').load('/ajaxpartial/CarPartial'); // instead of ?handler=CarPartial 
//      });
//
// public PartialViewResult OnGetFootballInfo()
// {
//     data = getData();
//     return new PartialViewResult
//     {
//         ViewName = "_DataPartial",
//         ViewData = new ViewDataDictionary<List<Data>>(ViewData, data)
//     };
// }

// ===========================================================================================================
// Ajax with Web Pages
// ===========================================================================================================
// https ://www.thereformedprogrammer.net/asp-net-core-razor-pages-how-to-implement-ajax-requests/
//
//