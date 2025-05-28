// DnDAdventure.Core/Models/DndClassesAndRaces.cs
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
        public List<Subclass> Subclasses { get; set; } = new();
        public SpellcastingInfo? Spellcasting { get; set; }
    }

    public class Subclass
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ClassFeature> Features { get; set; } = new();
        public string Source { get; set; } = "Player's Handbook";
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
                    new ClassFeature { Name = "Rage", Description = "In battle, you fight with primal ferocity.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Unarmored Defense", Description = "While you are not wearing any armor, your AC equals 10 + your Dexterity modifier + your Constitution modifier.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Path of the Berserker", Description = "A path of untrammeled fury, slick with blood.", Features = new List<ClassFeature> { new ClassFeature { Name = "Frenzy", Description = "You can go into a frenzy when you rage.", LevelUnlocked = 3 } } },
                    new Subclass { Name = "Path of the Totem Warrior", Description = "A spiritual journey with a spirit animal as guide.", Features = new List<ClassFeature> { new ClassFeature { Name = "Totem Spirit", Description = "Choose a totem spirit and gain its feature.", LevelUnlocked = 3 } } },
                    new Subclass { Name = "Path of the Ancestral Guardian", Description = "Warriors of the past linger as mighty spirits.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Path of the Storm Herald", Description = "Transform rage into a mantle of primal magic.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Path of the Zealot", Description = "Channel rage into displays of divine power.", Source = "Xanathar's Guide to Everything" }
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
                    new ClassFeature { Name = "Bardic Inspiration", Description = "You can inspire others through stirring words or music.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Spellcasting", Description = "You have learned to untangle and reshape the fabric of reality.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Charisma",
                    CantripsKnown = 2,
                    SpellsKnown = 4,
                    SpellSlots = 2,
                    RitualCasting = true,
                    SpellcastingFocus = "Musical instrument"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "College of Lore", Description = "Know something about most things, collecting knowledge from diverse sources." },
                    new Subclass { Name = "College of Valor", Description = "Daring skalds whose tales inspire a new generation of heroes." },
                    new Subclass { Name = "College of Glamour", Description = "Masters of craft in the vibrant realm of the Feywild.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "College of Swords", Description = "Blades who entertain through daring feats of weapon prowess.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "College of Whispers", Description = "Use their reputation to gather secrets and spread rumors.", Source = "Xanathar's Guide to Everything" }
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
                    new ClassFeature { Name = "Spellcasting", Description = "As a conduit for divine power, you can cast cleric spells.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Divine Domain", Description = "Choose one domain related to your deity.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Wisdom",
                    CantripsKnown = 3,
                    SpellsKnown = 0, // Clerics prepare spells
                    SpellSlots = 2,
                    RitualCasting = true,
                    SpellcastingFocus = "Holy symbol"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Knowledge Domain", Description = "Gods of knowledge value learning and understanding above all." },
                    new Subclass { Name = "Life Domain", Description = "Focuses on the vibrant positive energy that sustains all life." },
                    new Subclass { Name = "Light Domain", Description = "Promote ideals of rebirth, renewal, truth, and vigilance." },
                    new Subclass { Name = "Nature Domain", Description = "Gods of nature, from deep forests to particular springs and groves." },
                    new Subclass { Name = "Tempest Domain", Description = "Gods who govern storms, sea, and sky." },
                    new Subclass { Name = "Trickery Domain", Description = "Mischief-makers who challenge the accepted order." },
                    new Subclass { Name = "War Domain", Description = "Gods of war who make heroes of ordinary people." },
                    new Subclass { Name = "Forge Domain", Description = "Patrons of artisans who work with metal.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Grave Domain", Description = "Watch over the line between life and death.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Druid",
                Description = "A priest of nature, wielding elemental forces and transforming into animals",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Wisdom" },
                SavingThrowProficiencies = new List<string> { "Intelligence", "Wisdom" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields (non-metal)" },
                WeaponProficiencies = new List<string> { "Clubs", "Daggers", "Darts", "Javelins", "Maces", "Quarterstaffs", "Scimitars", "Sickles", "Slings", "Spears" },
                StartingEquipment = new List<string> { "Leather armor", "Shield", "Scimitar", "Druidcraft", "Explorer's pack" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Druidcraft", Description = "You know the druidcraft cantrip.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Spellcasting", Description = "Drawing on the divine essence of nature itself.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Wisdom",
                    CantripsKnown = 2,
                    SpellsKnown = 0, // Druids prepare spells
                    SpellSlots = 2,
                    RitualCasting = true,
                    SpellcastingFocus = "Druidcraft focus"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Circle of the Land", Description = "Mystics and sages who safeguard ancient knowledge through oral tradition." },
                    new Subclass { Name = "Circle of the Moon", Description = "Fierce guardians of the wilds who gather under the full moon." },
                    new Subclass { Name = "Circle of Dreams", Description = "Hail from regions with strong ties to the Feywild.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Circle of the Shepherd", Description = "Commune with spirits of nature, especially beasts and fey.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Fighter",
                Description = "A master of martial combat, skilled with a variety of weapons and armor",
                HitDie = 10,
                PrimaryAbilities = new List<string> { "Strength", "Dexterity" },
                SavingThrowProficiencies = new List<string> { "Strength", "Constitution" },
                ArmorProficiencies = new List<string> { "All Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Chain mail", "Shield", "Martial weapon", "Light crossbow and 20 bolts", "Dungeoneer's pack" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Fighting Style", Description = "You adopt a particular style of fighting as your specialty.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Second Wind", Description = "You have a limited well of stamina to protect yourself.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Champion", Description = "Focuses on raw physical power honed to deadly perfection." },
                    new Subclass { Name = "Battle Master", Description = "Employ martial techniques passed down through generations." },
                    new Subclass { Name = "Eldritch Knight", Description = "Combines martial mastery with careful study of magic." },
                    new Subclass { Name = "Arcane Archer", Description = "Studies elven archery that weaves magic into attacks.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Cavalier", Description = "Masters of mounted combat and chivalric ideals.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Samurai", Description = "Fighters who draw on implacable fighting spirit.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Monk",
                Description = "A master of martial arts, harnessing inner power through discipline and practice",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Dexterity", "Wisdom" },
                SavingThrowProficiencies = new List<string> { "Strength", "Dexterity" },
                ArmorProficiencies = new List<string> { },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Shortswords" },
                StartingEquipment = new List<string> { "Shortsword", "Dungeoneer's pack", "10 darts" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Unarmored Defense", Description = "Your AC equals 10 + Dex modifier + Wis modifier.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Martial Arts", Description = "Mastery of combat styles using unarmed strikes.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Way of the Open Hand", Description = "Masters of unarmed combat, manipulating ki to heal or harm." },
                    new Subclass { Name = "Way of Shadow", Description = "Follow a tradition that values stealth and subterfuge." },
                    new Subclass { Name = "Way of the Four Elements", Description = "Harness the elements through their ki." },
                    new Subclass { Name = "Way of the Drunken Master", Description = "Teaches students to move with jerky, unpredictable motions.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Way of the Kensei", Description = "Train relentlessly with weapons, to the point of becoming one with them.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Way of the Sun Soul", Description = "Learn to channel their life energy into searing bolts of light.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Paladin",
                Description = "A holy warrior bound to a sacred oath, wielding divine magic and martial prowess",
                HitDie = 10,
                PrimaryAbilities = new List<string> { "Strength", "Charisma" },
                SavingThrowProficiencies = new List<string> { "Wisdom", "Charisma" },
                ArmorProficiencies = new List<string> { "All Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Chain mail", "Shield", "Martial weapon", "5 javelins", "Priest's pack", "Holy symbol" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Divine Sense", Description = "The presence of strong evil registers on your senses.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Lay on Hands", Description = "Your blessed touch can heal wounds.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Oath of Devotion", Description = "Binds a paladin to the loftiest ideals of justice and virtue." },
                    new Subclass { Name = "Oath of the Ancients", Description = "As old as the race of elves and the rituals of the druids." },
                    new Subclass { Name = "Oath of Vengeance", Description = "A solemn commitment to punish those who have committed grievous sins." },
                    new Subclass { Name = "Oath of Conquest", Description = "Seeks to rule through strength and intimidation.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Oath of Redemption", Description = "Believes that anyone can be redeemed.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Oathbreaker", Description = "A paladin who breaks their sacred oaths to pursue dark ambitions." }
                }
            },
            new DndClass
            {
                Name = "Ranger",
                Description = "A warrior of the wilderness, skilled in tracking, survival, and combat",
                HitDie = 10,
                PrimaryAbilities = new List<string> { "Dexterity", "Wisdom" },
                SavingThrowProficiencies = new List<string> { "Strength", "Dexterity" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Scale mail", "Shortsword", "Longbow and quiver of 20 arrows", "Dungeoneer's pack" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Favored Enemy", Description = "You have experience studying and hunting certain creatures.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Natural Explorer", Description = "You are adept at traveling and surviving in natural environments.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Hunter", Description = "Emulates the Hunter archetype, accepting the responsibility to protect civilization." },
                    new Subclass { Name = "Beast Master", Description = "Embodies a friendship between civilized races and beasts of the world." },
                    new Subclass { Name = "Gloom Stalker", Description = "At home in the darkest places, seeking out evil wherever it lurks.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Horizon Walker", Description = "Guards the world against threats from other planes.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Monster Slayer", Description = "Dedicated to hunting down creatures of the night.", Source = "Xanathar's Guide to Everything" }
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
                    new ClassFeature { Name = "Sneak Attack", Description = "You know how to strike subtly and exploit a foe's distraction.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Thieves' Cant", Description = "A secret mix of dialect, jargon, and code.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Thief", Description = "Hones skills in larceny and develops talents useful for delving into ruins." },
                    new Subclass { Name = "Assassin", Description = "Masters of dealing sudden death, striking from shadows." },
                    new Subclass { Name = "Arcane Trickster", Description = "Enhances larcenous abilities with magic, learning tricks of enchantment and illusion." },
                    new Subclass { Name = "Inquisitive", Description = "Masters of rooting out secrets and unraveling mysteries.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Mastermind", Description = "Focus on people and influence rather than locks and traps.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Scout", Description = "Skirmishers, spies, bounty hunters, and assassins.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Swashbuckler", Description = "Focuses on the art of the blade and speed in combat.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Sorcerer",
                Description = "A spellcaster who draws on inherent magic from a gift or bloodline",
                HitDie = 6,
                PrimaryAbilities = new List<string> { "Charisma" },
                SavingThrowProficiencies = new List<string> { "Constitution", "Charisma" },
                ArmorProficiencies = new List<string> { },
                WeaponProficiencies = new List<string> { "Daggers", "Darts", "Slings", "Quarterstaffs", "Light crossbows" },
                StartingEquipment = new List<string> { "Light crossbow and 20 bolts", "Arcane focus", "Dungeoneer's pack", "Two daggers" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Spellcasting", Description = "An event in your past infused you with arcane magic.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Sorcerous Origin", Description = "Choose an origin that describes the source of your magical power.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Charisma",
                    CantripsKnown = 4,
                    SpellsKnown = 2,
                    SpellSlots = 2,
                    RitualCasting = false,
                    SpellcastingFocus = "Arcane focus"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "Draconic Bloodline", Description = "Your innate magic comes from draconic magic in your blood." },
                    new Subclass { Name = "Wild Magic", Description = "Your spellcasting can unleash surges of untamed magic." },
                    new Subclass { Name = "Divine Soul", Description = "Your magic springs from a divine source within yourself.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Shadow Magic", Description = "Your innate magic comes from the Shadowfell itself.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "Storm Sorcery", Description = "Your innate magic comes from the power of elemental air.", Source = "Xanathar's Guide to Everything" }
                }
            },
            new DndClass
            {
                Name = "Warlock",
                Description = "A wielder of magic derived from a bargain with an extraplanar entity",
                HitDie = 8,
                PrimaryAbilities = new List<string> { "Charisma" },
                SavingThrowProficiencies = new List<string> { "Wisdom", "Charisma" },
                ArmorProficiencies = new List<string> { "Light Armor" },
                WeaponProficiencies = new List<string> { "Simple Weapons" },
                StartingEquipment = new List<string> { "Light crossbow and 20 bolts", "Arcane focus", "Scholar's pack", "Leather armor", "Two daggers", "Simple weapon" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Otherworldly Patron", Description = "You have struck a pact with an otherworldly being.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Pact Magic", Description = "Your patron has given you facility with spells.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Charisma",
                    CantripsKnown = 2,
                    SpellsKnown = 2,
                    SpellSlots = 1,
                    RitualCasting = false,
                    SpellcastingFocus = "Arcane focus"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "The Archfey", Description = "Your patron is a lord or lady of the fey." },
                    new Subclass { Name = "The Fiend", Description = "You have made a pact with a fiend from the lower planes." },
                    new Subclass { Name = "The Great Old One", Description = "Your patron is a mysterious entity whose nature is alien to reality." },
                    new Subclass { Name = "The Celestial", Description = "Your patron is a powerful being of the Upper Planes.", Source = "Xanathar's Guide to Everything" },
                    new Subclass { Name = "The Hexblade", Description = "Your patron exists in the Shadowfell as a sentient magic weapon.", Source = "Xanathar's Guide to Everything" }
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
                    new ClassFeature { Name = "Spellcasting", Description = "You have a spellbook containing spells that show your true power.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Arcane Recovery", Description = "You can regain magical energy by studying your spellbook.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Intelligence",
                    CantripsKnown = 3,
                    SpellsKnown = 0, // Wizards prepare spells
                    SpellSlots = 2,
                    RitualCasting = true,
                    Spellbook = true,
                    SpellcastingFocus = "Arcane focus"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass { Name = "School of Abjuration", Description = "Emphasizes magic that blocks, banishes, or protects." },
                    new Subclass { Name = "School of Conjuration", Description = "Focuses on spells that produce objects and creatures out of nothing." },
                    new Subclass { Name = "School of Divination", Description = "Seeks to unveil the mysteries of the world through magic." },
                    new Subclass { Name = "School of Enchantment", Description = "Affects the minds of others, influencing or controlling their behavior." },
                    new Subclass { Name = "School of Evocation", Description = "Focuses on magic that creates powerful elemental effects." },
                    new Subclass { Name = "School of Illusion", Description = "Focuses on spells that dazzle the senses and trick the mind." },
                    new Subclass { Name = "School of Necromancy", Description = "Explores the cosmic forces of life, death, and undeath." },
                    new Subclass { Name = "School of Transmutation", Description = "Modifies energy and matter through magic." },
                    new Subclass { Name = "War Magic", Description = "Combines evocation and abjuration to dominate the battlefield.", Source = "Xanathar's Guide to Everything" }
                }
            }
        };

        public List<DndRace> Races { get; set; } = new()
        {
            new DndRace
            {
                Name = "Human",
                Description = "Humans are the most adaptable and ambitious people among the common races.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 1 }, { "Dexterity", 1 }, { "Constitution", 1 }, { "Intelligence", 1 }, { "Wisdom", 1 }, { "Charisma", 1 } },
                Speed = 30,
                Languages = new List<string> { "Common", "One additional language of your choice" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Extra Skill", Description = "You gain proficiency in one skill of your choice." },
                    new RacialTrait { Name = "Extra Feat", Description = "You gain one feat of your choice." }
                },
                Subraces = new List<Subrace>
                {
                    new Subrace
                    {
                        Name = "Variant Human",
                        Description = "Some humans are born with exceptional abilities.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Any Two Different", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Skills", Description = "You gain proficiency in one skill of your choice." },
                            new RacialTrait { Name = "Feat", Description = "You gain one feat of your choice." }
                        }
                    }
                }
            },
            new DndRace
            {
                Name = "Elf",
                Description = "Elves are a magical people of otherworldly grace, living in places of ethereal beauty.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 2 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Elvish" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Keen Senses", Description = "You have proficiency in the Perception skill." },
                    new RacialTrait { Name = "Fey Ancestry", Description = "You have advantage on saving throws against being charmed." },
                    new RacialTrait { Name = "Trance", Description = "Elves don't need to sleep. Instead, they meditate deeply for 4 hours a day." }
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
                            new RacialTrait { Name = "Elf Weapon Training", Description = "You have proficiency with longswords, shortbows, longbows, and shortswords." },
                            new RacialTrait { Name = "Cantrip", Description = "You know one cantrip of your choice from the wizard spell list." },
                            new RacialTrait { Name = "Extra Language", Description = "You can speak, read, and write one extra language of your choice." }
                        }
                    },
                    new Subrace
                    {
                        Name = "Wood Elf",
                        Description = "Wood elves have keen senses and intuition, and their fleet feet carry them quickly through their forest homes.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Wisdom", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Elf Weapon Training", Description = "You have proficiency with longswords, shortbows, longbows, and shortswords." },
                            new RacialTrait { Name = "Fleet of Foot", Description = "Your base walking speed increases to 35 feet." },
                            new RacialTrait { Name = "Mask of the Wild", Description = "You can attempt to hide even when only lightly obscured by foliage." }
                        }
                    },
                    new Subrace
                    {
                        Name = "Dark Elf (Drow)",
                        Description = "Descended from an earlier subrace of dark-skinned elves, the drow were banished from the surface world.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Charisma", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Superior Darkvision", Description = "Your darkvision has a radius of 120 feet." },
                            new RacialTrait { Name = "Sunlight Sensitivity", Description = "You have disadvantage on attack rolls and Perception checks in direct sunlight." },
                            new RacialTrait { Name = "Drow Magic", Description = "You know the dancing lights cantrip. At 3rd level, you can cast faerie fire once per day." },
                            new RacialTrait { Name = "Drow Weapon Training", Description = "You have proficiency with rapiers, shortswords, and hand crossbows." }
                        }
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
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Dwarven Resilience", Description = "You have advantage on saving throws against poison." },
                    new RacialTrait { Name = "Dwarven Combat Training", Description = "You have proficiency with battleaxes, handaxes, light hammers, and warhammers." },
                    new RacialTrait { Name = "Stonecunning", Description = "You add double your proficiency bonus to History checks related to stonework." }
                },
                Subraces = new List<Subrace>
                {
                    new Subrace
                    {
                        Name = "Hill Dwarf",
                        Description = "Hill dwarves are known for their keen senses, deep intuition, and remarkable resilience.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Wisdom", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Dwarven Toughness", Description = "Your hit point maximum increases by 1, and it increases by 1 every time you gain a level." }
                        }
                    },
                    new Subrace
                    {
                        Name = "Mountain Dwarf",
                        Description = "Mountain dwarves are strong and hardy, accustomed to a difficult life in rugged terrain.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 2 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Armor Proficiency", Description = "You have proficiency with light and medium armor." }
                        }
                    }
                }
            },
            new DndRace
            {
                Name = "Halfling",
                Description = "The diminutive halflings survive in a world full of larger creatures by avoiding notice.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 2 } },
                Speed = 25,
                Languages = new List<string> { "Common", "Halfling" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Lucky", Description = "When you roll a 1 on an attack roll, ability check, or saving throw, you can reroll the die." },
                    new RacialTrait { Name = "Brave", Description = "You have advantage on saving throws against being frightened." },
                    new RacialTrait { Name = "Halfling Nimbleness", Description = "You can move through the space of any creature that is larger than you." }
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
                            new RacialTrait { Name = "Naturally Stealthy", Description = "You can attempt to hide even when obscured only by a creature that is at least one size larger than you." }
                        }
                    },
                    new Subrace
                    {
                        Name = "Stout Halfling",
                        Description = "Stout halflings have dwarven blood and are more durable than other halflings.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Constitution", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Stout Resilience", Description = "You have advantage on saving throws against poison, and resistance to poison damage." }
                        }
                    }
                }
            },
            new DndRace
            {
                Name = "Dragonborn",
                Description = "Born of dragons, the dragonborn walk proudly through a world that greets them with fearful incomprehension.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 2 }, { "Charisma", 1 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Draconic" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Draconic Ancestry", Description = "You have draconic ancestry. Choose one type of dragon from the Draconic Ancestry table." },
                    new RacialTrait { Name = "Breath Weapon", Description = "You can use your action to exhale destructive energy." },
                    new RacialTrait { Name = "Damage Resistance", Description = "You have resistance to the damage type associated with your draconic ancestry." }
                }
            },
            new DndRace
            {
                Name = "Gnome",
                Description = "A gnome's energy and enthusiasm for living shines through every inch of his or her tiny body.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Intelligence", 2 } },
                Speed = 25,
                Languages = new List<string> { "Common", "Gnomish" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Gnome Cunning", Description = "You have advantage on all Intelligence, Wisdom, and Charisma saving throws against magic." }
                },
                Subraces = new List<Subrace>
                {
                    new Subrace
                    {
                        Name = "Forest Gnome",
                        Description = "Forest gnomes have a natural knack for illusion and inherent quickness and stealth.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Natural Illusionist", Description = "You know the minor illusion cantrip." },
                            new RacialTrait { Name = "Speak with Small Beasts", Description = "You can communicate simple ideas with Small or smaller beasts." }
                        }
                    },
                    new Subrace
                    {
                        Name = "Rock Gnome",
                        Description = "Rock gnomes are the most common gnomes, known for their technological bent.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Constitution", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Artificer's Lore", Description = "Add twice your proficiency bonus to History checks related to magic items, alchemical objects, or technological devices." },
                            new RacialTrait { Name = "Tinker", Description = "You have proficiency with artisan's tools (tinker's tools)." }
                        }
                    }
                }
            },
            new DndRace
            {
                Name = "Half-Elf",
                Description = "Half-elves combine what some say are the best qualities of their elf and human parents.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Charisma", 2 }, { "Any Two Different", 1 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Elvish", "One additional language of your choice" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Fey Ancestry", Description = "You have advantage on saving throws against being charmed." },
                    new RacialTrait { Name = "Two Skills", Description = "You gain proficiency in two skills of your choice." }
                }
            },
            new DndRace
            {
                Name = "Half-Orc",
                Description = "Half-orcs most often live among orcs. Of the other races, humans are most likely to accept half-orcs.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 2 }, { "Constitution", 1 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Orc" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Relentless Endurance", Description = "When you are reduced to 0 hit points but not killed outright, you can drop to 1 hit point instead." },
                    new RacialTrait { Name = "Savage Attacks", Description = "When you score a critical hit with a melee weapon attack, you can roll one additional weapon damage die." }
                }
            },
            new DndRace
            {
                Name = "Tiefling",
                Description = "Tieflings are derived from human bloodlines, and in the broadest possible sense, they still look human.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Intelligence", 1 }, { "Charisma", 2 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Infernal" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                    new RacialTrait { Name = "Hellish Resistance", Description = "You have resistance to fire damage." },
                    new RacialTrait { Name = "Infernal Legacy", Description = "You know the thaumaturgy cantrip. At 3rd level, you can cast hellish rebuke once per day." }
                }
            },
            new DndRace
            {
                Name = "Aarakocra",
                Description = "Aarakocra are bird-like humanoids who soar above the world, viewing it from the skies.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 2 }, { "Wisdom", 1 } },
                Speed = 25,
                Languages = new List<string> { "Common", "Aarakocra", "Auran" },
                Traits = new List<RacialTrait>
                {
                    new RacialTrait { Name = "Flight", Description = "You have a flying speed of 50 feet. You can't use this flying speed if you're wearing medium or heavy armor." },
                    new RacialTrait { Name = "Talons", Description = "Your talons are natural weapons, which you can use to make unarmed strikes." }
                },
                Source = "Elemental Evil Player's Companion"
            },
            new DndRace
            {
                Name = "Genasi",
                Description = "Genasi are planetouched humans, infused with the power of the elemental planes.",
                AbilityScoreIncrease = new Dictionary<string, int> { { "Constitution", 2 } },
                Speed = 30,
                Languages = new List<string> { "Common", "Primordial" },
                Traits = new List<RacialTrait>(),
                Subraces = new List<Subrace>
                {
                    new Subrace
                    {
                        Name = "Air Genasi",
                        Description = "Air genasi are proud of their heritage, sometimes to the point of haughtiness.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Dexterity", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Unending Breath", Description = "You can hold your breath indefinitely while you're not incapacitated." },
                            new RacialTrait { Name = "Mingle with the Wind", Description = "You can cast the levitate spell once with this trait, requiring no material components." }
                        },
                        Source = "Elemental Evil Player's Companion"
                    },
                    new Subrace
                    {
                        Name = "Earth Genasi",
                        Description = "Earth genasi are more withdrawn, and their connection to the earth keeps them confident and strong.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Strength", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Earth Walk", Description = "You can move across difficult terrain made of earth or stone without expending extra movement." },
                            new RacialTrait { Name = "Merge with Stone", Description = "You can cast the pass without trace spell once with this trait, requiring no material components." }
                        },
                        Source = "Elemental Evil Player's Companion"
                    },
                    new Subrace
                    {
                        Name = "Fire Genasi",
                        Description = "Fire genasi have obvious physical traits that mark them as different.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Intelligence", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Darkvision", Description = "You can see in dim light within 60 feet as if it were bright light." },
                            new RacialTrait { Name = "Fire Resistance", Description = "You have resistance to fire damage." },
                            new RacialTrait { Name = "Reach to the Blaze", Description = "You know the produce flame cantrip." }
                        },
                        Source = "Elemental Evil Player's Companion"
                    },
                    new Subrace
                    {
                        Name = "Water Genasi",
                        Description = "Water genasi are perfectly suited to life underwater and carry the power of the waves inside themselves.",
                        AbilityScoreIncrease = new Dictionary<string, int> { { "Wisdom", 1 } },
                        Traits = new List<RacialTrait>
                        {
                            new RacialTrait { Name = "Acid Resistance", Description = "You have resistance to acid damage." },
                            new RacialTrait { Name = "Amphibious", Description = "You can breathe air and water." },
                            new RacialTrait { Name = "Swimming", Description = "You have a swimming speed of 30 feet." },
                            new RacialTrait { Name = "Call to the Wave", Description = "You know the shape water cantrip." }
                        },
                        Source = "Elemental Evil Player's Companion"
                    }
                }
            }
        };
    }
}
