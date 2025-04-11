// DnDAdventure.Core/models/Enums.cs
namespace DnDAdventure.Core.Models
{
    public enum ArmorType
    {
        Light,
        Medium, 
        Heavy,
        Shield
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
        Finesse,
        Light,
        TwoHanded,
        Versatile,
        Heavy,
        Reach,
        Thrown,
        Ammunition,
        Loading,
        Special
    }
    
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}