// DnDAdventure.Core/Extensions/CharacterExtensions.cs
using DnDAdventure.Core.Models;

namespace DnDAdventure.Core.Extensions
{
    public static class CharacterExtensions
    {
        private static readonly Dictionary<Guid, CharacterEquipment> _characterEquipment = new();
        private static readonly EquipmentList _equipmentList = new();

        public static CharacterEquipment GetEquipment(this Character character)
        {
            if (!_characterEquipment.TryGetValue(character.Id, out var equipment))
            {
                equipment = new CharacterEquipment(character.Id, _equipmentList);
                _characterEquipment[character.Id] = equipment;
            }

            return equipment;
        }

        public static int GetArmorClass(this Character character)
        {
            var equipment = character.GetEquipment();
            return equipment.CalculateArmorClass(character.Attributes);
        }

        public static bool EquipWeapon(this Character character, string weaponName)
        {
            var equipment = character.GetEquipment();
            var result = equipment.EquipWeapon(weaponName);

            // Add to inventory if not already present
            if (result && !character.Inventory.Contains(weaponName))
            {
                character.Inventory.Add(weaponName);
            }

            return result;
        }

        public static bool EquipOffhand(this Character character, string itemName)
        {
            var equipment = character.GetEquipment();
            var result = equipment.EquipOffhand(itemName);

            // Add to inventory if not already present
            if (result && !character.Inventory.Contains(itemName))
            {
                character.Inventory.Add(itemName);
            }

            return result;
        }

        public static bool EquipArmor(this Character character, string armorName)
        {
            var equipment = character.GetEquipment();
            var result = equipment.EquipArmor(armorName);

            // Add to inventory if not already present
            if (result && !character.Inventory.Contains(armorName))
            {
                character.Inventory.Add(armorName);
            }

            return result;
        }

        // Helper for combat calculations
        public static string GetWeaponDamage(this Character character)
        {
            var equipment = character.GetEquipment();
            int strMod = (character.Attributes.ContainsKey("Strength") ? character.Attributes["Strength"] : 10) - 10;

            if (equipment.EquippedWeapon != null)
            {
                string baseDamage = equipment.EquippedWeapon.Damage;

                // Get ability modifier for damage bonus
                string abilityUsed = "Strength"; // Default for melee weapons

                // If it's a finesse weapon, can use Dex instead of Str if higher
                if (equipment.EquippedWeapon.Properties.Contains(WeaponProperty.Finesse))
                {
                    strMod = strMod / 2;

                    int dexMod = (character.Attributes.ContainsKey("Dexterity") ? character.Attributes["Dexterity"] : 10) - 10;
                    dexMod = dexMod / 2;

                    if (dexMod > strMod)
                    {
                        abilityUsed = "Dexterity";
                    }
                }

                // For ranged weapons, always use Dex
                if (equipment.EquippedWeapon.Type == WeaponType.SimpleRanged ||
                    equipment.EquippedWeapon.Type == WeaponType.MartialRanged)
                {
                    abilityUsed = "Dexterity";
                }

                // Calculate modifier
                int modifier = 0;
                if (character.Attributes.ContainsKey(abilityUsed))
                {
                    modifier = (character.Attributes[abilityUsed] - 10) / 2;
                }

                // Add damage bonus if positive
                if (modifier > 0)
                {
                    return $"{baseDamage} + {modifier} {equipment.EquippedWeapon.DamageType}";
                }
                else if (modifier < 0)
                {
                    return $"{baseDamage} - {Math.Abs(modifier)} {equipment.EquippedWeapon.DamageType}";
                }

                return $"{baseDamage} {equipment.EquippedWeapon.DamageType}";
            }

            // Unarmed strike damage
            // int unarmedStrMod  = (character.Attributes.ContainsKey("Strength") ? character.Attributes["Strength"] : 10) - 10;
            strMod = strMod / 2;

            if (strMod > 0)
            {
                return $"1 + {strMod} Bludgeoning";
            }
            else if (strMod < 0)
            {
                return $"1 - {Math.Abs(strMod)} Bludgeoning";
            }

            return "1 Bludgeoning";
        }
    }
}
