// DnDAdventure.Core/models/NPC.cs
namespace DnDAdventure.Core
{
    public class NPC
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsTalkative { get; set; }
        public bool HasQuestsAvailable { get; set; }
        public bool HasItemsForSale { get; set; }
    }
}