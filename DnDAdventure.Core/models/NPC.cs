// DnDAdventure.Core/models/NPC.cs
using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Models
{
    public class NPC
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<NPCDialog> Dialogs { get; set; } = new();
        public List<Guid> AvailableQuestIds { get; set; } = new();
        public List<NPCInventoryItem> Inventory { get; set; } = new();
    }

    public class NPCDialog
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<NPCDialogResponse> PossibleResponses { get; set; } = new();
    }

    public class NPCDialogResponse
    {
        public string Text { get; set; } = string.Empty;
        public string NextDialogId { get; set; } = string.Empty;
        public Dictionary<string, string> Effects { get; set; } = new();
    }

    public class NPCInventoryItem
    {
        public string ItemId { get; set; } = string.Empty;
        public bool IsTradeable { get; set; }
        public int Price { get; set; }
    }
}
