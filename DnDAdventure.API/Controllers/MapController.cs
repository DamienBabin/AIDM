// DnDAdventure.API/Controllers/MapController.cs
using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;
        
        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }
        
        [HttpGet("{characterId}")]
        public ActionResult<string> GetMapDisplay(Guid characterId)
        {
            try
            {
                var mapDisplay = _mapService.GetMapDisplay(characterId);
                return Ok(mapDisplay);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting map display: {ex.Message}");
            }
        }
        
        [HttpGet("location/{characterId}")]
        public ActionResult<LocationInfo> GetCharacterLocation(Guid characterId)
        {
            try
            {
                var (map, x, y) = _mapService.GetCharacterLocation(characterId);
                
                if (map == null)
                    return NotFound("Character location not found");
                    
                return Ok(new LocationInfo
                {
                    MapId = map.Id,
                    MapName = map.Name,
                    X = x,
                    Y = y,
                    TerrainType = map.Grid[x, y].TerrainType.ToString(),
                    LocationName = map.Grid[x, y].Name,
                    Description = map.Grid[x, y].Description
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting character location: {ex.Message}");
            }
        }
        
        [HttpPost("move/{characterId}")]
        public async Task<ActionResult<MoveResult>> MoveCharacter(Guid characterId, [FromBody] MoveRequest request)
        {
            try
            {
                var success = await _mapService.MoveCharacter(characterId, request.Direction);
                
                if (!success)
                    return BadRequest(new MoveResult { Success = false, Message = "Unable to move in that direction" });
                    
                var (map, x, y) = _mapService.GetCharacterLocation(characterId);
                
                return Ok(new MoveResult
                {
                    Success = true,
                    Message = $"Moved to {map?.Grid[x, y].Name ?? "new location"}",
                    NewX = x,
                    NewY = y,
                    NewMapId = map?.Id ?? Guid.Empty,
                    NewMapName = map?.Name ?? string.Empty
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new MoveResult
                {
                    Success = false,
                    Message = $"Error moving character: {ex.Message}"
                });
            }
        }
        
        [HttpGet("explore/{characterId}")]
        public async Task<ActionResult<ExplorationResult>> ExploreCurrentLocation(Guid characterId)
        {
            try
            {
                var result = await _mapService.ExploreCurrentCell(characterId);
                
                if (!result.Success)
                    return BadRequest(result);
                    
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ExplorationResult
                {
                    Success = false,
                    Message = $"Error exploring location: {ex.Message}"
                });
            }
        }
        
        [HttpPost("interact/{characterId}")]
        public async Task<ActionResult<InteractionResult>> InteractWithPOI(Guid characterId, [FromBody] InteractRequest request)
        {
            try
            {
                var result = await _mapService.InteractWithPOI(characterId, request.PointOfInterestId, request.Action);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new InteractionResult
                {
                    Success = false,
                    Message = $"Error interacting with point of interest: {ex.Message}"
                });
            }
        }

        [HttpGet("pois/{mapId}")]
        public ActionResult<List<POIBriefInfo>> GetPointsOfInterestOnMap(Guid mapId)
        {
            try
            {
                var pois = _mapService.GetMapPOIs(mapId);
                return Ok(pois);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting points of interest: {ex.Message}");
            }
        }

        [HttpGet("maps")]
        public ActionResult<List<MapInfo>> GetAllMaps()
        {
            try
            {
                var maps = _mapService.GetAllMaps();

                var mapInfos = maps.Select(m => new MapInfo
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    WorldX = m.WorldX,
                    WorldY = m.WorldY
                }).ToList();

                return Ok(mapInfos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting maps: {ex.Message}");
            }
        }

        [HttpGet("maps/{mapId}")]
        public ActionResult<MapDetailInfo> GetMapDetails(Guid mapId)
        {
            try
            {
                var map = _mapService.GetMap(mapId);

                if (map == null)
                    return NotFound($"Map with ID {mapId} not found");

                var mapDetail = new MapDetailInfo
                {
                    Id = map.Id,
                    Name = map.Name,
                    Description = map.Description,
                    WorldX = map.WorldX,
                    WorldY = map.WorldY,
                    TextDisplay = map.GetMapDisplay(),
                    ConnectedMaps = map.ConnectedMaps.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value
                    )
                };

                return Ok(mapDetail);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting map details: {ex.Message}");
            }
        }
    }

    // Request and response models for the API endpoints
    public class MoveRequest
    {
        public Direction Direction { get; set; }
    }

    public class InteractRequest
    {
        public Guid PointOfInterestId { get; set; }
        public string Action { get; set; } = string.Empty;
    }

    public class LocationInfo
    {
        public Guid MapId { get; set; }
        public string MapName { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public string TerrainType { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class MoveResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int NewX { get; set; }
        public int NewY { get; set; }
        public Guid NewMapId { get; set; }
        public string NewMapName { get; set; } = string.Empty;
    }

    public class MapInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldX { get; set; }
        public int WorldY { get; set; }
    }

    public class MapDetailInfo : MapInfo
    {
        public string TextDisplay { get; set; } = string.Empty;
        public Dictionary<string, Guid> ConnectedMaps { get; set; } = new();
    }
}
