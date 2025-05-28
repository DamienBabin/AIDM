// DnDAdventure.Core/Models/Spells.cs
namespace DnDAdventure.Core.Models
{
    public class Spell
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; } // 0 for cantrips
        public string School { get; set; } = string.Empty;
        public string CastingTime { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string Components { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public List<string> Classes { get; set; } = new(); // Which classes can learn this spell
        public string Source { get; set; } = "Player's Handbook";
    }

    public class SpellcastingInfo
    {
        public string SpellcastingAbility { get; set; } = string.Empty; // "Intelligence", "Wisdom", "Charisma"
        public int CantripsKnown { get; set; } = 0;
        public int SpellsKnown { get; set; } = 0;
        public int SpellSlots { get; set; } = 0;
        public bool RitualCasting { get; set; } = false;
        public bool Spellbook { get; set; } = false; // For wizards
        public string SpellcastingFocus { get; set; } = string.Empty;
    }

    public class SpellList
    {
        public List<Spell> Spells { get; set; } = new()
        {
            // Cantrips (Level 0)
            new Spell
            {
                Name = "Acid Splash",
                Description = "You hurl a bubble of acid. Choose one or two creatures within range. A creature must succeed on a Dexterity saving throw or take 1d6 acid damage.",
                Level = 0,
                School = "Conjuration",
                CastingTime = "1 action",
                Range = "60 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Blade Ward",
                Description = "You extend your hand and trace a sigil of warding in the air. Until the end of your next turn, you have resistance against bludgeoning, piercing, and slashing damage dealt by weapon attacks.",
                Level = 0,
                School = "Abjuration",
                CastingTime = "1 action",
                Range = "Self",
                Components = "V, S",
                Duration = "1 round",
                Classes = new List<string> { "Bard", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Chill Touch",
                Description = "You create a ghostly, skeletal hand in the space of a creature within range. Make a ranged spell attack against the creature to assail it with the chill of the grave. On a hit, the target takes 1d8 necrotic damage.",
                Level = 0,
                School = "Necromancy",
                CastingTime = "1 action",
                Range = "120 feet",
                Components = "V, S",
                Duration = "1 round",
                Classes = new List<string> { "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Dancing Lights",
                Description = "You create up to four torch-sized lights within range, making them appear as torches, lanterns, or glowing orbs that hover in the air for the duration.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "120 feet",
                Components = "V, S, M (a bit of phosphorus or wychwood, or a glowworm)",
                Duration = "Concentration, up to 1 minute",
                Classes = new List<string> { "Bard", "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Druidcraft",
                Description = "Whispering to the spirits of nature, you create one of the following effects within range: You create a tiny, harmless sensory effect that predicts what the weather will be at your location for the next 24 hours.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Druid" }
            },
            new Spell
            {
                Name = "Eldritch Blast",
                Description = "A beam of crackling energy streaks toward a creature within range. Make a ranged spell attack against the target. On a hit, the target takes 1d10 force damage.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "120 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Warlock" }
            },
            new Spell
            {
                Name = "Fire Bolt",
                Description = "You hurl a mote of fire at a creature or object within range. Make a ranged spell attack against the target. On a hit, the target takes 1d10 fire damage.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "120 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Guidance",
                Description = "You touch one willing creature. Once before the spell ends, the target can roll a d4 and add the number rolled to one ability check of its choice.",
                Level = 0,
                School = "Divination",
                CastingTime = "1 action",
                Range = "Touch",
                Components = "V, S",
                Duration = "Concentration, up to 1 minute",
                Classes = new List<string> { "Cleric", "Druid" }
            },
            new Spell
            {
                Name = "Light",
                Description = "You touch one object that is no larger than 10 feet in any dimension. Until the spell ends, the object sheds bright light in a 20-foot radius and dim light for an additional 20 feet.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "Touch",
                Components = "V, M (a firefly or phosphorescent moss)",
                Duration = "1 hour",
                Classes = new List<string> { "Bard", "Cleric", "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Mage Hand",
                Description = "A spectral, floating hand appears at a point you choose within range. The hand lasts for the duration or until you dismiss it as an action.",
                Level = 0,
                School = "Conjuration",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "V, S",
                Duration = "1 minute",
                Classes = new List<string> { "Bard", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Mending",
                Description = "This spell repairs a single break or tear in an object you touch, such as a broken chain link, two halves of a broken key, a torn cloak, or a leaking wineskin.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 minute",
                Range = "Touch",
                Components = "V, S, M (two lodestones)",
                Duration = "Instantaneous",
                Classes = new List<string> { "Bard", "Cleric", "Druid", "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Minor Illusion",
                Description = "You create a sound or an image of an object within range that lasts for the duration. The illusion also ends if you dismiss it as an action or cast this spell again.",
                Level = 0,
                School = "Illusion",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "S, M (a bit of fleece)",
                Duration = "1 minute",
                Classes = new List<string> { "Bard", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Poison Spray",
                Description = "You extend your hand toward a creature you can see within range and project a puff of noxious gas from your palm. The creature must succeed on a Constitution saving throw or take 1d12 poison damage.",
                Level = 0,
                School = "Conjuration",
                CastingTime = "1 action",
                Range = "10 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Druid", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Prestidigitation",
                Description = "This spell is a minor magical trick that novice spellcasters use for practice. You create one of the following magical effects within range.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 action",
                Range = "10 feet",
                Components = "V, S",
                Duration = "Up to 1 hour",
                Classes = new List<string> { "Bard", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Produce Flame",
                Description = "A flickering flame appears in your hand. The flame remains there for the duration and harms neither you nor your equipment.",
                Level = 0,
                School = "Conjuration",
                CastingTime = "1 action",
                Range = "Self",
                Components = "V, S",
                Duration = "10 minutes",
                Classes = new List<string> { "Druid" }
            },
            new Spell
            {
                Name = "Ray of Frost",
                Description = "A frigid beam of blue-white light streaks toward a creature within range. Make a ranged spell attack against the target. On a hit, it takes 1d8 cold damage, and its speed is reduced by 10 feet until the start of your next turn.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "60 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Resistance",
                Description = "You touch one willing creature. Once before the spell ends, the target can roll a d4 and add the number rolled to one saving throw of its choice.",
                Level = 0,
                School = "Abjuration",
                CastingTime = "1 action",
                Range = "Touch",
                Components = "V, S, M (a miniature cloak)",
                Duration = "Concentration, up to 1 minute",
                Classes = new List<string> { "Cleric", "Druid" }
            },
            new Spell
            {
                Name = "Sacred Flame",
                Description = "Flame-like radiance descends on a creature that you can see within range. The target must succeed on a Dexterity saving throw or take 1d8 radiant damage.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "60 feet",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Cleric" }
            },
            new Spell
            {
                Name = "Shillelagh",
                Description = "The wood of a club or quarterstaff you are holding is imbued with nature's power. For the duration, you can use your spellcasting ability instead of Strength for the attack and damage rolls of melee attacks using that weapon.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 bonus action",
                Range = "Touch",
                Components = "V, S, M (mistletoe, a shamrock leaf, and a club or quarterstaff)",
                Duration = "1 minute",
                Classes = new List<string> { "Druid" }
            },
            new Spell
            {
                Name = "Shocking Grasp",
                Description = "Lightning springs from your hand to deliver a shock to a creature you try to touch. Make a melee spell attack against the target. You have advantage on the attack roll if the target is wearing armor made of metal.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "Touch",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Sorcerer", "Wizard" }
            },
            new Spell
            {
                Name = "Spare the Dying",
                Description = "You touch a living creature that has 0 hit points. The creature becomes stable. This spell has no effect on undead or constructs.",
                Level = 0,
                School = "Necromancy",
                CastingTime = "1 action",
                Range = "Touch",
                Components = "V, S",
                Duration = "Instantaneous",
                Classes = new List<string> { "Cleric" }
            },
            new Spell
            {
                Name = "Thaumaturgy",
                Description = "You manifest a minor wonder, a sign of supernatural power, within range. You create one of the following magical effects within range.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "V",
                Duration = "Up to 1 minute",
                Classes = new List<string> { "Cleric" }
            },
            new Spell
            {
                Name = "Thorn Whip",
                Description = "You create a long, vine-like whip covered in thorns that lashes out at your command toward a creature in range. Make a melee spell attack against the target.",
                Level = 0,
                School = "Transmutation",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "V, S, M (the stem of a plant with thorns)",
                Duration = "Instantaneous",
                Classes = new List<string> { "Druid" }
            },
            new Spell
            {
                Name = "True Strike",
                Description = "You extend your hand and point a finger at a target in range. Your magic grants you a brief insight into the target's defenses.",
                Level = 0,
                School = "Divination",
                CastingTime = "1 action",
                Range = "30 feet",
                Components = "S",
                Duration = "Concentration, up to 1 round",
                Classes = new List<string> { "Bard", "Sorcerer", "Warlock", "Wizard" }
            },
            new Spell
            {
                Name = "Vicious Mockery",
                Description = "You unleash a string of insults laced with subtle enchantments at a creature you can see within range. If the target can hear you, it must succeed on a Wisdom saving throw or take 1d4 psychic damage and have disadvantage on its next attack roll before the end of its next turn.",
                Level = 0,
                School = "Enchantment",
                CastingTime = "1 action",
                Range = "60 feet",
                Components = "V",
                Duration = "Instantaneous",
                Classes = new List<string> { "Bard" }
            }
        };
    }
}
