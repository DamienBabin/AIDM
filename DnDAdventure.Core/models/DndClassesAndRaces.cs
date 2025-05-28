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
        public SpellcastingInfo? Spellcasting { get; set; }
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
                Name = "Barbarian",
                Description = "A fierce warrior of primitive background who can enter a battle rage",
                HitDie = 12,
                PrimaryAbilities = new List<string> { "Strength" },
                SavingThrowProficiencies = new List<string> { "Strength", "Constitution" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Leather armor", "Shield", "Greataxe", "Javelin", "Explorer's pack" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Rage", Description = "In battle, you fight with primal ferocity.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Unarmored Defense", Description = "While not wearing armor, your AC equals 10 + Dex modifier + Con modifier.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Reckless Attack", Description = "You can throw aside all concern for defense to attack with fierce desperation.", LevelUnlocked = 2 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass 
                    { 
                        Name = "Path of the Berserker", 
                        Description = "For some barbarians, rage is a means to an end—that end being violence.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Frenzy", Description = "You can go into a frenzy when you rage, making an additional attack as a bonus action.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Mindless Rage", Description = "You can't be charmed or frightened while raging.", LevelUnlocked = 6 }
                        },
                        Pros = new List<string> 
                        { 
                            "Frenzy provides extra attacks for massive damage",
                            "Mindless Rage prevents charm and fear effects",
                            "Intimidating Presence provides crowd control",
                            "Retaliation punishes enemies who attack you",
                            "Simple and effective damage-focused subclass"
                        },
                        Cons = new List<string> 
                        { 
                            "Frenzy causes exhaustion after rage ends",
                            "Limited utility outside of combat",
                            "Exhaustion can accumulate quickly",
                            "Less tactical options than other subclasses"
                        },
                        BestFor = new List<string> 
                        { 
                            "Players who want maximum damage output",
                            "Characters focused on pure combat effectiveness",
                            "Short adventuring days where exhaustion isn't a concern",
                            "Players who enjoy straightforward gameplay"
                        },
                        PlayStyle = "Relentless damage dealer who sacrifices everything for overwhelming offensive power"
                    },
                    new Subclass 
                    { 
                        Name = "Path of the Totem Warrior", 
                        Description = "Accepts a spirit animal as guide, protector, and inspiration.",
                        Features = new List<ClassFeature> 
                        { 
                            new ClassFeature { Name = "Spirit Seeker", Description = "You gain the ability to cast beast sense and speak with animals as rituals.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Totem Spirit", Description = "Choose a totem spirit and gain its benefit while raging.", LevelUnlocked = 3 }
                        },
                        Pros = new List<string> 
                        { 
                            "Bear totem provides resistance to all damage except psychic",
                            "Eagle totem allows others to dash as bonus action",
                            "Wolf totem gives allies advantage on attacks",
                            "Great utility with ritual spells",
                            "Flexible and supportive subclass"
                        },
                        Cons = new List<string> 
                        { 
                            "Less direct damage than Path of the Berserker",
                            "Benefits depend on totem choice",
                            "Some totems are more situational",
                            "Requires understanding of different totem options"
                        },
                        BestFor = new List<string> 
                        { 
                            "Players who want survivability (Bear totem)",
                            "Characters who support the party in combat",
                            "Players interested in nature and spirit themes",
                            "Those who prefer tactical flexibility"
                        },
                        PlayStyle = "Spiritually-connected warrior who channels animal spirits for diverse combat benefits"
                    }
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
                StartingEquipment = new List<string> { "Chain mail", "Shield", "Martial weapon", "Light crossbow", "Dungeoneer's pack" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Fighting Style", Description = "You adopt a particular style of fighting as your specialty.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Second Wind", Description = "You have a limited well of stamina that you can draw on to protect yourself from harm.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Action Surge", Description = "You can push yourself beyond your normal limits for a moment.", LevelUnlocked = 2 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass
                    {
                        Name = "Champion",
                        Description = "The archetypal Champion focuses on the development of raw physical power honed to deadly perfection.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Improved Critical", Description = "Your weapon attacks score a critical hit on a roll of 19 or 20.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Remarkable Athlete", Description = "You can add half your proficiency bonus to any Strength, Dexterity, or Constitution check you make that doesn't already use your proficiency bonus.", LevelUnlocked = 7 }
                        },
                        Pros = new List<string>
                        {
                            "Simple and effective for new players",
                            "Improved critical hit chance",
                            "Enhanced athletic abilities",
                            "Consistent damage output",
                            "No resource management required"
                        },
                        Cons = new List<string>
                        {
                            "Less tactical complexity than other subclasses",
                            "Limited utility outside combat",
                            "Fewer special abilities",
                            "Can feel repetitive in play"
                        },
                        BestFor = new List<string>
                        {
                            "New players learning the game",
                            "Players who prefer straightforward combat",
                            "Characters focused on weapon mastery",
                            "Campaigns with lots of combat"
                        },
                        PlayStyle = "Straightforward warrior who excels at consistent, reliable combat performance"
                    },
                    new Subclass
                    {
                        Name = "Battle Master",
                        Description = "Those who emulate the archetypal Battle Master employ martial techniques passed down through generations.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Combat Superiority", Description = "You learn maneuvers that are fueled by special dice called superiority dice.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Student of War", Description = "You gain proficiency with one type of artisan's tools of your choice.", LevelUnlocked = 3 }
                        },
                        Pros = new List<string>
                        {
                            "Tactical combat maneuvers",
                            "Versatile battlefield control",
                            "Resource management adds depth",
                            "Great for tactical players",
                            "Scales well with level"
                        },
                        Cons = new List<string>
                        {
                            "Requires understanding of maneuvers",
                            "Limited superiority dice per rest",
                            "More complex than Champion",
                            "Needs tactical thinking"
                        },
                        BestFor = new List<string>
                        {
                            "Tactical combat enthusiasts",
                            "Players who like resource management",
                            "Characters who want battlefield control",
                            "Experienced players"
                        },
                        PlayStyle = "Tactical combatant who uses superior techniques and battlefield awareness"
                    },
                    new Subclass
                    {
                        Name = "Eldritch Knight",
                        Description = "The archetypal Eldritch Knight combines the martial mastery common to all fighters with a careful study of magic.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Spellcasting", Description = "You augment your martial prowess with the ability to cast spells.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Weapon Bond", Description = "You learn a ritual that creates a magical bond between yourself and one weapon.", LevelUnlocked = 3 }
                        },
                        Spellcasting = new SpellcastingInfo
                        {
                            SpellcastingAbility = "Intelligence",
                            CantripsKnown = 2,
                            SpellsKnown = 3,
                            SpellSlots = 2,
                            RitualCasting = false,
                            SpellcastingFocus = "Component pouch or arcane focus"
                        },
                        Pros = new List<string>
                        {
                            "Combines martial and magical abilities",
                            "Shield spell for excellent defense",
                            "Weapon bond prevents disarming",
                            "War Magic allows spell + attack",
                            "Unique hybrid playstyle"
                        },
                        Cons = new List<string>
                        {
                            "Limited spell selection",
                            "MAD (Multiple Ability Dependent)",
                            "Slower spell progression",
                            "Complex resource management"
                        },
                        BestFor = new List<string>
                        {
                            "Players who want magic and martial combat",
                            "Characters interested in war magic",
                            "Gish (warrior-mage) concepts",
                            "Players who like versatility"
                        },
                        PlayStyle = "Warrior-mage who blends sword and sorcery for versatile combat options"
                    }
                }
            },
            new DndClass
            {
                Name = "Wizard",
                Description = "A scholarly magic-user capable of manipulating the structures of spellcasting to cast spells of explosive power",
                HitDie = 6,
                PrimaryAbilities = new List<string> { "Intelligence" },
                SavingThrowProficiencies = new List<string> { "Intelligence", "Wisdom" },
                ArmorProficiencies = new List<string> { },
                WeaponProficiencies = new List<string> { "Daggers", "Darts", "Slings", "Quarterstaffs", "Light crossbows" },
                StartingEquipment = new List<string> { "Quarterstaff", "Component pouch", "Scholar's pack", "Spellbook", "Two daggers" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Spellcasting", Description = "As a student of arcane magic, you have a spellbook containing spells that show the first glimmerings of your true power.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Arcane Recovery", Description = "You have learned to regain some of your magical energy by studying your spellbook.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Ritual Casting", Description = "You can cast a wizard spell as a ritual if that spell has the ritual tag and you have the spell in your spellbook.", LevelUnlocked = 1 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Intelligence",
                    CantripsKnown = 3,
                    SpellsKnown = 6,
                    SpellSlots = 2,
                    RitualCasting = true,
                    SpellcastingFocus = "Arcane focus",
                    Spellbook = true
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass
                    {
                        Name = "School of Evocation",
                        Description = "You focus your study on magic that creates powerful elemental effects such as bitter cold, searing flame, rolling thunder, crackling lightning, and burning acid.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Evocation Savant", Description = "The gold and time you must spend to copy an evocation spell into your spellbook is halved.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Sculpt Spells", Description = "You can create pockets of relative safety within the effects of your evocation spells.", LevelUnlocked = 2 }
                        },
                        Pros = new List<string>
                        {
                            "Sculpt Spells protects allies from area damage",
                            "Excellent damage output",
                            "Cheaper evocation spells to learn",
                            "Great for blaster wizards",
                            "Potent Cantrip adds damage"
                        },
                        Cons = new List<string>
                        {
                            "Limited to damage-focused magic",
                            "Less utility than other schools",
                            "Can be one-dimensional",
                            "Friendly fire concerns without Sculpt Spells"
                        },
                        BestFor = new List<string>
                        {
                            "Players who want to deal massive damage",
                            "Characters focused on combat magic",
                            "Blaster caster concepts",
                            "Players who like explosive spells"
                        },
                        PlayStyle = "Destructive spellcaster who specializes in elemental damage and battlefield control"
                    },
                    new Subclass
                    {
                        Name = "School of Divination",
                        Description = "The counsel of a diviner is sought by royalty and commoners alike, for all seek a clearer understanding of the past, present, and future.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Divination Savant", Description = "The gold and time you must spend to copy a divination spell into your spellbook is halved.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Portent", Description = "You can replace any attack roll, saving throw, or ability check made by you or a creature that you can see with one of these foretelling rolls.", LevelUnlocked = 2 }
                        },
                        Pros = new List<string>
                        {
                            "Portent allows incredible control over dice rolls",
                            "Excellent utility and information gathering",
                            "Can guarantee critical successes or failures",
                            "Great support capabilities",
                            "Third Eye provides various benefits"
                        },
                        Cons = new List<string>
                        {
                            "Less direct damage than other schools",
                            "Requires strategic thinking",
                            "Limited Portent uses per day",
                            "Divination spells can be situational"
                        },
                        BestFor = new List<string>
                        {
                            "Strategic players who like controlling outcomes",
                            "Support-focused characters",
                            "Players interested in information gathering",
                            "Characters who want to manipulate fate"
                        },
                        PlayStyle = "Oracle-like spellcaster who sees the future and manipulates probability"
                    },
                    new Subclass
                    {
                        Name = "School of Enchantment",
                        Description = "You have honed your ability to entrance and beguile other people and monsters.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Enchantment Savant", Description = "The gold and time you must spend to copy an enchantment spell into your spellbook is halved.", LevelUnlocked = 2 },
                            new ClassFeature { Name = "Hypnotic Gaze", Description = "Your soft words and enchanting gaze can magically enthrall another creature.", LevelUnlocked = 2 }
                        },
                        Pros = new List<string>
                        {
                            "Excellent crowd control abilities",
                            "Hypnotic Gaze provides at-will charm",
                            "Great for social encounters",
                            "Split Enchantment affects multiple targets",
                            "Strong utility outside combat"
                        },
                        Cons = new List<string>
                        {
                            "Many creatures are immune to charm",
                            "Less effective against undead and constructs",
                            "Requires careful positioning",
                            "Limited direct damage options"
                        },
                        BestFor = new List<string>
                        {
                            "Social-focused campaigns",
                            "Players who like mind control",
                            "Support and control specialists",
                            "Characters interested in manipulation"
                        },
                        PlayStyle = "Manipulative spellcaster who controls minds and bends others to their will"
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
                WeaponProficiencies = new List<string> { "Simple Weapons", "Hand crossbows", "Longswords", "Rapiers", "Shortswords" },
                StartingEquipment = new List<string> { "Leather armor", "Two shortswords", "Thieves' tools", "Burglar's pack", "Dagger" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Expertise", Description = "Choose two of your skill proficiencies. Your proficiency bonus is doubled for any ability check you make that uses either of the chosen proficiencies.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Sneak Attack", Description = "You know how to strike subtly and exploit a foe's distraction.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Thieves' Cant", Description = "You learned thieves' cant, a secret mix of dialect, jargon, and code.", LevelUnlocked = 1 }
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass
                    {
                        Name = "Thief",
                        Description = "You hone your skills in the larcenous arts. Burglars, bandits, cutpurses, and other criminals typically follow this archetype.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Fast Hands", Description = "You can use the bonus action granted by your Cunning Action to make a Dexterity (Sleight of Hand) check, use your thieves' tools to disarm a trap or open a lock, or take the Use an Object action.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Second-Story Work", Description = "You gain the ability to climb faster than normal; climbing no longer costs you extra movement.", LevelUnlocked = 3 }
                        },
                        Pros = new List<string>
                        {
                            "Fast Hands allows bonus action object use",
                            "Excellent climbing and mobility",
                            "Use Magic Device for any magic item",
                            "Thief's Reflexes for two turns in first round",
                            "Great utility and exploration"
                        },
                        Cons = new List<string>
                        {
                            "Less combat-focused than other archetypes",
                            "Abilities are situational",
                            "Requires creative thinking",
                            "Limited direct damage bonuses"
                        },
                        BestFor = new List<string>
                        {
                            "Exploration-heavy campaigns",
                            "Players who like creative problem solving",
                            "Characters focused on utility",
                            "Classic thief concepts"
                        },
                        PlayStyle = "Classic burglar and treasure hunter with unmatched utility and mobility"
                    },
                    new Subclass
                    {
                        Name = "Assassin",
                        Description = "You focus your training on the grim art of death. Those who adhere to this archetype are diverse.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Bonus Proficiencies", Description = "You gain proficiency with the disguise kit and the poisoner's kit.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Assassinate", Description = "You are at your deadliest when you get the drop on your enemies.", LevelUnlocked = 3 }
                        },
                        Pros = new List<string>
                        {
                            "Assassinate provides automatic crits on surprised enemies",
                            "Advantage on initiative",
                            "Infiltration Expertise for creating identities",
                            "Impostor ability for perfect disguises",
                            "Excellent burst damage potential"
                        },
                        Cons = new List<string>
                        {
                            "Heavily dependent on surprise",
                            "Limited utility if stealth fails",
                            "Requires careful positioning",
                            "Less effective in prolonged combat"
                        },
                        BestFor = new List<string>
                        {
                            "Stealth-focused campaigns",
                            "Players who like high-risk, high-reward gameplay",
                            "Infiltration and espionage themes",
                            "Characters who want massive damage potential"
                        },
                        PlayStyle = "Deadly infiltrator who strikes from the shadows for devastating surprise attacks"
                    },
                    new Subclass
                    {
                        Name = "Arcane Trickster",
                        Description = "Some rogues enhance their fine-honed skills of stealth and agility with magic, learning tricks of enchantment and illusion.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Spellcasting", Description = "You augment your martial prowess with the ability to cast spells.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Mage Hand Legerdemain", Description = "You can make the spectral hand invisible, and you can perform additional tasks with it.", LevelUnlocked = 3 }
                        },
                        Spellcasting = new SpellcastingInfo
                        {
                            SpellcastingAbility = "Intelligence",
                            CantripsKnown = 3,
                            SpellsKnown = 3,
                            SpellSlots = 2,
                            RitualCasting = false,
                            SpellcastingFocus = "Component pouch or arcane focus"
                        },
                        Pros = new List<string>
                        {
                            "Combines rogue skills with wizard spells",
                            "Invisible Mage Hand for remote manipulation",
                            "Shield spell for defense",
                            "Magical Ambush for spell advantage",
                            "Versatile utility and combat options"
                        },
                        Cons = new List<string>
                        {
                            "Limited spell selection",
                            "MAD (Multiple Ability Dependent)",
                            "Slower spell progression",
                            "Complex resource management"
                        },
                        BestFor = new List<string>
                        {
                            "Players who want magic and stealth",
                            "Characters interested in magical trickery",
                            "Utility-focused rogues",
                            "Players who like versatility"
                        },
                        PlayStyle = "Magical trickster who combines stealth, cunning, and arcane power"
                    }
                }
            },
            new DndClass
            {
                Name = "Ranger",
                Description = "A warrior of the wilderness, skilled in tracking, survival, and combat against favored enemies",
                HitDie = 10,
                PrimaryAbilities = new List<string> { "Dexterity", "Wisdom" },
                SavingThrowProficiencies = new List<string> { "Strength", "Dexterity" },
                ArmorProficiencies = new List<string> { "Light Armor", "Medium Armor", "Shields" },
                WeaponProficiencies = new List<string> { "Simple Weapons", "Martial Weapons" },
                StartingEquipment = new List<string> { "Scale mail", "Two shortswords", "Dungeoneer's pack", "Leather armor", "Longbow" },
                Features = new List<ClassFeature>
                {
                    new ClassFeature { Name = "Favored Enemy", Description = "You have significant experience studying, tracking, hunting, and even talking to a certain type of creature.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Natural Explorer", Description = "You are particularly familiar with one type of natural environment and are adept at traveling and surviving in such regions.", LevelUnlocked = 1 },
                    new ClassFeature { Name = "Spellcasting", Description = "You have learned to use the magical essence of nature to cast spells.", LevelUnlocked = 2 }
                },
                Spellcasting = new SpellcastingInfo
                {
                    SpellcastingAbility = "Wisdom",
                    CantripsKnown = 0,
                    SpellsKnown = 2,
                    SpellSlots = 2,
                    RitualCasting = true,
                    SpellcastingFocus = "Druidfocus"
                },
                Subclasses = new List<Subclass>
                {
                    new Subclass
                    {
                        Name = "Hunter",
                        Description = "Emulating the Hunter archetype means accepting your place as a bulwark between civilization and the terrors of the wilderness.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Hunter's Prey", Description = "You gain one of the following features of your choice: Colossus Slayer, Giant Killer, or Horde Breaker.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Defensive Tactics", Description = "You gain one of the following features of your choice: Escape the Horde, Multiattack Defense, or Steel Will.", LevelUnlocked = 7 }
                        },
                        Pros = new List<string>
                        {
                            "Versatile combat options",
                            "Horde Breaker for multiple attacks",
                            "Colossus Slayer for extra damage against wounded enemies",
                            "Defensive tactics for survivability",
                            "Multiattack for high-level combat",
                            "Flexible and adaptable subclass"
                        },
                        Cons = new List<string>
                        {
                            "Abilities can be situational",
                            "Less specialized than other subclasses",
                            "Requires tactical decision making",
                            "Some features compete for bonus actions"
                        },
                        BestFor = new List<string>
                        {
                            "Players who want tactical flexibility",
                            "Characters facing varied enemy types",
                            "Balanced combat and utility focus",
                            "Classic ranger concepts"
                        },
                        PlayStyle = "Versatile wilderness warrior who adapts tactics to different combat situations"
                    },
                    new Subclass
                    {
                        Name = "Beast Master",
                        Description = "The Beast Master archetype embodies a friendship between the civilized races and the beasts of the world.",
                        Features = new List<ClassFeature>
                        {
                            new ClassFeature { Name = "Ranger's Companion", Description = "You gain a beast companion that accompanies you on your adventures and is trained to fight alongside you.", LevelUnlocked = 3 },
                            new ClassFeature { Name = "Exceptional Training", Description = "Your animal companion gains proficiency in two skills of your choice.", LevelUnlocked = 7 }
                        },
                        Pros = new List<string>
                        {
                            "Animal companion provides extra actions",
                            "Companion can scout and track",
                            "Share Spells allows buffing companion",
                            "Unique roleplay opportunities",
                            "Companion grows stronger with levels"
                        },
                        Cons = new List<string>
                        {
                            "Companion can be fragile",
                            "Action economy can be complex",
                            "Companion may die and need replacement",
                            "Less personal combat effectiveness"
                        },
                        BestFor = new List<string>
                        {
                            "Players who want an animal companion",
                            "Characters interested in nature bonds",
                            "Players who like managing multiple units",
                            "Roleplay-focused campaigns"
                        },
                        PlayStyle = "Nature-bonded ranger who fights alongside a loyal animal companion"
                    }
                }
            }
        };

        public List<DndRace> Races { get; set; } = new()
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
