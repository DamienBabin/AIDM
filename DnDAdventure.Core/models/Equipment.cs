// DnDAdventure.Core/models/Equipment.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDAdventure.Core.Models
{
    public class Equipment
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EquipmentType { get; set; } = string.Empty; // Renamed from Type
        public string Rarity { get; set; } = string.Empty; // Common, Uncommon, Rare, etc.
        public Dictionary<string, int> StatModifiers { get; set; } = new();
        public int GoldValue { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class Weapon : Equipment
    {
        public WeaponType WeaponType { get; set; }
        public WeaponProperty[] Properties { get; set; } = Array.Empty<WeaponProperty>();
        public string Damage { get; set; } = string.Empty; // e.g. "1d6"
        public string DamageType { get; set; } = string.Empty; // e.g. "Slashing"
        public int Weight { get; set; } // in pounds
        public string Range { get; set; } = string.Empty; // e.g. "20/60" for ranged weapons
    }

    public enum WeaponType
    {
        SimpleRanged,
        SimpleMelee,
        MartialRanged,
        MartialMelee
    }

    public enum WeaponProperty
    {
        Ammunition,
        Finesse,
        Heavy,
        Light,
        Loading,
        Range,
        Reach,
        Special,
        Thrown,
        TwoHanded,
        Versatile
    }

    public class Armor : Equipment
    {
        public ArmorType ArmorType { get; set; }
        public int ArmorClass { get; set; }
        public bool AddDexModifier { get; set; }
        public int? MaxDexModifier { get; set; } // null means no limit
        public int Weight { get; set; } // in pounds
        public bool StealthDisadvantage { get; set; }
        public int? StrengthRequirement { get; set; } // null means no requirement
        public bool MagicalArmor { get; set; } = false;
        public int MagicalBonus { get; set; } = 0; // +1, +2, etc.
    }

    public enum ArmorType
    {
        LightArmor,
        MediumArmor,
        HeavyArmor,
        Shield
    }

    public class EquipmentList
    {
        public List<Weapon> Weapons { get; private set; }
        public List<Armor> Armors { get; private set; }

        public EquipmentList()
        {
            Weapons = InitializeWeapons();
            Armors = InitializeArmors();
        }

        private List<Weapon> InitializeWeapons()
        {
            return new List<Weapon>
            {
                // Simple Melee Weapons
                new Weapon
                {
                    Id = "weapon_club",
                    Name = "Club",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Light },
                    Damage = "1d4",
                    DamageType = "Bludgeoning",
                    Weight = 2,
                    GoldValue = 1,
                    Description = "A simple wooden club."
                },
                new Weapon
                {
                    Id = "weapon_dagger",
                    Name = "Dagger",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Finesse, WeaponProperty.Light, WeaponProperty.Thrown },
                    Damage = "1d4",
                    DamageType = "Piercing",
                    Weight = 1,
                    GoldValue = 2,
                    Range = "20/60",
                    Description = "A small knife with a pointed blade."
                },
                new Weapon
                {
                    Id = "weapon_greatclub",
                    Name = "Greatclub",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Bludgeoning",
                    Weight = 10,
                    GoldValue = 2,
                    Description = "A heavy wooden club, larger than a regular club."
                },
                new Weapon
                {
                    Id = "weapon_handaxe",
                    Name = "Handaxe",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Light, WeaponProperty.Thrown },
                    Damage = "1d6",
                    DamageType = "Slashing",
                    Weight = 2,
                    GoldValue = 5,
                    Range = "20/60",
                    Description = "A light axe designed for one-handed use."
                },
                new Weapon
                {
                    Id = "weapon_quarterstaff",
                    Name = "Quarterstaff",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d6", // 1d8 when used with two hands
                    DamageType = "Bludgeoning",
                    Weight = 4,
                    GoldValue = 2,
                    Description = "A wooden staff that can be wielded with one or two hands."
                },

                // Simple Ranged Weapons
                new Weapon
                {
                    Id = "weapon_light_crossbow",
                    Name = "Crossbow, Light",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Loading, WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 5,
                    GoldValue = 25,
                    Range = "80/320",
                    Description = "A light crossbow that fires bolts."
                },
                new Weapon
                {
                    Id = "weapon_shortbow",
                    Name = "Shortbow",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.SimpleRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.TwoHanded },
                    Damage = "1d6",
                    DamageType = "Piercing",
                    Weight = 2,
                    GoldValue = 25,
                    Range = "80/320",
                    Description = "A small bow that's easy to use."
                },
                
                // Martial Melee Weapons
                new Weapon
                {
                    Id = "weapon_longsword",
                    Name = "Longsword",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d8", // 1d10 when used with two hands
                    DamageType = "Slashing",
                    Weight = 3,
                    GoldValue = 15,
                    Description = "A versatile sword that can be used with one or two hands."
                },
                new Weapon
                {
                    Id = "weapon_greatsword",
                    Name = "Greatsword",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Heavy, WeaponProperty.TwoHanded },
                    Damage = "2d6",
                    DamageType = "Slashing",
                    Weight = 6,
                    GoldValue = 50,
                    Description = "A large, heavy sword that must be wielded with two hands."
                },
                new Weapon
                {
                    Id = "weapon_rapier",
                    Name = "Rapier",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Finesse },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 2,
                    GoldValue = 25,
                    Description = "A slender, sharply pointed sword."
                },
                new Weapon
                {
                    Id = "weapon_warhammer",
                    Name = "Warhammer",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d8", // 1d10 when used with two hands
                    DamageType = "Bludgeoning",
                    Weight = 2,
                    GoldValue = 15,
                    Description = "A heavy hammer designed for combat."
                },

                // Martial Ranged Weapons
                new Weapon
                {
                    Id = "weapon_heavy_crossbow",
                    Name = "Crossbow, Heavy",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Heavy, WeaponProperty.Loading, WeaponProperty.TwoHanded },
                    Damage = "1d10",
                    DamageType = "Piercing",
                    Weight = 18,
                    GoldValue = 50,
                    Range = "100/400",
                    Description = "A powerful crossbow that requires training to use effectively."
                },
                new Weapon
                {
                    Id = "weapon_longbow",
                    Name = "Longbow",
                    EquipmentType = "Weapon",
                    Rarity = "Common",
                    WeaponType = WeaponType.MartialRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Heavy, WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 2,
                    GoldValue = 50,
                    Range = "150/600",
                    Description = "A tall bow that can shoot arrows over long distances."
                }
            };
        }

        private List<Armor> InitializeArmors()
        {
            return new List<Armor>
            {
                // Light Armor
                new Armor
                {
                    Id = "armor_padded",
                    Name = "Padded",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.LightArmor,
                    ArmorClass = 11,
                    AddDexModifier = true,
                    Weight = 8,
                    GoldValue = 5,
                    StealthDisadvantage = true,
                    Description = "Padded armor consists of quilted layers of cloth and batting."
                },
                new Armor
                {
                    Id = "armor_leather",
                    Name = "Leather",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.LightArmor,
                    ArmorClass = 11,
                    AddDexModifier = true,
                    Weight = 10,
                    GoldValue = 10,
                    StealthDisadvantage = false,
                    Description = "The breastplate and shoulder protectors of this armor are made of leather that has been stiffened by being boiled in oil."
                },
                new Armor
                {
                    Id = "armor_studded_leather",
                    Name = "Studded Leather",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.LightArmor,
                    ArmorClass = 12,
                    AddDexModifier = true,
                    Weight = 13,
                    GoldValue = 45,
                    StealthDisadvantage = false,
                    Description = "Made from tough but flexible leather, studded leather is reinforced with close-set rivets or spikes."
                },

                // Medium Armor
                new Armor
                {
                    Id = "armor_hide",
                    Name = "Hide",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.MediumArmor,
                    ArmorClass = 12,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 12,
                    GoldValue = 10,
                    StealthDisadvantage = false,
                    Description = "This crude armor consists of thick furs and pelts."
                },
                new Armor
                {
                    Id = "armor_chain_shirt",
                    Name = "Chain Shirt",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.MediumArmor,
                    ArmorClass = 13,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 20,
                    GoldValue = 50,
                    StealthDisadvantage = false,
                    Description = "Made of interlocking metal rings, a chain shirt is worn between layers of clothing or leather."
                },
                new Armor
                {
                    Id = "armor_scale_mail",
                    Name = "Scale Mail",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.MediumArmor,
                    ArmorClass = 14,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 45,
                    GoldValue = 50,
                    StealthDisadvantage = true,
                    Description = "This armor consists of a coat and leggings of leather covered with overlapping pieces of metal, much like the scales of a fish."
                },
                new Armor
                {
                    Id = "armor_breastplate",
                    Name = "Breastplate",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.MediumArmor,
                    ArmorClass = 14,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 20,
                    GoldValue = 400,
                    StealthDisadvantage = false,
                    Description = "This armor consists of a fitted metal chest piece worn with supple leather."
                },
                new Armor
                {
                    Id = "armor_half_plate",
                    Name = "Half Plate",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.MediumArmor,
                    ArmorClass = 15,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 40,
                    GoldValue = 750,
                    StealthDisadvantage = true,
                    Description = "Half plate consists of shaped metal plates that cover most of the wearer's body."
                },

                // Heavy Armor
                new Armor
                {
                    Id = "armor_ring_mail",
                    Name = "Ring Mail",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.HeavyArmor,
                    ArmorClass = 14,
                    AddDexModifier = false,
                    Weight = 40,
                    GoldValue = 30,
                    StealthDisadvantage = true,
                    Description = "This armor is leather armor with heavy rings sewn into it."
                },
                new Armor
                {
                    Id = "armor_chain_mail",
                    Name = "Chain Mail",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.HeavyArmor,
                    ArmorClass = 16,
                    AddDexModifier = false,
                    Weight = 55,
                    GoldValue = 75,
                    StealthDisadvantage = true,
                    StrengthRequirement = 13,
                    Description = "Made of interlocking metal rings, chain mail includes a layer of quilted fabric worn underneath to prevent chafing and to cushion the impact of blows."
                },
                new Armor
                {
                    Id = "armor_splint",
                    Name = "Splint",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.HeavyArmor,
                    ArmorClass = 17,
                    AddDexModifier = false,
                    Weight = 60,
                    GoldValue = 200,
                    StealthDisadvantage = true,
                    StrengthRequirement = 15,
                    Description = "This armor is made of narrow vertical strips of metal riveted to a backing of leather that is worn over cloth padding."
                },
                new Armor
                {
                    Id = "armor_plate",
                    Name = "Plate",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.HeavyArmor,
                    ArmorClass = 18,
                    AddDexModifier = false,
                    Weight = 65,
                    GoldValue = 1500,
                    StealthDisadvantage = true,
                    StrengthRequirement = 15,
                    Description = "Plate consists of shaped, interlocking metal plates to cover the entire body."
                },

                // Shield
                new Armor
                {
                    Id = "armor_shield",
                    Name = "Shield",
                    EquipmentType = "Armor",
                    Rarity = "Common",
                    ArmorType = ArmorType.Shield,
                    ArmorClass = 2, // This is a bonus to AC, not base AC
                    AddDexModifier = false,
                    Weight = 6,
                    GoldValue = 10,
                    StealthDisadvantage = false,
                    Description = "A shield is made from wood or metal and is carried in one hand."
                }
            };
        }

        // Helper methods to find equipment
        public Weapon? GetWeaponByName(string name)
        {
            return Weapons.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Armor? GetArmorByName(string name)
        {
            return Armors.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Weapon> GetWeaponsByType(WeaponType type)
        {
            return Weapons.Where(w => w.WeaponType == type).ToList();
        }

        public List<Armor> GetArmorsByType(ArmorType type)
        {
            return Armors.Where(a => a.ArmorType == type).ToList();
        }
    }
}

