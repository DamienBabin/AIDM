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
                    TerrainType = map.Grid[x][y].TerrainType.ToString(),
                    LocationName = map.Grid[x][y].Name,
                    Description = map.Grid[x][y].Description
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting character location: {ex.Message}");
            }
        }

        [HttpGet("state/{characterId}")]
        public ActionResult<MapPlayState> GetMapState(Guid characterId)
        {
            try
            {
                var (map, x, y) = _mapService.GetCharacterLocation(characterId);

                if (map == null)
                    return NotFound("Character location not found");

                var poisById = _mapService.GetMapPOIs(map.Id).ToDictionary(p => p.Id);
                var cells = new List<HexCellInfo>();
                for (var cellY = 0; cellY < 9; cellY++)
                {
                    for (var cellX = 0; cellX < 9; cellX++)
                    {
                        var cell = map.Grid[cellX][cellY];
                        poisById.TryGetValue(cell.PointOfInterestId, out var poi);
                        cells.Add(new HexCellInfo
                        {
                            X = cellX,
                            Y = cellY,
                            Name = string.IsNullOrWhiteSpace(cell.Name) ? cell.TerrainType.ToString() : cell.Name,
                            Description = cell.Description,
                            TerrainType = cell.TerrainType.ToString(),
                            Passable = cell.Passable,
                            MovementCost = cell.MovementCost,
                            HasPlayer = cell.HasPlayer,
                            HasNpc = cell.NPCId != Guid.Empty,
                            HasPointOfInterest = cell.PointOfInterestId != Guid.Empty,
                            HasStructure = cell.StructureId != Guid.Empty,
                            IsEntryPoint = cell.Passable && (cellX == 0 || cellX == 8 || cellY == 0 || cellY == 8),
                            PointOfInterestId = cell.PointOfInterestId == Guid.Empty ? null : cell.PointOfInterestId,
                            PointOfInterestActions = poi?.AvailableActions ?? new List<string>()
                        });
                    }
                }

                var movementFeet = _mapService.GetMovementFeet(characterId);

                return Ok(new MapPlayState
                {
                    MapId = map.Id,
                    MapName = map.Name,
                    Description = map.Description,
                    PlayerX = x,
                    PlayerY = y,
                    MovementFeet = movementFeet,
                    HexesPerTurn = Math.Max(1, movementFeet / 5),
                    Cells = cells
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting map state: {ex.Message}");
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
                    Message = $"Moved to {map?.Grid[x][y].Name ?? "new location"}",
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

        [HttpPost("move-to/{characterId}")]
        public async Task<ActionResult<MoveResult>> MoveCharacterTo(Guid characterId, [FromBody] MoveToRequest request)
        {
            try
            {
                var success = await _mapService.MoveCharacterTo(characterId, request.X, request.Y);

                if (!success)
                    return BadRequest(new MoveResult { Success = false, Message = "That hex is outside your movement range or cannot be entered." });

                var (map, x, y) = _mapService.GetCharacterLocation(characterId);

                var cellName = map == null
                    ? "new location"
                    : string.IsNullOrWhiteSpace(map.Grid[x][y].Name)
                        ? map.Grid[x][y].TerrainType.ToString()
                        : map.Grid[x][y].Name;

                return Ok(new MoveResult
                {
                    Success = true,
                    Message = $"Moved to {cellName}",
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

    public class MoveToRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
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

    public class MapPlayState
    {
        public Guid MapId { get; set; }
        public string MapName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public int MovementFeet { get; set; }
        public int HexesPerTurn { get; set; }
        public List<HexCellInfo> Cells { get; set; } = new();
    }

    public class HexCellInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TerrainType { get; set; } = string.Empty;
        public bool Passable { get; set; }
        public int MovementCost { get; set; }
        public bool HasPlayer { get; set; }
        public bool HasNpc { get; set; }
        public bool HasPointOfInterest { get; set; }
        public bool HasStructure { get; set; }
        public bool IsEntryPoint { get; set; }
        public Guid? PointOfInterestId { get; set; }
        public List<string> PointOfInterestActions { get; set; } = new();
    }
}
