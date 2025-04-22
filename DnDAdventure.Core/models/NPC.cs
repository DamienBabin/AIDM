using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Models
{
    public class NPC
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public int Disposition { get; set; } = 50;
        public List<string> Dialogs { get; set; } = new();
        public List<Guid> AvailableQuestIds { get; set; } = new();
        public List<InventoryItem> Inventory { get; set; } = new();
    }

    public class InventoryItem
    {
        public string Name { get; set; } = string.Empty;
        public bool IsTradeable { get; set; }
        public int Value { get; set; }
    }
}