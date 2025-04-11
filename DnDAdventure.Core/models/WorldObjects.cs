// DnDAdventure.Core/models/WorldObjects.cs
using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Models
{
    public class PointOfInterest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public POIType Type { get; set; }
        public List<POIAction> AvailableActions { get; set; } = new();
    }

    public enum POIType
    {
        Landmark,
        Treasure,
        Puzzle,
        Trap,
        Portal,
        Quest
    }

    public class POIAction
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, string> Requirements { get; set; } = new();
        public Dictionary<string, string> Effects { get; set; } = new();
    }

    public class Structure
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StructureType Type { get; set; }
    }

    public enum StructureType
    {
        Building,
        Ruin,
        Monument,
        Bridge,
        Gate,
        Altar
    }
}