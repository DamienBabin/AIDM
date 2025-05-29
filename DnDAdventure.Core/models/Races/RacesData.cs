// DnDAdventure.Core/Models/Races/RacesData.cs
using DnDAdventure.Core.Models.Races;

namespace DnDAdventure.Core.Models.Races
{
    public static class RacesData
    {
        public static List<DndRace> GetRaces()
        {
            return new List<DndRace>
            {
                new DndRace
                {
                    Name = "Human",
                    Description = "Humans are the most adaptable and ambitious people among the common races. They have widely varying tastes, morals, and customs in the many different lands where they have settled.",
                    AbilityScoreIncrease = new Dictionary<string, int> { { "All", 1 } },
                    Speed = 30,
                    Languages = new List<string> { "Common", "One extra language of your choice" },
                    Traits = new List<RacialTrait>
                    {
                        new RacialTrait { Name = "Extra Language", Description = "You can speak, read, and write one extra language of your choice." },
                        new RacialTrait { Name = "Extra Skill", Description = "You gain proficiency in one skill of your choice." }
                    },
                    Subraces = new List<Subrace>
                    {
                        new Subrace
                        {
                            Name = "Variant Human",
                            Description = "If your campaign uses the optional feat rules, your Dungeon Master might allow these variant traits, all of which replace the human's Ability Score Increase trait.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Choice1", 1 }, { "Choice2", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Skills", Description = "You gain proficiency in one skill of your choice." },
                                new RacialTrait { Name = "Feat", Description = "You gain one feat of your choice." }
                            },
                            Pros = new List<string>
                            {
                                "Free feat at level 1",
                                "Flexible ability score increases",
                                "Extra skill proficiency",
                                "Works with any class",
                                "Highly customizable"
                            },
                            Cons = new List<string>
                            {
                                "No unique racial abilities",
                                "Less flavorful than other races",
                                "Dependent on feat choice",
                                "Can feel generic"
                            },
                            BestFor = new List<string>
                            {
                                "Players who want mechanical optimization",
                                "Characters with specific feat requirements",
                                "Flexible character concepts",
                                "Min-maxing builds"
                            },
                            PlayStyle = "Highly adaptable character with early access to powerful feats"
                        }
                    }
                },
                new DndRace
                {
                    Name = "Elf",
                    Description = "Elves are a magical people of otherworldly grace, living in places of ethereal beauty, in the midst of ancient forests or in silvery spires glittering with faerie light.",
                    AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 2 } },
                    Speed = 30,
                    Languages = new List<string> { "Common", "Elvish" },
                    Traits = new List<RacialTrait>
                    {
                        new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light." },
                        new RacialTrait { Name = "Keen Senses", Description = "You have proficiency in the Perception skill." },
                        new RacialTrait { Name = "Fey Ancestry", Description = "You have advantage on saving throws against being charmed, and magic can't put you to sleep." },
                        new RacialTrait { Name = "Trance", Description = "Elves don't need to sleep. Instead, they meditate deeply, remaining semiconscious, for 4 hours a day." }
                    },
                    Subraces = new List<Subrace>
                    {
                        new Subrace
                        {
                            Name = "High Elf",
                            Description = "High elves are graceful warriors and wizards who originated from the realm of Faerie.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Intelligence", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Elf Weapon Training", Description = "You have proficiency with longswords, shortbows, longbows, and shortbows." },
                                new RacialTrait { Name = "Cantrip", Description = "You know one cantrip of your choice from the wizard spell list." },
                                new RacialTrait { Name = "Extra Language", Description = "You can speak, read, and write one extra language of your choice." }
                            },
                            Pros = new List<string>
                            {
                                "Free wizard cantrip",
                                "Weapon proficiencies",
                                "Extra language",
                                "Intelligence bonus for wizards",
                                "Good for gish builds"
                            },
                            Cons = new List<string>
                            {
                                "Intelligence bonus not useful for all classes",
                                "Weapon training may be redundant",
                                "Less specialized than other subraces"
                            },
                            BestFor = new List<string>
                            {
                                "Wizard characters",
                                "Eldritch Knights",
                                "Arcane Tricksters",
                                "Characters wanting magic and martial skills"
                            },
                            PlayStyle = "Scholarly warrior-mage with arcane knowledge and martial training"
                        },
                        new Subrace
                        {
                            Name = "Wood Elf",
                            Description = "Wood elves have keen senses and intuition, and their fleet feet carry them quickly and stealthily through their native forests.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Wisdom", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Elf Weapon Training", Description = "You have proficiency with longswords, shortbows, longbows, and shortbows." },
                                new RacialTrait { Name = "Fleet of Foot", Description = "Your base walking speed increases to 35 feet." },
                                new RacialTrait { Name = "Mask of the Wild", Description = "You can attempt to hide even when you are only lightly obscured by foliage, heavy rain, falling snow, mist, and other natural phenomena." }
                            },
                            Pros = new List<string>
                            {
                                "Increased movement speed",
                                "Mask of the Wild for stealth",
                                "Weapon proficiencies",
                                "Wisdom bonus for druids/rangers",
                                "Excellent for nature characters"
                            },
                            Cons = new List<string>
                            {
                                "Stealth ability is situational",
                                "Wisdom bonus not useful for all classes",
                                "Less versatile than High Elf"
                            },
                            BestFor = new List<string>
                            {
                                "Rangers and druids",
                                "Stealth-focused characters",
                                "Nature-themed campaigns",
                                "Characters who value mobility"
                            },
                            PlayStyle = "Swift forest guardian with natural stealth and archery skills"
                        },
                        new Subrace
                        {
                            Name = "Dark Elf (Drow)",
                            Description = "Descended from an earlier subrace of dark-skinned elves, the drow were banished from the surface world for following the goddess Lolth down the path to evil and corruption.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Charisma", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Superior Darkvision", Description = "Your darkvision has a radius of 120 feet." },
                                new RacialTrait { Name = "Sunlight Sensitivity", Description = "You have disadvantage on attack rolls and on Wisdom (Perception) checks that rely on sight when you, the target of your attack, or whatever you are trying to perceive is in direct sunlight." },
                                new RacialTrait { Name = "Drow Magic", Description = "You know the dancing lights cantrip. When you reach 3rd level, you can cast the faerie fire spell once per day. When you reach 5th level, you can also cast the darkness spell once per day." },
                                new RacialTrait { Name = "Drow Weapon Training", Description = "You have proficiency with rapiers, shortswords, and hand crossbows." }
                            },
                            Pros = new List<string>
                            {
                                "Superior darkvision (120 feet)",
                                "Innate spellcasting abilities",
                                "Charisma bonus for social classes",
                                "Unique weapon proficiencies",
                                "Distinctive roleplay opportunities"
                            },
                            Cons = new List<string>
                            {
                                "Sunlight sensitivity is major drawback",
                                "Social stigma in most settings",
                                "Limited by sunlight in outdoor campaigns",
                                "Spells are once per day only"
                            },
                            BestFor = new List<string>
                            {
                                "Underdark campaigns",
                                "Charisma-based casters",
                                "Characters with dark backgrounds",
                                "Players who like challenging roleplay"
                            },
                            PlayStyle = "Mysterious underground dweller with innate magic and social complexity"
                        }
                    }
                },
                new DndRace
                {
                    Name = "Dwarf",
                    Description = "Bold and hardy, dwarves are known as skilled warriors, miners, and workers of stone and metal.",
                    AbilityScoreIncrease = new Dictionary<string, int> { { "Constitution", 2 } },
                    Speed = 25,
                    Languages = new List<string> { "Common", "Dwarvish" },
                    Traits = new List<RacialTrait>
                    {
                        new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light." },
                        new RacialTrait { Name = "Dwarven Resilience", Description = "You have advantage on saving throws against poison, and you have resistance against poison damage." },
                        new RacialTrait { Name = "Dwarven Combat Training", Description = "You have proficiency with the battleaxe, handaxe, light hammer, and warhammer." },
                        new RacialTrait { Name = "Stonecunning", Description = "Whenever you make an Intelligence (History) check related to the origin of stonework, you are considered proficient in the History skill and add double your proficiency bonus to the check." }
                    },
                    Subraces = new List<Subrace>
                    {
                        new Subrace
                        {
                            Name = "Mountain Dwarf",
                            Description = "Mountain dwarves are strong and hardy, accustomed to a difficult life in rugged terrain.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 2 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Armor Proficiency", Description = "You have proficiency with light and medium armor." }
                            },
                            Pros = new List<string>
                            {
                                "Strength and Constitution bonuses",
                                "Armor proficiency for all classes",
                                "Excellent for martial characters",
                                "Dwarven weapon training",
                                "Poison resistance"
                            },
                            Cons = new List<string>
                            {
                                "Slower movement speed",
                                "Strength bonus not useful for all classes",
                                "Limited to martial concepts",
                                "No magical abilities"
                            },
                            BestFor = new List<string>
                            {
                                "Fighters and paladins",
                                "Strength-based characters",
                                "Frontline combatants",
                                "Characters needing durability"
                            },
                            PlayStyle = "Tough mountain warrior with natural armor training and weapon expertise"
                        },
                        new Subrace
                        {
                            Name = "Hill Dwarf",
                            Description = "Hill dwarves are known for their keen senses, deep intuition, and remarkable resilience.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Wisdom", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Dwarven Toughness", Description = "Your hit point maximum increases by 1, and it increases by 1 every time you gain a level." }
                            },
                            Pros = new List<string>
                            {
                                "Extra hit points every level",
                                "Wisdom bonus for clerics/druids",
                                "Excellent survivability",
                                "Poison resistance",
                                "Good for any class"
                            },
                            Cons = new List<string>
                            {
                                "Slower movement speed",
                                "Less specialized than Mountain Dwarf",
                                "Wisdom bonus not useful for all classes",
                                "No armor proficiency"
                            },
                            BestFor = new List<string>
                            {
                                "Clerics and druids",
                                "Any character wanting extra HP",
                                "Wisdom-based characters",
                                "Survivability-focused builds"
                            },
                            PlayStyle = "Hardy hill dweller with exceptional toughness and natural wisdom"
                        }
                    }
                },
                new DndRace
                {
                    Name = "Halfling",
                    Description = "The diminutive halflings survive in a world full of larger creatures by avoiding notice or, barring that, avoiding offense.",
                    AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 2 } },
                    Speed = 25,
                    Languages = new List<string> { "Common", "Halfling" },
                    Traits = new List<RacialTrait>
                    {
                        new RacialTrait { Name = "Lucky", Description = "When you roll a 1 on the d20 for an attack roll, ability check, or saving throw, you can reroll the die and must use the new roll." },
                        new RacialTrait { Name = "Brave", Description = "You have advantage on saving throws against being frightened." },
                        new RacialTrait { Name = "Halfling Nimbleness", Description = "You can move through the space of any creature that is of a size larger than yours." }
                    },
                    Subraces = new List<Subrace>
                    {
                        new Subrace
                        {
                            Name = "Lightfoot Halfling",
                            Description = "Lightfoot halflings can easily hide from notice, even using other people as cover.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Charisma", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Naturally Stealthy", Description = "You can attempt to hide even when you are obscured only by a creature that is at least one size larger than you." }
                            },
                            Pros = new List<string>
                            {
                                "Lucky trait rerolls natural 1s",
                                "Naturally Stealthy for hiding",
                                "Charisma bonus for social classes",
                                "Brave prevents fear effects",
                                "Excellent for rogues and bards"
                            },
                            Cons = new List<string>
                            {
                                "Slower movement speed",
                                "Small size limits weapon options",
                                "Charisma bonus not useful for all classes",
                                "Less durable than larger races"
                            },
                            BestFor = new List<string>
                            {
                                "Rogues and bards",
                                "Stealth-focused characters",
                                "Social characters",
                                "Characters wanting luck mechanics"
                            },
                            PlayStyle = "Nimble social character who uses stealth and luck to overcome challenges"
                        },
                        new Subrace
                        {
                            Name = "Stout Halfling",
                            Description = "Stout halflings are hardier than average and have some resistance to poison.",
                            AbilityScoreIncrease = new Dictionary<string, int> { { "Constitution", 1 } },
                            Traits = new List<RacialTrait>
                            {
                                new RacialTrait { Name = "Stout Resilience", Description = "You have advantage on saving throws against poison, and you have resistance against poison damage." }
                            },
                            Pros = new List<string>
                            {
                                "Lucky trait rerolls natural 1s",
                                "Poison resistance like dwarves",
                                "Constitution bonus for survivability",
                                "Brave prevents fear effects",
                                "Good for any class"
                            },
                            Cons = new List<string>
                            {
                                "Slower movement speed",
                                "Small size limits weapon options",
                                "Less specialized abilities",
                                "Constitution bonus modest"
                            },
                            BestFor = new List<string>
                            {
                                "Any class wanting survivability",
                                "Characters facing poison threats",
                                "Durable small characters",
                                "Players who like luck mechanics"
                            },
                            PlayStyle = "Tough little survivor who relies on luck and resilience"
                        }
                    }
                }
            };
        }
    }
}
