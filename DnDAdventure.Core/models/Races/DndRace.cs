// DnDAdventure.Core/Models/Races/DndRace.cs
namespace DnDAdventure.Core.Models.Races
{
    public class DndRace
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, int> AbilityScoreIncrease { get; set; } = new();
        public int Speed { get; set; } = 30; // In feet
        public List<string> Languages { get; set; } = new();
        public List<RacialTrait> Traits { get; set; } = new();
        public List<Subrace> Subraces { get; set; } = new();
        public string Source { get; set; } = "Player's Handbook";
    }

    public class Subrace
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, int> AbilityScoreIncrease { get; set; } = new();
        public List<RacialTrait> Traits { get; set; } = new();
        public string Source { get; set; } = "Player's Handbook";
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
        public List<string> BestFor { get; set; } = new();
        public string PlayStyle { get; set; } = string.Empty;
    }

    public class RacialTrait
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
