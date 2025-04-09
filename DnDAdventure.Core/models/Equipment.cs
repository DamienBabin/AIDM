// DnDAdventure.Core/Models/Equipment.cs
namespace DnDAdventure.Core.Models
{
    public class Weapon
    {
        public string Name { get; set; } = string.Empty;
        public WeaponType Type { get; set; }
        public WeaponProperty[] Properties { get; set; } = Array.Empty<WeaponProperty>();
        public string Damage { get; set; } = string.Empty; // e.g. "1d6"
        public string DamageType { get; set; } = string.Empty; // e.g. "Slashing"
        public int Weight { get; set; } // in pounds
        public int Cost { get; set; } // in gold pieces
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

    public class Armor
    {
        public string Name { get; set; } = string.Empty;
        public ArmorType Type { get; set; }
        public int ArmorClass { get; set; }
        public bool AddDexModifier { get; set; }
        public int? MaxDexModifier { get; set; } // null means no limit
        public int Weight { get; set; } // in pounds
        public int Cost { get; set; } // in gold pieces
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
                    Name = "Club",
                    Type = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Light },
                    Damage = "1d4",
                    DamageType = "Bludgeoning",
                    Weight = 2,
                    Cost = 1
                },
                new Weapon
                {
                    Name = "Dagger",
                    Type = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Finesse, WeaponProperty.Light, WeaponProperty.Thrown },
                    Damage = "1d4",
                    DamageType = "Piercing",
                    Weight = 1,
                    Cost = 2,
                    Range = "20/60"
                },
                new Weapon
                {
                    Name = "Greatclub",
                    Type = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Bludgeoning",
                    Weight = 10,
                    Cost = 2
                },
                new Weapon
                {
                    Name = "Handaxe",
                    Type = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Light, WeaponProperty.Thrown },
                    Damage = "1d6",
                    DamageType = "Slashing",
                    Weight = 2,
                    Cost = 5,
                    Range = "20/60"
                },
                new Weapon
                {
                    Name = "Quarterstaff",
                    Type = WeaponType.SimpleMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d6", // 1d8 when used with two hands
                    DamageType = "Bludgeoning",
                    Weight = 4,
                    Cost = 2
                },
                
                // Simple Ranged Weapons
                new Weapon
                {
                    Name = "Crossbow, Light",
                    Type = WeaponType.SimpleRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Loading, WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 5,
                    Cost = 25,
                    Range = "80/320"
                },
                new Weapon
                {
                    Name = "Shortbow",
                    Type = WeaponType.SimpleRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.TwoHanded },
                    Damage = "1d6",
                    DamageType = "Piercing",
                    Weight = 2,
                    Cost = 25,
                    Range = "80/320"
                },
                
                // Martial Melee Weapons
                new Weapon
                {
                    Name = "Longsword",
                    Type = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d8", // 1d10 when used with two hands
                    DamageType = "Slashing",
                    Weight = 3,
                    Cost = 15
                },
                new Weapon
                {
                    Name = "Greatsword",
                    Type = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Heavy, WeaponProperty.TwoHanded },
                    Damage = "2d6",
                    DamageType = "Slashing",
                    Weight = 6,
                    Cost = 50
                },
                new Weapon
                {
                    Name = "Rapier",
                    Type = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Finesse },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 2,
                    Cost = 25
                },
                new Weapon
                {
                    Name = "Warhammer",
                    Type = WeaponType.MartialMelee,
                    Properties = new[] { WeaponProperty.Versatile },
                    Damage = "1d8", // 1d10 when used with two hands
                    DamageType = "Bludgeoning",
                    Weight = 2,
                    Cost = 15
                },
                
                // Martial Ranged Weapons
                new Weapon
                {
                    Name = "Crossbow, Heavy",
                    Type = WeaponType.MartialRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Heavy, WeaponProperty.Loading, WeaponProperty.TwoHanded },
                    Damage = "1d10",
                    DamageType = "Piercing",
                    Weight = 18,
                    Cost = 50,
                    Range = "100/400"
                },
                new Weapon
                {
                    Name = "Longbow",
                    Type = WeaponType.MartialRanged,
                    Properties = new[] { WeaponProperty.Ammunition, WeaponProperty.Heavy, WeaponProperty.TwoHanded },
                    Damage = "1d8",
                    DamageType = "Piercing",
                    Weight = 2,
                    Cost = 50,
                    Range = "150/600"
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
                    Name = "Padded",
                    Type = ArmorType.LightArmor,
                    ArmorClass = 11,
                    AddDexModifier = true,
                    Weight = 8,
                    Cost = 5,
                    StealthDisadvantage = true
                },
                new Armor
                {
                    Name = "Leather",
                    Type = ArmorType.LightArmor,
                    ArmorClass = 11,
                    AddDexModifier = true,
                    Weight = 10,
                    Cost = 10,
                    StealthDisadvantage = false
                },
                new Armor
                {
                    Name = "Studded Leather",
                    Type = ArmorType.LightArmor,
                    ArmorClass = 12,
                    AddDexModifier = true,
                    Weight = 13,
                    Cost = 45,
                    StealthDisadvantage = false
                },
                
                // Medium Armor
                new Armor
                {
                    Name = "Hide",
                    Type = ArmorType.MediumArmor,
                    ArmorClass = 12,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 12,
                    Cost = 10,
                    StealthDisadvantage = false
                },
                new Armor
                {
                    Name = "Chain Shirt",
                    Type = ArmorType.MediumArmor,
                    ArmorClass = 13,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 20,
                    Cost = 50,
                    StealthDisadvantage = false
                },
                new Armor
                {
                    Name = "Scale Mail",
                    Type = ArmorType.MediumArmor,
                    ArmorClass = 14,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 45,
                    Cost = 50,
                    StealthDisadvantage = true
                },
                new Armor
                {
                    Name = "Breastplate",
                    Type = ArmorType.MediumArmor,
                    ArmorClass = 14,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 20,
                    Cost = 400,
                    StealthDisadvantage = false
                },
                new Armor
                {
                    Name = "Half Plate",
                    Type = ArmorType.MediumArmor,
                    ArmorClass = 15,
                    AddDexModifier = true,
                    MaxDexModifier = 2,
                    Weight = 40,
                    Cost = 750,
                    StealthDisadvantage = true
                },
                
                // Heavy Armor
                new Armor
                {
                    Name = "Ring Mail",
                    Type = ArmorType.HeavyArmor,
                    ArmorClass = 14,
                    AddDexModifier = false,
                    Weight = 40,
                    Cost = 30,
                    StealthDisadvantage = true
                },
                new Armor
                {
                    Name = "Chain Mail",
                    Type = ArmorType.HeavyArmor,
                    ArmorClass = 16,
                    AddDexModifier = false,
                    Weight = 55,
                    Cost = 75,
                    StealthDisadvantage = true,
                    StrengthRequirement = 13
                },
                new Armor
                {
                    Name = "Splint",
                    Type = ArmorType.HeavyArmor,
                    ArmorClass = 17,
                    AddDexModifier = false,
                    Weight = 60,
                    Cost = 200,
                    StealthDisadvantage = true,
                    StrengthRequirement = 15
                },
                new Armor
                {
                    Name = "Plate",
                    Type = ArmorType.HeavyArmor,
                    ArmorClass = 18,
                    AddDexModifier = false,
                    Weight = 65,
                    Cost = 1500,
                    StealthDisadvantage = true,
                    StrengthRequirement = 15
                },
                
                // Shield
                new Armor
                {
                    Name = "Shield",
                    Type = ArmorType.Shield,
                    ArmorClass = 2, // This is a bonus to AC, not base AC
                    AddDexModifier = false,
                    Weight = 6,
                    Cost = 10,
                    StealthDisadvantage = false
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
            return Weapons.Where(w => w.Type == type).ToList();
        }
        
        public List<Armor> GetArmorsByType(ArmorType type)
        {
            return Armors.Where(a => a.Type == type).ToList();
        }
    }
}
