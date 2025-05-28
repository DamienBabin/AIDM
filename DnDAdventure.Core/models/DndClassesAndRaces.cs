// DnDAdventure.Core/Models/DndClassesAndRaces.cs
namespace DnDAdventure.Core.Models
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

    public class DndClassesAndRaces
    {
        public List<DndClass> Classes { get; set; } = new()
        {
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
                    new Subclass 
                    { 
                        Name = "Champion", 
                        Description = "Focuses on raw physical power honed to deadly perfection.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Improved Critical", Description = "Your weapon attacks score a critical hit on a roll of 19 or 20.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Remarkable Athlete", Description = "Add half your proficiency bonus to Strength, Dexterity, and Constitution checks.", LevelUnlocked = 7 }
                        },
                        Pros = new List<string> 
                        { 
                            "Simple and effective - great for new players",
                            "Expanded critical hit range increases damage output",
                            "Self-healing with Second Wind",
                            "Excellent survivability with high AC and HP",
                            "Versatile - works with any fighting style"
                        },
                        Cons = new List<string> 
                        { 
                            "Limited tactical options compared to other subclasses",
                            "No magical abilities or utility spells",
                            "Can become repetitive in combat",
                            "Fewer out-of-combat abilities"
                        },
                        BestFor = new List<string> 
                        { 
                            "New players learning the game",
                            "Players who prefer straightforward combat",
                            "Tank builds focused on survivability",
                            "Damage dealers who want consistent performance"
                        },
                        PlayStyle = "Straightforward melee combatant focused on dealing consistent damage and surviving on the front lines"
                    },
                    new Subclass 
                    { 
                        Name = "Battle Master", 
                        Description = "Employ martial techniques passed down through generations.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Combat Superiority", Description = "Learn maneuvers that are fueled by special dice called superiority dice.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Student of War", Description = "Gain proficiency with one type of artisan's tools.", LevelUnlocked = 3 }
                        },
                        Pros = new List<string> 
                        { 
                            "Highly tactical with many combat maneuvers",
                            "Versatile - can adapt to different situations",
                            "Great battlefield control options",
                            "Superiority dice add strategic resource management",
                            "Excellent for experienced players who like options"
                        },
                        Cons = new List<string> 
                        { 
                            "More complex than Champion - requires planning",
                            "Limited uses of maneuvers per short rest",
                            "Can be overwhelming for new players",
                            "Requires good knowledge of positioning and tactics"
                        },
                        BestFor = new List<string> 
                        { 
                            "Tactical players who enjoy complex combat",
                            "Leaders who want to support allies",
                            "Players who like resource management",
                            "Characters focused on battlefield control"
                        },
                        PlayStyle = "Tactical combatant who uses special maneuvers to control the battlefield and support allies"
                    },
                    new Subclass 
                    { 
                        Name = "Eldritch Knight", 
                        Description = "Combines martial mastery with careful study of magic.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Spellcasting", Description = "You learn to cast spells, focusing on abjuration and evocation.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Weapon Bond", Description = "You can bond with up to two weapons, allowing you to summon them.", LevelUnlocked = 3 }
                        },
                        Spellcasting = new SpellcastingInfo
                        {
                            SpellcastingAbility = "Intelligence",
                            CantripsKnown = 2,
                            SpellsKnown = 3,
                            SpellSlots = 2,
                            RitualCasting = false,
                            SpellcastingFocus = "Weapon or component pouch"
                        },
                        Pros = new List<string> 
                        { 
                            "Combines martial prowess with magic",
                            "Shield spell provides excellent defense",
                            "Cantrips offer ranged attack options",
                            "War Magic allows weapon attacks with cantrips",
                            "Unique blend of fighter and wizard abilities"
                        },
                        Cons = new List<string> 
                        { 
                            "Limited spell selection (mostly abjuration/evocation)",
                            "Slower spell progression than full casters",
                            "Requires investment in Intelligence",
                            "More complex character management",
                            "May feel like a weaker version of both fighter and wizard"
                        },
                        BestFor = new List<string> 
                        { 
                            "Players who want magic without full spellcaster complexity",
                            "Characters who need both melee and ranged options",
                            "Players interested in gish (warrior-mage) builds",
                            "Those who enjoy defensive magic like Shield and Absorb Elements"
                        },
                        PlayStyle = "Warrior-mage who enhances martial combat with protective and offensive spells"
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
                    new Subclass 
                    { 
                        Name = "School of Evocation", 
                        Description = "Focuses on magic that creates powerful elemental effects.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Sculpt Spells", Description = "You can protect allies from your area-of-effect spells.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Potent Cantrip", Description = "Your damaging cantrips affect even those who avoid the brunt of the effect.", LevelUnlocked = 6 }
                        },
                        Pros = new List<string> 
                        { 
                            "Excellent damage output with evocation spells",
                            "Sculpt Spells prevents friendly fire",
                            "Potent Cantrip ensures cantrips always deal some damage",
                            "Empowered Evocation adds Intelligence to damage",
                            "Great for blaster builds focused on damage"
                        },
                        Cons = new List<string> 
                        { 
                            "Limited utility compared to other schools",
                            "Focused primarily on damage dealing",
                            "May struggle in social encounters",
                            "Fewer defensive options than abjuration",
                            "Can be one-dimensional in playstyle"
                        },
                        BestFor = new List<string> 
                        { 
                            "Players who want to be the party's primary damage dealer",
                            "Those who enjoy blasting enemies with fireballs",
                            "Characters focused on battlefield control through damage",
                            "Players who prefer straightforward spellcasting"
                        },
                        PlayStyle = "Destructive spellcaster focused on dealing maximum damage while protecting allies from collateral damage"
                    },
                    new Subclass 
                    { 
                        Name = "School of Abjuration", 
                        Description = "Emphasizes magic that blocks, banishes, or protects.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Arcane Ward", Description = "You can weave magic around yourself for protection.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Projected Ward", Description = "You can use your Arcane Ward to protect others.", LevelUnlocked = 6 }
                        },
                        Pros = new List<string> 
                        { 
                            "Excellent survivability with Arcane Ward",
                            "Can protect allies with Projected Ward",
                            "Resistance to spell damage at higher levels",
                            "Great utility with counterspell and dispel magic",
                            "Perfect for defensive-minded players"
                        },
                        Cons = new List<string> 
                        { 
                            "Lower damage output than evocation",
                            "More reactive than proactive playstyle",
                            "Requires good game knowledge to use effectively",
                            "May feel less exciting than damage-focused builds",
                            "Ward can be depleted quickly in tough fights"
                        },
                        BestFor = new List<string> 
                        { 
                            "Players who want to be the party's protector",
                            "Those who enjoy defensive and utility magic",
                            "Characters focused on battlefield control",
                            "Players who like countering enemy spellcasters"
                        },
                        PlayStyle = "Protective spellcaster who shields allies and counters enemy magic while maintaining battlefield control"
                    },
                    new Subclass 
                    { 
                        Name = "School of Divination", 
                        Description = "Seeks to unveil the mysteries of the world through magic.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Portent", Description = "You can replace any attack roll, saving throw, or ability check with a foretold roll.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Expert Divination", Description = "Casting divination spells comes so easily to you that it expends only a fraction of your spellcasting efforts.", LevelUnlocked = 6 }
                        },
                        Pros = new List<string> 
                        { 
                            "Portent provides incredible tactical control",
                            "Can guarantee critical successes or failures",
                            "Excellent utility with divination spells",
                            "Great for players who like to plan ahead",
                            "Third Eye provides useful reconnaissance abilities"
                        },
                        Cons = new List<string> 
                        { 
                            "Lower direct damage than other schools",
                            "Portent dice are limited per day",
                            "Requires strategic thinking to use effectively",
                            "May feel less impactful in pure combat",
                            "Divination spells can be situational"
                        },
                        BestFor = new List<string> 
                        { 
                            "Strategic players who enjoy controlling outcomes",
                            "Those who like support and utility roles",
                            "Characters focused on information gathering",
                            "Players who enjoy manipulating probability"
                        },
                        PlayStyle = "Oracle-like spellcaster who manipulates fate and gathers information to support the party"
                    }
                }
            }
        };

        public List<DndRace> Races { get; set; } = new()
        {
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
                        },
                        Pros = new List<string> 
                        { 
                            "Free wizard cantrip provides magical versatility",
                            "Intelligence bonus great for wizards and artificers",
                            "Weapon training gives martial options to casters",
                            "Extra language aids in social encounters",
                            "Excellent for gish (warrior-mage) builds"
                        },
                        Cons = new List<string> 
                        { 
                            "Intelligence bonus less useful for non-caster classes",
                            "Weapon training may be redundant for some classes",
                            "Cantrip uses Intelligence, may not scale well",
                            "Less specialized than other elf subraces"
                        },
                        BestFor = new List<string> 
                        { 
                            "Wizards and other Intelligence-based casters",
                            "Eldritch Knights and other gish builds",
                            "Characters who want magical and martial options",
                            "Players who enjoy versatile characters"
                        },
                        PlayStyle = "Versatile character combining martial prowess with arcane magic, excellent for hybrid builds"
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
                        },
                        Pros = new List<string> 
                        { 
                            "Wisdom bonus excellent for druids, clerics, and rangers",
                            "Increased speed provides tactical mobility",
                            "Mask of the Wild enables stealth in natural environments",
                            "Weapon training includes longbow proficiency",
                            "Perfect for nature-based characters"
                        },
                        Cons = new List<string> 
                        { 
                            "Wisdom bonus less useful for non-Wisdom classes",
                            "Mask of the Wild only works in natural settings",
                            "Weapon training may overlap with class proficiencies",
                            "Less versatile than High Elf's cantrip"
                        },
                        BestFor = new List<string> 
                        { 
                            "Rangers, druids, and clerics",
                            "Characters focused on archery and stealth",
                            "Nature-themed campaigns and characters",
                            "Players who enjoy mobility and stealth"
                        },
                        PlayStyle = "Swift, stealthy character at home in natural environments, excellent archer and scout"
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
                        },
                        Pros = new List<string> 
                        { 
                            "Charisma bonus great for bards, sorcerers, warlocks, and paladins",
                            "Superior darkvision excellent for underground adventures",
                            "Drow magic provides useful utility spells",
                            "Weapon training includes finesse weapons",
                            "Unique and interesting roleplay opportunities"
                        },
                        Cons = new List<string> 
                        { 
                            "Sunlight sensitivity is a significant drawback",
                            "May face social prejudice in many campaigns",
                            "Charisma bonus less useful for non-Charisma classes",
                            "Drow magic spells are limited use",
                            "Sunlight sensitivity affects outdoor adventures"
                        },
                        BestFor = new List<string> 
                        { 
                            "Charisma-based casters (bards, sorcerers, warlocks)",
                            "Paladins who can handle the moral complexity",
                            "Underground or nighttime campaigns",
                            "Players who enjoy complex roleplay challenges"
                        },
                        PlayStyle = "Charismatic character with innate magic, best suited for underground adventures and complex social dynamics"
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
                        },
                        Pros = new List<string> 
                        { 
                            "Exceptional survivability with Constitution bonus and extra HP",
                            "Wisdom bonus great for clerics, druids, and rangers",
                            "Dwarven Toughness provides permanent HP boost",
                            "Poison resistance useful against many enemies",
                            "Excellent for tanky support characters"
                        },
                        Cons = new List<string> 
                        { 
                            "Reduced speed can limit tactical positioning",
                            "Wisdom bonus less useful for non-Wisdom classes",
                            "No additional combat abilities beyond base dwarf traits",
                            "May struggle to keep up with faster party members"
                        },
                        BestFor = new List<string> 
                        { 
                            "Clerics and other Wisdom-based casters",
                            "Tank builds focused on survivability",
                            "Support characters who need to stay alive",
                            "Players who prefer defensive playstyles"
                        },
                        PlayStyle = "Incredibly durable support character with excellent survivability and healing capabilities"
                    },
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
                            "Strength bonus excellent for fighters, paladins, and barbarians",
                            "Armor proficiency allows casters to wear better protection",
                            "Constitution bonus provides excellent survivability",
                            "Great for front-line combatants",
                            "Unique combination of mental and physical stats"
                        },
                        Cons = new List<string> 
                        { 
                            "Reduced speed limits mobility in combat",
                            "Strength bonus less useful for Dexterity-based builds",
                            "Armor proficiency may be redundant for some classes",
                            "No additional HP like Hill Dwarf"
                        },
                        BestFor = new List<string> 
                        { 
                            "Strength-based fighters and paladins",
                            "Clerics who want better armor options",
                            "Barbarians (though speed reduction hurts)",
                            "Any character wanting durability and martial prowess"
                        },
                        PlayStyle = "Sturdy warrior combining physical might with natural toughness, excellent front-line combatant"
                    }
                }
            },
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
                        },
                        Pros = new List<string> 
                        { 
                            "Free feat at 1st level provides significant customization",
                            "Flexible ability score increases fit any build",
                            "Extra skill proficiency adds versatility",
                            "Can take powerful feats like Great Weapon Master early",
                            "Excellent for optimized builds"
                        },
                        Cons = new List<string> 
                        { 
                            "No special abilities like darkvision or resistances",
                            "May feel 'boring' compared to exotic races",
                            "Smaller total ability score bonuses than standard human",
                            "Relies heavily on feat choice for uniqueness"
                        },
                        BestFor = new List<string> 
                        { 
                            "Optimized builds that benefit from early feats",
                            "Any class that wants specific feat synergies",
