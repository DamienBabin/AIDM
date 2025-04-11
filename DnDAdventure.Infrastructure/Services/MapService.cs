// DnDAdventure.Infrastructure/Services/MapService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnDAdventure.Core.Models;

namespace DnDAdventure.Infrastructure.Services
{
    public class MapService
    {
        private readonly WorldService _worldService;

        public MapService(WorldService worldService)
        {
            _worldService = worldService;
        }
/// <summary>
        /// Moves a character on their current map
/// </summary>
        public async Task<bool> MoveCharacter(Guid characterId, Direction direction)
{
    var position = _worldService.CurrentWorld.GetPlayerPosition(characterId);
    if (position == null)
                return false;
    var map = _worldService.CurrentWorld.GetMap(position.CurrentMapId);
    if (map == null)
                return false;

            // Calculate new position based on direction
            int newX = position.X;
            int newY = position.Y;

            switch (direction)
    {
                case Direction.North:
                    newY--;
                break;
                case Direction.East:
                    newX++;
                break;
                case Direction.South:
                    newY++;
                break;
                case Direction.West:
                    newX--;
                    break;
        }

            // Check if new position is within map bounds
            if (newX < 0 || newX >= 9 || newY < 0 || newY >= 9)
{
                // Check if there's a connected map in this direction
                if (map.ConnectedMaps.TryGetValue(direction, out var connectedMapId))
{
                    // Calculate entry point on the new map
                    int entryX = newX;
                    int entryY = newY;

                    // Adjust coordinates for map boundaries
                    switch (direction)
    {
                        case Direction.North:
                            entryY = 8; // Enter from bottom
                            break;
                        case Direction.East:
                            entryX = 0; // Enter from left
                            break;
                        case Direction.South:
                            entryY = 0; // Enter from top
                            break;
                        case Direction.West:
                            entryX = 8; // Enter from right
                            break;
                    }

                    // Check if the target map exists
                    var connectedMap = _worldService.CurrentWorld.GetMap(connectedMapId);
                    if (connectedMap == null)
                        return false;

                    // Check if the entry point is passable
                    if (!connectedMap.Grid[entryX, entryY].Passable)
                        return false;

                    // Move to the new map
                    _worldService.CurrentWorld.SetPlayerPosition(characterId, connectedMapId, entryX, entryY);
                    return true;
                }

                // No connected map, can't move beyond boundaries
                return false;
            }

            // Check if the new position is passable
            if (!map.Grid[newX, newY].Passable)
                return false;

            // Update player position
            _worldService.CurrentWorld.SetPlayerPosition(characterId, position.CurrentMapId, newX, newY);
            return true;
        }
        /// <summary>
        /// Explores the current cell a character is standing on
        /// </summary>
        public async Task<ExplorationResult> ExploreCurrentCell(Guid characterId)
        {
            var position = _worldService.CurrentWorld.GetPlayerPosition(characterId);
            if (position == null)
                return new ExplorationResult { Success = false, Message = "Character position not found." };

            var map = _worldService.CurrentWorld.GetMap(position.CurrentMapId);
            if (map == null)
                return new ExplorationResult { Success = false, Message = "Map not found." };

            var cell = map.Grid[position.X, position.Y];

            var result = new ExplorationResult
            {
                Success = true,
                CellName = cell.Name,
                CellDescription = cell.Description,
                MapName = map.Name,
                TerrainType = cell.TerrainType.ToString()
            };

            // Check for NPCs
            if (cell.NPCId != Guid.Empty)
            {
                var npc = _worldService.CurrentWorld.NPCs.TryGetValue(cell.NPCId, out var npcValue) ? npcValue : null;
                if (npc != null)
                {
                    result.HasNPC = true;
                    result.NPCInfo = new NPCBriefInfo
                    {
                        Id = npc.Id,
                        Name = npc.Name,
                        Description = npc.Description
                    };
                }
            }

            // Check for points of interest
            if (cell.PointOfInterestId != Guid.Empty)
            {
                var poi = _worldService.CurrentWorld.PointsOfInterest.TryGetValue(cell.PointOfInterestId, out var poiValue) ? poiValue : null;
                if (poi != null)
                {
                    result.HasPointOfInterest = true;
                    result.PointOfInterestInfo = new POIBriefInfo
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description,
                        Type = poi.Type.ToString()
                    };
                }
            }

            // Check for structures
            if (cell.StructureId != Guid.Empty)
            {
                var structure = _worldService.CurrentWorld.Structures.TryGetValue(cell.StructureId, out var structureValue) ? structureValue : null;
                if (structure != null)
                {
                    result.HasStructure = true;
                    result.StructureInfo = new StructureBriefInfo
                    {
                        Id = structure.Id,
                        Name = structure.Name,
                        Description = structure.Description,
                        Type = structure.Type.ToString()
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Interacts with a point of interest
        /// </summary>
        public async Task<InteractionResult> InteractWithPOI(Guid characterId, Guid poiId, string action)
        {
            var character = _worldService.CurrentWorld.Characters.TryGetValue(characterId, out var characterValue) ? characterValue : null;
            if (character == null)
                return new InteractionResult { Success = false, Message = "Character not found." };

            var poi = _worldService.CurrentWorld.PointsOfInterest.TryGetValue(poiId, out var poiValue) ? poiValue : null;
            if (poi == null)
                return new InteractionResult { Success = false, Message = "Point of interest not found." };

            // Find the requested action
            var poiAction = poi.AvailableActions.FirstOrDefault(a => a.Name.Equals(action, StringComparison.OrdinalIgnoreCase));
            if (poiAction == null)
                return new InteractionResult { Success = false, Message = $"Action '{action}' not available for this point of interest." };

            // Check requirements
            foreach (var req in poiAction.Requirements)
            {
                // This is a simplified example - you'd want more comprehensive requirement checking
                switch (req.Key)
                {
                    case "MinLevel":
                        if (character.Level < int.Parse(req.Value))
                            return new InteractionResult { Success = false, Message = $"You must be at least level {req.Value} to perform this action." };
                        break;
                    case "HasItem":
                        if (!character.Inventory.Contains(req.Value))
                            return new InteractionResult { Success = false, Message = $"You need a {req.Value} to perform this action." };
                        break;
                    // Add more requirement types as needed
                }
            }

            // Apply effects
            var result = new InteractionResult { Success = true, Message = poiAction.Description };

            foreach (var effect in poiAction.Effects)
            {
                switch (effect.Key)
                {
                    case "AddItem":
                        character.Inventory.Add(effect.Value);
                        result.ItemsGained.Add(effect.Value);
                        break;
                    case "RemoveItem":
                        character.Inventory.Remove(effect.Value);
                        result.ItemsLost.Add(effect.Value);
                        break;
                    case "AddQuest":
                        // Find the game state for this character
                        var gameState = _worldService.CurrentWorld.GameStates.Values
                            .FirstOrDefault(gs => gs.CharacterId == characterId);
                        if (gameState != null && !gameState.ActiveQuests.Contains(effect.Value))
                        {
                            gameState.ActiveQuests.Add(effect.Value);
                            result.QuestsStarted.Add(effect.Value);
                        }
                        break;
                    // Add more effect types as needed
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a detailed map display for a character
        /// </summary>
        public string GetMapDisplay(Guid characterId)
        {
            var position = _worldService.CurrentWorld.GetPlayerPosition(characterId);
            if (position == null)
                return "Character position not found.";

            var map = _worldService.CurrentWorld.GetMap(position.CurrentMapId);
            if (map == null)
                return "Map not found.";

            return $"Map: {map.Name}\n\n{map.GetMapDisplay()}\n\nYou are at coordinates: ({position.X}, {position.Y})";
        }

        /// <summary>
        /// Gets all available points of interest on a map
        /// </summary>
        public List<POIBriefInfo> GetMapPOIs(Guid mapId)
        {
            var map = _worldService.CurrentWorld.GetMap(mapId);
            if (map == null)
                return new List<POIBriefInfo>();

            var result = new List<POIBriefInfo>();

            // Find all cells with POIs
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = map.Grid[x, y];
                    if (cell.PointOfInterestId != Guid.Empty)
                    {
                        var poi = _worldService.CurrentWorld.PointsOfInterest.TryGetValue(cell.PointOfInterestId, out var poiValue) ? poiValue : null;
                        if (poi != null)
                        {
                            result.Add(new POIBriefInfo
                            {
                                Id = poi.Id,
                                Name = poi.Name,
                                Description = poi.Description,
                                Type = poi.Type.ToString(),
                                X = x,
                                Y = y
                            });
                        }
                    }
                }
            }

            return result;
        }

        // Result classes for the service methods - move these to separate files or make them nested classes
        public class ExplorationResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public string CellName { get; set; } = string.Empty;
            public string CellDescription { get; set; } = string.Empty;
            public string MapName { get; set; } = string.Empty;
            public string TerrainType { get; set; } = string.Empty;

            public bool HasNPC { get; set; } = false;
            public NPCBriefInfo? NPCInfo { get; set; }

            public bool HasPointOfInterest { get; set; } = false;
            public POIBriefInfo? PointOfInterestInfo { get; set; }

            public bool HasStructure { get; set; } = false;
            public StructureBriefInfo? StructureInfo { get; set; }
        }

        public class InteractionResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public List<string> ItemsGained { get; set; } = new();
            public List<string> ItemsLost { get; set; } = new();
            public List<string> QuestsStarted { get; set; } = new();
        }

        public class NPCBriefInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class POIBriefInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class StructureBriefInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }
    }
}