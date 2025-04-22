// DnDAdventure.API/Controllers/SaveController.cs
using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;
using DnDAdventure.Infrastructure.Services;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaveController : ControllerBase
    {
        private readonly IWorldService _worldService;
        
        public SaveController(IWorldService worldService)
        {
            _worldService = worldService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<SaveFileInfo>> GetSaves()
        {
            return Ok(_worldService.GetAvailableSaves());
        }
        
        [HttpGet("{filePath}")]
        public ActionResult<World> LoadSave(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path is required");
            }
            
            // Decode the file path
            filePath = System.Web.HttpUtility.UrlDecode(filePath);
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Save file not found: {filePath}");
            }
            
            if (_worldService.LoadWorld(filePath))
            {
                return Ok(_worldService.CurrentWorld);
            }
            
            return BadRequest("Failed to load save file");
        }
        
        [HttpPost("quicksave")]
        public ActionResult<string> CreateQuickSave()
        {
            var filePath = _worldService.CreateQuickSave();
            
            if (filePath != null)
            {
                return Ok(new { filePath });
            }
            
            return BadRequest("Failed to create quicksave");
        }
        
        [HttpPost("save")]
        public ActionResult<string> SaveGame([FromBody] SaveGameRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.WorldName))
            {
                return BadRequest("World name is required");
            }
            
            var filePath = _worldService.SaveWorld(request.WorldName);
            
            if (filePath != null)
            {
                return Ok(new { filePath });
            }
            
            return BadRequest("Failed to save game");
        }
        
        public class SaveGameRequest
        {
            public string WorldName { get; set; } = string.Empty;
        }
    }
}

