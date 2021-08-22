using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SeedPacket.Examples.Pages
{
    public class RowsModel : PageModel
    {
        private readonly ILogger<RowsModel> _logger;

        public RowsModel(ILogger<RowsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
