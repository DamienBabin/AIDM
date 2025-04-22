// DnDAdventure.Core/Models/GameState.cs
namespace DnDAdventure.Core.Models
{
    public class GameState
    {
        public Guid Id { get; set; }
        public Guid CharacterId { get; set; }
        public Guid WorldId { get; set; } // Reference to the World this game state belongs to
        public string CurrentLocation { get; set; } = string.Empty;
        public List<string> CompletedQuests { get; set; } = new();
        public List<string> ActiveQuests { get; set; } = new();
        public int CurrentStoryNode { get; set; }
        public Dictionary<string, bool> Flags { get; set; } = new();
    }
}
