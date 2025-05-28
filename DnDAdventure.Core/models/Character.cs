// DnDAdventure.Core/Models/Character.cs
namespace DnDAdventure.Core.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string? Subrace { get; set; }
        public string Class { get; set; } = string.Empty;
        public string? Subclass { get; set; }
        public string Background { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public Dictionary<string, int> Attributes { get; set; } = new();
        public Dictionary<string, int> BaseAttributes { get; set; } = new();
        public Dictionary<string, int> RacialBonuses { get; set; } = new();
        public List<string> RacialTraits { get; set; } = new();
        public List<string> Inventory { get; set; } = new();
        public int HealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }
        public List<string> Cantrips { get; set; } = new();
        public List<string> Spells { get; set; } = new();
        public string? SpellcastingAbility { get; set; }
        public int ArmorClass { get; set; } = 10;
        public int Speed { get; set; } = 30;
        public List<string> Languages { get; set; } = new();
        public List<string> Proficiencies { get; set; } = new();
    }
}
