using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SeedPacket.Examples.Pages
{
    public class SimpleSeedModel : PageModel
    {
        private readonly ILogger<SimpleSeedModel> _logger;

        public SimpleSeedModel(ILogger<SimpleSeedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
