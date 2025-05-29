// DnDAdventure.Core/Models/Classes/DndClass.cs
namespace DnDAdventure.Core.Models.Classes
{
    public class SpellcastingInfo
    {
        public string SpellcastingAbility { get; set; } = string.Empty;
        public int CantripsKnown { get; set; }
        public int SpellsKnown { get; set; }
        public int SpellSlots { get; set; }
        public bool RitualCasting { get; set; }
        public string SpellcastingFocus { get; set; } = string.Empty;
        public bool Spellbook { get; set; } = false;
    }

    public class DndClass
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int HitDie { get; set; } // d6, d8, d10, d12 etc.
        public List<string> PrimaryAbilities { get; set; } = new();
        public List<string> SavingThrowProficiencies { get; set; } = new();
        public List<string> ArmorProficiencies { get; set; } = new();
        public List<string> WeaponProficiencies { get; set; } = new();
        public List<string> StartingEquipment { get; set; } = new();
        public List<ClassFeature> Features { get; set; } = new();
        public List<Subclass> Subclasses { get; set; } = new();
        public SpellcastingInfo? Spellcasting { get; set; }
    }

    public class Subclass
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ClassFeature> Features { get; set; } = new();
        public string Source { get; set; } = "Player's Handbook";
        public List<string> Pros { get; set; } = new();
        public List<string> Cons { get; set; } = new();
        public List<string> BestFor { get; set; } = new();
        public string PlayStyle { get; set; } = string.Empty;
        public SpellcastingInfo? Spellcasting { get; set; }
    }

    public class ClassFeature
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LevelUnlocked { get; set; } = 1;
    }
}
