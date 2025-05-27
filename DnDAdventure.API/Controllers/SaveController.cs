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
        
        [HttpPost("import")]
        public ActionResult<World> ImportFromJson([FromBody] ImportJsonRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.JsonContent))
            {
                return BadRequest("JSON content is required");
            }
            
            try
            {
                // Create a temporary file to use the existing LoadFromJson method
                var tempFileName = Path.GetTempFileName();
                System.IO.File.WriteAllText(tempFileName, request.JsonContent);
                
                if (_worldService.LoadWorld(tempFileName))
                {
                    // Clean up temp file
                    System.IO.File.Delete(tempFileName);
                    
                    return Ok(_worldService.CurrentWorld);
                }
                
                // Clean up temp file on failure
                System.IO.File.Delete(tempFileName);
                return BadRequest("Failed to import JSON - invalid format or structure");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to import JSON: {ex.Message}");
            }
        }
        
        [HttpPost("import-file")]
        public async Task<ActionResult<World>> ImportFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided");
            }
            
            if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a JSON file");
            }
            
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                var jsonContent = await reader.ReadToEndAsync();
                
                // Create a temporary file to use the existing LoadFromJson method
                var tempFileName = Path.GetTempFileName();
                await System.IO.File.WriteAllTextAsync(tempFileName, jsonContent);
                
                if (_worldService.LoadWorld(tempFileName))
                {
                    // Clean up temp file
                    System.IO.File.Delete(tempFileName);
                    
                    return Ok(_worldService.CurrentWorld);
                }
                
                // Clean up temp file on failure
                System.IO.File.Delete(tempFileName);
                return BadRequest("Failed to import file - invalid JSON format or structure");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to import file: {ex.Message}");
            }
        }
        
        [HttpPost("validate-json")]
        public ActionResult<ValidationResult> ValidateJson([FromBody] ImportJsonRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.JsonContent))
            {
                return BadRequest("JSON content is required");
            }
            
            try
            {
                // Try to deserialize without actually loading into the service
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                
                var testWorld = System.Text.Json.JsonSerializer.Deserialize<World>(request.JsonContent, options);
                
                if (testWorld != null)
                {
                    var result = new ValidationResult
                    {
                        IsValid = true,
                        WorldName = testWorld.Name,
                        CharacterCount = testWorld.Characters.Count,
                        MapCount = testWorld.Maps.Count,
                        NPCCount = testWorld.NPCs.Count,
                        Message = "JSON is valid and ready for import"
                    };
                    
                    return Ok(result);
                }
                
                return Ok(new ValidationResult
                {
                    IsValid = false,
                    Message = "JSON structure is invalid"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ValidationResult
                {
                    IsValid = false,
                    Message = $"JSON validation failed: {ex.Message}"
                });
            }
        }
        
        public class SaveGameRequest
        {
            public string WorldName { get; set; } = string.Empty;
        }
        
        public class ImportJsonRequest
        {
            public string JsonContent { get; set; } = string.Empty;
        }
        
        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public string WorldName { get; set; } = string.Empty;
            public int CharacterCount { get; set; }
            public int MapCount { get; set; }
            public int NPCCount { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }
}
