// DnDAdventure.Core/models/WorldMap.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DnDAdventure.Core.Models
{
    /// <summary>
    /// Represents a 9x9 grid map within the game world
    /// </summary>
    public class WorldMap
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // The actual 9x9 grid of map cells - using jagged array for JSON serialization
        public MapCell[][] Grid { get; set; } = new MapCell[9][];
        
        // Coordinates within the larger world (for connecting multiple maps)
        public int WorldX { get; set; }
        public int WorldY { get; set; }
        
        // Reference to connected maps (North, East, South, West)
        public Dictionary<Direction, Guid> ConnectedMaps { get; set; } = new();
        
        /// <summary>
        /// Initial map setup
        /// </summary>
        public WorldMap()
        {
            // Initialize all grid cells
            for (int x = 0; x < 9; x++)
            {
                Grid[x] = new MapCell[9];
                for (int y = 0; y < 9; y++)
                {
                    Grid[x][y] = new MapCell
                    {
                        X = x,
                        Y = y,
                        TerrainType = TerrainType.Plains, // Default terrain
                        Passable = true
                    };
                }
            }
        }
        
        /// <summary>
        /// Gets a cell at the specified coordinates
        /// </summary>
        public MapCell GetCell(int x, int y)
        {
            if (x < 0 || x >= 9 || y < 0 || y >= 9)
                throw new ArgumentOutOfRangeException("Coordinates must be within the 9x9 grid");
                
            return Grid[x][y];
        }
        
        /// <summary>
        /// Sets a cell at the specified coordinates
        /// </summary>
        public void SetCell(int x, int y, MapCell cell)
        {
            if (x < 0 || x >= 9 || y < 0 || y >= 9)
                throw new ArgumentOutOfRangeException("Coordinates must be within the 9x9 grid");
                
            cell.X = x;
            cell.Y = y;
            Grid[x][y] = cell;
        }
        
        /// <summary>
        /// Updates an existing cell at the specified coordinates
        /// </summary>
        public void UpdateCell(int x, int y, Action<MapCell> updateAction)
        {
            if (x < 0 || x >= 9 || y < 0 || y >= 9)
                throw new ArgumentOutOfRangeException("Coordinates must be within the 9x9 grid");
                
            updateAction(Grid[x][y]);
        }
        
        /// <summary>
        /// Gets a text representation of the map for display
        /// </summary>
        public string GetMapDisplay()
        {
            var sb = new StringBuilder();
            
            // Add legend
            sb.AppendLine("Legend:");
            sb.AppendLine("# - Wall/Obstacle");
            sb.AppendLine("~ - Water");
            sb.AppendLine("^ - Mountain");
            sb.AppendLine("T - Tree/Forest");
            sb.AppendLine("B - Building/Structure");
            sb.AppendLine("@ - NPC");
            sb.AppendLine("! - Point of Interest");
            sb.AppendLine("X - Player's Position");
            sb.AppendLine();
            
            // Column headers (0-8)
            sb.Append("  ");
            for (int x = 0; x < 9; x++)
            {
                sb.Append($" {x} ");
            }
            sb.AppendLine();
            
            // Draw the grid
            for (int y = 0; y < 9; y++)
            {
                sb.Append($"{y} "); // Row headers (0-8)
                
                for (int x = 0; x < 9; x++)
                {
                    var cell = Grid[x][y];
                    sb.Append($" {GetCellSymbol(cell)} ");
                }
                
                sb.AppendLine();
            }
            
            return sb.ToString();
        }
        
        /// <summary>
        /// Gets a symbol representing a cell's contents for the text-based map display
        /// </summary>
        private char GetCellSymbol(MapCell cell)
        {
            // Player position takes precedence
            if (cell.HasPlayer)
                return 'X';
                
            // Then check for NPCs and POIs
            if (cell.NPCId != Guid.Empty)
                return '@';
                
            if (cell.PointOfInterestId != Guid.Empty)
                return '!';
                
            // Then check for terrain and structures
            switch (cell.TerrainType)
            {
                case TerrainType.Water:
                    return '~';
                case TerrainType.Mountain:
                    return '^';
                case TerrainType.Forest:
                    return 'T';
                case TerrainType.Plains:
                    return '.';
                case TerrainType.Desert:
                    return ':';
                case TerrainType.Swamp:
                    return ',';
                default:
                    return '.';
            }
        }
    }
    
    /// <summary>
    /// Represents a single cell within a WorldMap grid
    /// </summary>
    public class MapCell
    {
        // Position within the grid (0-8 for both X and Y)
        public int X { get; set; }
        public int Y { get; set; }
        
        // Basic information
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TerrainType TerrainType { get; set; } = TerrainType.Plains;
        
        // Cell contents and references
        public bool HasPlayer { get; set; } = false;
        public Guid NPCId { get; set; } = Guid.Empty;
        public Guid PointOfInterestId { get; set; } = Guid.Empty;
        public Guid StructureId { get; set; } = Guid.Empty;
        
        // Movement properties
        public bool Passable { get; set; } = true;
        public int MovementCost { get; set; } = 1; // Default movement cost
        
        // Additional data
        public Dictionary<string, string> Properties { get; set; } = new();
        
        /// <summary>
        /// Provides a detailed explanation of what's in this cell
        /// </summary>
        public string GetCellExplanation()
        {
            var sb = new StringBuilder();
            
            // Add name if it exists
            if (!string.IsNullOrEmpty(Name))
                sb.AppendLine($"Location: {Name}");
                
            // Add terrain description
            sb.AppendLine($"Terrain: {TerrainType} {(Passable ? "(Passable)" : "(Impassable)")}");
            
            // Add description if it exists
            if (!string.IsNullOrEmpty(Description))
                sb.AppendLine(Description);
                
            // Note special contents
            if (HasPlayer)
                sb.AppendLine("You are currently here.");
                
            if (NPCId != Guid.Empty)
                sb.AppendLine("There is someone here you can interact with.");
                
            if (PointOfInterestId != Guid.Empty)
                sb.AppendLine("There is something interesting here to examine.");
                
            if (StructureId != Guid.Empty)
                sb.AppendLine("There is a structure here that you can enter or interact with.");
                
            // Add any special properties
            if (Properties.Count > 0)
            {
                sb.AppendLine("Special properties:");
                foreach (var property in Properties)
                {
                    sb.AppendLine($"- {property.Key}: {property.Value}");
                }
            }
            
            return sb.ToString();
        }
        
        /// <summary>
        /// Creates a link to another cell within the same map
        /// </summary>
        public void AddCellLink(string direction, int targetX, int targetY, string description = "")
        {
            Properties[$"Link_{direction}"] = $"{targetX},{targetY}";
            if (!string.IsNullOrEmpty(description))
                Properties[$"Link_{direction}_Description"] = description;
        }
        
        /// <summary>
        /// Creates a link to a cell in a different map
        /// </summary>
        public void AddMapLink(string direction, Guid targetMapId, int targetX, int targetY, string description = "")
        {
            Properties[$"MapLink_{direction}"] = $"{targetMapId}:{targetX},{targetY}";
            if (!string.IsNullOrEmpty(description))
                Properties[$"MapLink_{direction}_Description"] = description;
        }
    }
    
    /// <summary>
    /// Represents a point of interest on a map
    /// </summary>
    public class PointOfInterest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public POIType Type { get; set; } = POIType.Landmark;
        
        // Possible actions at this point of interest
        public List<POIAction> AvailableActions { get; set; } = new();
        
        // Requirements to interact with this POI
        public Dictionary<string, string> Requirements { get; set; } = new();
        
        // Additional data
        public Dictionary<string, string> Properties { get; set; } = new();
    }
    
    /// <summary>
    /// Represents a structure on a map (building, dungeon entrance, etc.)
    /// </summary>
    public class MapStructure
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StructureType Type { get; set; } = StructureType.Building;
        
        // If this structure links to another map (like a dungeon)
        public Guid? LinkedMapId { get; set; }
        public int? EntryPointX { get; set; }
        public int? EntryPointY { get; set; }
        
        // Possible interactions with this structure
        public List<string> AvailableInteractions { get; set; } = new();
        
        // NPCs associated with this structure
        public List<Guid> ResidentNPCIds { get; set; } = new();
        
        // Additional data
        public Dictionary<string, string> Properties { get; set; } = new();
    }
    
    /// <summary>
    /// Action available at a point of interest
    /// </summary>
    public class POIAction
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Effects when this action is taken
        public Dictionary<string, string> Effects { get; set; } = new();
        
        // Requirements to perform this action
        public Dictionary<string, string> Requirements { get; set; } = new();
    }
    
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
    
    public enum TerrainType
    {
        Plains,
        Forest,
        Mountain,
        Water,
        Desert,
        Swamp,
        Road,
        Cave
    }
    
    public enum POIType
    {
        Landmark,
        Treasure,
        Puzzle,
        Quest,
        Resource,
        Encounter
    }
    
    public enum StructureType
    {
        Building,
        Temple,
        Shop,
        Tavern,
        Castle,
        Ruins,
        DungeonEntrance,
        Portal
    }
}
