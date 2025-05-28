using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DnDAdventure.Web.Pages
{
    public class AdventureModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AdventureModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ApiBaseUrl { get; private set; } = string.Empty;
        public string? GameStateId { get; private set; }
        public string? CharacterId { get; private set; }

        public IActionResult OnGet(string? gameStateId, string? characterId)
        {
            // If no game state ID is provided, redirect to home page
            if (string.IsNullOrEmpty(gameStateId))
            {
                return RedirectToPage("/Index");
            }

            ApiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7001";
            GameStateId = gameStateId;
            CharacterId = characterId;

            return Page();
        }
    }
}
