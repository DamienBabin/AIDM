// DnDAdventure.Core/Models/ExplorationResult.cs
namespace DnDAdventure.Core.Models
{
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