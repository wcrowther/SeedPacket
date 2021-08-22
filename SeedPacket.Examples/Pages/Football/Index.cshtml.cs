using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SeedPacket.Examples.Pages
{
    public class FootballModel : PageModel
    {
        private readonly ILogger<FootballModel> _logger;

        public FootballModel(ILogger<FootballModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
