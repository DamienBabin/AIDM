// DnDAdventure.Core/models/Map.cs
using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Models
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MapCell[,] Grid { get; set; } = new MapCell[9, 9];
        public Dictionary<Direction, Guid> ConnectedMaps { get; set; } = new();

        public string GetMapDisplay()
        {
            // Simple ASCII representation of the map
            var result = "";
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    var cell = Grid[x, y];
                    if (cell.Passable)
                        result += cell.PointOfInterestId != Guid.Empty ? "P " : 
                                 cell.NPCId != Guid.Empty ? "N " : 
                                 cell.StructureId != Guid.Empty ? "S " : ". ";
                    else
                        result += "# ";
                }
                result += "\n";
            }
            return result;
        }
    }

    public class MapCell
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TerrainType TerrainType { get; set; }
        public bool Passable { get; set; } = true;
        public Guid NPCId { get; set; } = Guid.Empty;
        public Guid PointOfInterestId { get; set; } = Guid.Empty;
        public Guid StructureId { get; set; } = Guid.Empty;
    }

    public enum TerrainType
    {
        Plains,
        Forest,
        Mountain,
        Desert,
        Water,
        Road,
        Settlement,
        Cave,
        Dungeon
    }
}