using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;
using DnDAdventure.Infrastructure.Services;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorldController : ControllerBase
    {
        private readonly IWorldService _worldService;
        
        public WorldController(IWorldService worldService)
        {
            _worldService = worldService;
        }
        
        [HttpGet("available")]
        public ActionResult<IEnumerable<WorldInfo>> GetAvailableWorlds()
        {
            try
            {
                var saves = _worldService.GetAvailableSaves();
                var worlds = saves.Select(save => new WorldInfo
                {
                    Id = Path.GetFileNameWithoutExtension(save.FilePath),
                    Name = save.WorldName,
                    Description = save.Description ?? "No description available",
                    CreatedAt = save.CreatedAt,
                    Characters = new List<string>(), // Will be populated when world is loaded
                    Quests = new Dictionary<string, object>(),
                    Locations = new Dictionary<string, object>()
                });
                
                return Ok(worlds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error loading available worlds: {ex.Message}");
            }
        }
        
        [HttpGet("{worldId}")]
        public ActionResult<WorldDetails> GetWorldDetails(string worldId)
        {
            try
            {
                var saves = _worldService.GetAvailableSaves();
                var save = saves.FirstOrDefault(s => Path.GetFileNameWithoutExtension(s.FilePath) == worldId);
                
                if (save == null)
                {
                    return NotFound($"World with ID '{worldId}' not found");
                }
                
                // Load the world to get detailed information
                if (_worldService.LoadWorld(save.FilePath))
                {
                    var world = _worldService.CurrentWorld;
                    if (world != null)
                    {
                        var details = new WorldDetails
                        {
                            Id = worldId,
                            Name = world.Name,
                            Description = world.Description ?? "No description available",
                            CreatedAt = save.CreatedAt,
                            Characters = world.Characters.Values.Select(c => c.Name).ToList(),
                            Quests = world.Quests.ToDictionary(q => q.Key, q => (object)q.Value),
                            Locations = world.Locations.ToDictionary(l => l.Key, l => (object)l.Value)
                        };
                        
                        return Ok(details);
                    }
                }
                
                return BadRequest("Failed to load world details");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error loading world details: {ex.Message}");
            }
        }
        
        [HttpPost("import")]
        public ActionResult<WorldImportResult> ImportWorld([FromBody] WorldImportRequest request)
        {
            if (request == null)
            {
                return BadRequest("Import request is required");
            }
            
            try
            {
                // Create a temporary file to use the existing LoadFromJson method
                var tempFileName = Path.GetTempFileName();
                System.IO.File.WriteAllText(tempFileName, System.Text.Json.JsonSerializer.Serialize(request.World));
                
                if (_worldService.LoadWorld(tempFileName))
                {
                    // Save the imported world with a new name if provided
                    var worldName = request.World.Name ?? "Imported World";
                    var savedPath = _worldService.SaveWorld(worldName);
                    
                    // Clean up temp file
                    System.IO.File.Delete(tempFileName);
                    
                    if (savedPath != null)
                    {
                        var result = new WorldImportResult
                        {
                            Id = Path.GetFileNameWithoutExtension(savedPath),
                            Name = worldName,
                            Success = true,
                            Message = "World imported successfully"
                        };
                        
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("World loaded but failed to save");
                    }
                }
                
                // Clean up temp file on failure
                System.IO.File.Delete(tempFileName);
                return BadRequest("Failed to import world - invalid format or structure");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to import world: {ex.Message}");
            }
        }
        
        [HttpPost("create")]
        public ActionResult<WorldImportResult> CreateWorld([FromBody] CreateWorldRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("World name is required");
            }
            
            try
            {
                // Use the WorldService to create a new world
                var world = _worldService.CreateNewWorld(request.Name, request.Description ?? "");
                
                // Initialize with default content using WorldInitializer
                var worldInitializer = new WorldInitializer((WorldService)_worldService);
                worldInitializer.InitializeNewWorld(request.Name);
                
                // Save the world
                var savedPath = _worldService.SaveWorld(request.Name);
                
                if (savedPath != null)
                {
                    var result = new WorldImportResult
                    {
                        Id = Path.GetFileNameWithoutExtension(savedPath),
                        Name = request.Name,
                        Success = true,
                        Message = "World created successfully"
                    };
                    
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Failed to save new world");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create world: {ex.Message}");
            }
        }
        
        public class WorldInfo
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public List<string> Characters { get; set; } = new();
            public Dictionary<string, object> Quests { get; set; } = new();
            public Dictionary<string, object> Locations { get; set; } = new();
        }
        
        public class WorldDetails : WorldInfo
        {
            // Inherits all properties from WorldInfo
        }
        
        public class WorldImportRequest
        {
            public World World { get; set; } = new();
        }
        
        public class CreateWorldRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
        
        public class WorldImportResult
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }
}
