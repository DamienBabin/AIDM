// DnDAdventure.Core/models/DndClassesAndRaces.cs
using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Models
{
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
    }

    public class ClassFeature
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LevelUnlocked { get; set; } = 1;
    }

    public class DndRace
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, int> AbilityScoreIncrease { get; set; } = new();
        public int Speed { get; set; } = 30; // In feet
        public List<string> Languages { get; set; } = new();
        public List<RacialTrait> Traits { get; set; } = new();
        public List<string> Subraces { get; set; } = new();
    }

    public class RacialTrait
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DndClassesAndRaces
    {
        public List<DndClass> Classes { get; set; } = new()
        {
            new DndClass
            {
                Name = "Barbarian",
                Description = "A fierce warrior who can enter a battle rage",
                HitDie = 12,
                PrimaryAbilities = new List<string> { "Strength" },
                SavingThrowProficiencies = new List<string> { "Strength", "Constitution" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Greataxe", "Two handaxes", "Explorer's pack", "Four javelins" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature
                    {
                        Name = "Rage",
                        Description = "In battle, you fight with primal ferocity. On your turn, you can enter a rage as a bonus action.",
                        LevelUnlocked = 1
                    },
                    new ClassFeature
                    {
                        Name = "Unarmored Defense",
                        Description = "While you are not wearing any armor, your AC equals 10 + your Dexterity modifier + your Constitution modifier.",
                        LevelUnlocked = 1
                    }
                }
            },
            new DndClass
            {
                Name = "Bard",
                Description = "An inspiring magician whose power echoes the music of creation",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Charisma" },
                SavingThrowProficiencies = new List<string> { "Dexterity", "Charisma" },
                ArmorProficiencies = new List<string> { "Light Armor" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Hand Crossbows", "Longswords", "Rapiers", "Shortswords" },
                StartingEquipment = new List<string> { "Rapier", "Diplomat's pack", "Lute", "Leather armor", "Dagger" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature
                    {
                        Name = "Bardic Inspiration",
                        Description = "You can inspire others through stirring words or music. To do so, you use a bonus action on your turn to choose one creature other than yourself within 60 feet of you who can hear you.",
                        LevelUnlocked = 1
                    },
                    new ClassFeature
                    {
                        Name = "Spellcasting",
                        Description = "You have learned to untangle and reshape the fabric of reality in harmony with your wishes and music.",
                        LevelUnlocked = 1
                    }
                }
            },
            new DndClass
            {
                Name = "Cleric",
                Description = "A priestly champion who wields divine magic in service of a higher power",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Wisdom" },
                SavingThrowProficiencies = new List<string> { "Wisdom", "Charisma" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons" },
                StartingEquipment = new List<string> { "Mace", "Scale mail", "Light crossbow and 20 bolts", "Priest's pack", "Shield", "Holy symbol" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature
                    {
                        Name = "Spellcasting",
                        Description = "As a conduit for divine power, you can cast cleric spells.",
                        LevelUnlocked = 1
                    },
                    new ClassFeature
                    {
                        Name = "Divine Domain",
                        Description = "Choose one domain related to your deity. Your domain grants you domain spells and other features.",
                        LevelUnlocked = 1
                    }
                }
            },
            new DndClass
            {
                Name = "Wizard",
                Description = "A scholarly magic-user capable of manipulating the structures of reality",
                HitDie = 6,
                PrimaryAbilities = new List<string> { "Intelligence" },
                SavingThrowProficiencies = new List<string> { "Intelligence", "Wisdom" },
                ArmorProficiencies = new List<string> { },
                WeaponProficiencies = new List<string> { "Daggers", "Darts", "Slings", "Quarterstaffs", "Light crossbows" },
                StartingEquipment = new List<string> { "Spellbook", "Arcane focus", "Scholar's pack", "Dagger" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature
                    {
                        Name = "Spellcasting",
                        Description = "As a student of arcane magic, you have a spellbook containing spells that show the first glimmerings of your true power.",
                        LevelUnlocked = 1
                    },
                    new ClassFeature
                    {
                        Name = "Arcane Recovery",
                        Description = "You have learned to regain some of your magical energy by studying your spellbook.",
                        LevelUnlocked = 1
                    }
                }
            },
            new DndClass
            {
                Name = "Rogue",
                Description = "A scoundrel who uses stealth and trickery to overcome obstacles and enemies",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Dexterity" },
                SavingThrowProficiencies = new List<string> { "Dexterity", "Intelligence" },
                ArmorProficiencies = new List<string> { "Light Armor" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Hand Crossbows", "Longswords", "Rapiers", "Shortswords" },
                StartingEquipment = new List<string> { "Rapier", "Shortbow and quiver of 20 arrows", "Burglar's pack", "Leather armor", "Two daggers", "Thieves' tools" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature
                    {
                        Name = "Sneak Attack",
                        Description = "You know how to strike subtly and exploit a foe's distraction.",
                        LevelUnlocked = 1
                    },
                    new ClassFeature
                    {
                        Name = "Thieves' Cant",
                        Description = "During your rogue training you learned thieves' cant, a secret mix of dialect, jargon, and code that allows you to hide messages in seemingly normal conversation.",
                        LevelUnlocked = 1
                    }
                }
            }
        };

        public List<DndRace> Races { get; set; } = new()
        {
            new DndRace
            {
                Name = "Human",
                Description = "Humans are the most adaptable and ambitious people among the common races.",
                AbilityScoreIncrease = new Dictionary<string, int>
                {
                    { "Strength", 1 },
                    { "Dexterity", 1 },
                    { "Constitution", 1 },
                    { "Intelligence", 1 },
                    { "Wisdom", 1 },
                    { "Charisma", 1 }
                },
                Speed = 30,
                Languages = new List<string> { "Common", "One additional language of your choice" },
                Traits = new List<RacialTrait>()
            },
            new DndRace
            {
                Name = "Elf",
                Description = "Elves are a magical people of otherworldly grace, living in places of ethereal beauty.",
                AbilityScoreIncrease = new Dictionary<string, int>
                {
                    { "Dexterity", 2 }
                },
                Speed = 30,
                Languages = new List<string> { "Common", "Elvish" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait
                    {
                        Name = "Darkvision",
                        Description = "You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light."
                    },
                    new RacialTrait
                    {
                        Name = "Keen Senses",
                        Description = "You have proficiency in the Perception skill."
                    },
                    new RacialTrait
                    {
                        Name = "Fey Ancestry",
                        Description = "You have advantage on saving throws against being charmed, and magic can't put you to sleep."
                    },
                    new RacialTrait
                    {
                        Name = "Trance",
                        Description = "Elves don't need to sleep. Instead, they meditate deeply for 4 hours a day."
                    }
                },
                Subraces = new List<string> { "High Elf", "Wood Elf", "Dark Elf (Drow)" }
            },
            new DndRace
            {
                Name = "Dwarf",
                Description = "Bold and hardy, dwarves are known as skilled warriors, miners, and workers of stone and metal.",
                AbilityScoreIncrease = new Dictionary<string, int>
                {
                    { "Constitution", 2 }
                },
                Speed = 25,
                Languages = new List<string> { "Common", "Dwarvish" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait
                    {
                        Name = "Darkvision",
                        Description = "You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light."
                    },
                    new RacialTrait
                    {
                        Name = "Dwarven Resilience",
                        Description = "You have advantage on saving throws against poison, and you have resistance against poison damage."
                    },
                    new RacialTrait
                    {
                        Name = "Stonecunning",
                        Description = "Whenever you make an Intelligence (History) check related to the origin of stonework, you are considered proficient in the History skill."
                    }
                },
                Subraces = new List<string> { "Hill Dwarf", "Mountain Dwarf" }
            },
            new DndRace
            {
                Name = "Halfling",
                Description = "The diminutive halflings survive in a world full of larger creatures by avoiding notice or, barring that, avoiding offense.",
                AbilityScoreIncrease = new Dictionary<string, int>
                {
                    { "Dexterity", 2 }
                },
                Speed = 25,
                Languages = new List<string> { "Common", "Halfling" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait
                    {
                        Name = "Lucky",
                        Description = "When you roll a 1 on an attack roll, ability check, or saving throw, you can reroll the die and must use the new roll."
                    },
                    new RacialTrait
                    {
                        Name = "Brave",
                        Description = "You have advantage on saving throws against being frightened."
                    },
                    new RacialTrait
                    {
                        Name = "Halfling Nimbleness",
                        Description = "You can move through the space of any creature that is of a size larger than yours."
                    }
                },
                Subraces = new List<string> { "Lightfoot Halfling", "Stout Halfling" }
            },
            new DndRace
            {
                Name = "Dragonborn",
                Description = "Born of dragons, as their name proclaims, the dragonborn walk proudly through a world that greets them with fearful incomprehension.",
                AbilityScoreIncrease = new Dictionary<string, int>
                {
                    { "Strength", 2 },
                    { "Charisma", 1 }
                },
                Speed = 30,
                Languages = new List<string> { "Common", "Draconic" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait
                    {
                        Name = "Draconic Ancestry",
                        Description = "You have draconic ancestry. Choose one type of dragon from the Draconic Ancestry table. Your breath weapon and damage resistance are determined by the dragon type."
                    },
                    new RacialTrait
                    {
                        Name = "Breath Weapon",
                        Description = "You can use your action to exhale destructive energy. Your draconic ancestry determines the size, shape, and damage type of the exhalation."
                    },
                    new RacialTrait
                    {
                        Name = "Damage Resistance",
                        Description = "You have resistance to the damage type associated with your draconic ancestry."
                    }
                }
            }
        };
    }
}

