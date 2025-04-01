// DnDAdventure.Web/Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DnDAdventure.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string ApiBaseUrl { get; private set; } = string.Empty;

        public void OnGet()
        {
            ApiBaseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }
    }
}