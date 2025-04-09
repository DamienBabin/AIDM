// DnDAdventure.Core/Models/CharacterEquipment.cs
using System.Text.Json.Serialization;

namespace DnDAdventure.Core.Models
{
    public class CharacterEquipment
    {
        public Guid CharacterId { get; set; }
        
        // Equipment slots
        public string? WeaponSlot { get; set; }
        public string? OffhandSlot { get; set; } // Could be a shield or second weapon
        public string? ArmorSlot { get; set; }
        
        // Equipped item details (for easier access without lookup)
        [JsonIgnore]
        public Weapon? EquippedWeapon { get; private set; }
        
        [JsonIgnore]
        public Weapon? EquippedOffhand { get; private set; }
        
        [JsonIgnore]
        public Armor? EquippedArmor { get; private set; }
        
        // Reference to equipment list for lookup
        [JsonIgnore]
        private readonly EquipmentList _equipmentList;
        
        public CharacterEquipment(Guid characterId, EquipmentList equipmentList)
        {
            CharacterId = characterId;
            _equipmentList = equipmentList;
        }
        
        // Equipment methods
        public bool EquipWeapon(string weaponName)
        {
            var weapon = _equipmentList.GetWeaponByName(weaponName);
            if (weapon == null) return false;
            
            // If two-handed, can't have offhand
            if (weapon.Properties.Contains(WeaponProperty.TwoHanded) && !string.IsNullOrEmpty(OffhandSlot))
            {
                OffhandSlot = null;
                EquippedOffhand = null;
            }
            
            WeaponSlot = weaponName;
            EquippedWeapon = weapon;
            return true;
        }
        
        public bool EquipOffhand(string itemName)
        {
            // Check if it's a weapon
            var weapon = _equipmentList.GetWeaponByName(itemName);
            
            if (weapon != null)
            {
                // Can't equip two-handed weapons in offhand
                if (weapon.Properties.Contains(WeaponProperty.TwoHanded))
                    return false;
                
                // Can't have offhand if main weapon is two-handed
                if (EquippedWeapon != null && EquippedWeapon.Properties.Contains(WeaponProperty.TwoHanded))
                    return false;
                
                // Light weapons only in offhand unless you have the Dual Wielder feat
                if (!weapon.Properties.Contains(WeaponProperty.Light))
                    return false;
                
                OffhandSlot = itemName;
                EquippedOffhand = weapon;
                return true;
            }
            
            // Check if it's a shield
            var shield = _equipmentList.GetArmorByName(itemName);
            
            if (shield != null && shield.Type == ArmorType.Shield)
            {
                // Can't have shield if main weapon is two-handed
                if (EquippedWeapon != null && EquippedWeapon.Properties.Contains(WeaponProperty.TwoHanded))
                    return false;
                
                OffhandSlot = itemName;
                EquippedOffhand = null; // Not a weapon
                return true;
            }
            
            return false;
        }
        
        public bool EquipArmor(string armorName)
        {
            var armor = _equipmentList.GetArmorByName(armorName);
            if (armor == null || armor.Type == ArmorType.Shield) return false;
            
            ArmorSlot = armorName;
            EquippedArmor = armor;
            return true;
        }
        
        // Calculate armor class based on equipment and attributes
        public int CalculateArmorClass(Dictionary<string, int> attributes)
        {
            // Get Dexterity modifier
            int dexMod = (attributes.ContainsKey("Dexterity") ? attributes["Dexterity"] : 10) - 10;
            dexMod = dexMod / 2; // Integer division rounds down
            
            int baseAC = 10; // Unarmored AC base
            int shieldBonus = 0;
            
            // Check for equipped shield
            if (!string.IsNullOrEmpty(OffhandSlot))
            {
                var shield = _equipmentList.GetArmorByName(OffhandSlot);
                if (shield != null && shield.Type == ArmorType.Shield)
                {
                    shieldBonus = shield.ArmorClass;
                }
            }
            
            // Calculate AC based on armor
            if (EquippedArmor != null)
            {
                baseAC = EquippedArmor.ArmorClass;
                
                // Add limited Dex modifier for certain armor types
                if (EquippedArmor.AddDexModifier)
                {
                    if (EquippedArmor.MaxDexModifier.HasValue)
                    {
                        dexMod = Math.Min(dexMod, EquippedArmor.MaxDexModifier.Value);
                    }
                    
                    baseAC += dexMod;
                }
                
                // Heavy armor doesn't add Dex at all (already handled by AddDexModifier = false)
            }
            else
            {
                // Unarmored - use base AC + full Dex mod
                baseAC += dexMod;
            }
            
            return baseAC + shieldBonus;
        }
        
        // Load equipment state after deserialization
        public void LoadEquipmentData()
        {
            if (!string.IsNullOrEmpty(WeaponSlot))
            {
                EquippedWeapon = _equipmentList.GetWeaponByName(WeaponSlot);
            }
            
            if (!string.IsNullOrEmpty(OffhandSlot))
            {
                EquippedOffhand = _equipmentList.GetWeaponByName(OffhandSlot);
            }
            
            if (!string.IsNullOrEmpty(ArmorSlot))
            {
                EquippedArmor = _equipmentList.GetArmorByName(ArmorSlot);
            }
        }
    }
}
