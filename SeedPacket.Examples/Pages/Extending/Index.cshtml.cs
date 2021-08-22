using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SeedPacket.Examples.Pages
{
    public class ExtendingModel : PageModel
    {
        private readonly ILogger<ExtendingModel> _logger;

        public ExtendingModel(ILogger<ExtendingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
