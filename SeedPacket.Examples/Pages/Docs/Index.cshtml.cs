using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SeedPacket.Examples.Pages
{
    public class DocsModel : PageModel
    {
        private readonly ILogger<DocsModel> _logger;

        public DocsModel(ILogger<DocsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
