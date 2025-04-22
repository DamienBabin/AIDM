// DnDAdventure.Core/Models/World.cs
using System.Text.Json;
using System.Text.Json.Serialization;
using DnDAdventure.Core.Extensions;

namespace DnDAdventure.Core.Models
{
    /// <summary>
    /// Represents the entire game world, including all characters, game states, and world data
    /// </summary>
    public class World
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Default World";
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSavedAt { get; set; } = DateTime.UtcNow;
        
        // Collections of game entities
        public Dictionary<Guid, Character> Characters { get; set; } = new();
        public Dictionary<Guid, GameState> GameStates { get; set; } = new();
        public Dictionary<int, AdventureNode> AdventureNodes { get; set; } = new();
        public Dictionary<Guid, NPC> NPCs { get; set; } = new();

        // World-specific data
        public Dictionary<string, string> Locations { get; set; } = new();
        public Dictionary<string, string> Quests { get; set; } = new();
        public Dictionary<string, object> CustomData { get; set; } = new();
        
        // Equipment data
        [JsonIgnore]
        private EquipmentList _equipmentList = new EquipmentList();
        
        // Dictionary to store equipment assignments for characters
        public Dictionary<Guid, CharacterEquipmentData> CharacterEquipment { get; set; } = new();

        // Collection of all world maps
        public Dictionary<Guid, WorldMap> Maps { get; set; } = new();

        // Collection of points of interest
        public Dictionary<Guid, PointOfInterest> PointsOfInterest { get; set; } = new();

        // Collection of structures
        public Dictionary<Guid, MapStructure> Structures { get; set; } = new();

        // Current map and position for each player
        public Dictionary<Guid, PlayerMapPosition> PlayerPositions { get; set; } = new();

        /// <summary>
        /// A serializable version of CharacterEquipment to avoid JsonIgnore issues
        /// </summary>
        public class CharacterEquipmentData
        {
            public Guid CharacterId { get; set; }
            public string? WeaponSlot { get; set; }
            public string? OffhandSlot { get; set; }
            public string? ArmorSlot { get; set; }
        }

        // Helper class to track player positions
        public class PlayerMapPosition
        {
            public Guid CharacterId { get; set; }
            public Guid CurrentMapId { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
        /// <summary>
        /// Saves the current world state to a JSON file
        /// </summary>
        /// <param name="filePath">Path where the JSON file will be saved</param>
        /// <returns>True if save succeeded, false otherwise</returns>
        public bool SaveToJson(string filePath)
        {
            try
            {
                // Update timestamps before saving
                LastSavedAt = DateTime.UtcNow;

                // Prepare character equipment data for serialization
                PrepareEquipmentDataForSerialization();

                // Configure JSON options
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                
                // Serialize and write to file
                string jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(filePath, jsonString);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving world to JSON: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Loads a world from a JSON file
        /// </summary>
        /// <param name="filePath">Path to the JSON file</param>
        /// <returns>The loaded World object, or null if loading failed</returns>
        public static World? LoadFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Save file not found: {filePath}");
                return null;
            }

                string jsonString = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
            {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var world = JsonSerializer.Deserialize<World>(jsonString, options);

                if (world != null)
                {
                    // Restore equipment references
                    world.RestoreEquipmentReferences();
        }
        
                return world;
            }
            catch (Exception ex)
        {
                Console.WriteLine($"Error loading world from JSON: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Creates a quick save with a timestamped filename
        /// </summary>
        /// <param name="saveDirectory">Directory where saves are stored</param>
        /// <returns>The path to the saved file, or null if saving failed</returns>
        public string? CreateQuickSave(string saveDirectory)
        {
            try
                {
                // Ensure directory exists
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
            }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"{Name.Replace(" ", "_")}_{timestamp}.json";
                string fullPath = Path.Combine(saveDirectory, filename);

                if (SaveToJson(fullPath))
        {
                    return fullPath;
        }
        
                return null;
            }
            catch (Exception ex)
        {
                Console.WriteLine($"Error creating quick save: {ex.Message}");
                return null;
        }
        }
        
        /// <summary>
        /// Gets a list of all available save files in the given directory
        /// </summary>
        /// <param name="saveDirectory">Directory to search for save files</param>
        /// <returns>List of save file paths</returns>
        public static List<string> GetAvailableSaves(string saveDirectory)
        {
            if (!Directory.Exists(saveDirectory))
            {
                return new List<string>();
            }
            
            return Directory.GetFiles(saveDirectory, "*.json")
                .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                .ToList();
            }
            
        /// <summary>
        /// Adds a character to the world and initializes its equipment
        /// </summary>
        /// <param name="character">The character to add</param>
        public void AddCharacter(Character character)
        {
            Characters[character.Id] = character;

            // Initialize character equipment
                if (!CharacterEquipment.ContainsKey(character.Id))
                {
                    CharacterEquipment[character.Id] = new CharacterEquipmentData
                    {
                        CharacterId = character.Id
                    };
                }
            }
        /// <summary>
        /// Adds a game state to the world
        /// </summary>
        /// <param name="gameState">The game state to add</param>
        public void AddGameState(GameState gameState)
        {
            GameStates[gameState.Id] = gameState;
        }

        /// <summary>
        /// Adds an adventure node to the world
        /// </summary>
        /// <param name="node">The adventure node to add</param>
        public void AddAdventureNode(AdventureNode node)
        {
            AdventureNodes[node.Id] = node;
    }

        /// <summary>
        /// Equips a weapon for a character
        /// </summary>
        /// <param name="characterId">ID of the character</param>
        /// <param name="weaponName">Name of the weapon to equip</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool EquipWeapon(Guid characterId, string weaponName)
        {
            if (!Characters.TryGetValue(characterId, out var character))
                return false;

            var weapon = _equipmentList.GetWeaponByName(weaponName);
            if (weapon == null)
                return false;

            if (!CharacterEquipment.TryGetValue(characterId, out var equipData))
            {
                equipData = new CharacterEquipmentData { CharacterId = characterId };
                CharacterEquipment[characterId] = equipData;
}

            // Apply equipment rules
            if (weapon.Properties.Contains(WeaponProperty.TwoHanded))
            {
                equipData.OffhandSlot = null;
            }

            equipData.WeaponSlot = weaponName;

            // Add to inventory if not already present
            if (!character.Inventory.Contains(weaponName))
            {
                character.Inventory.Add(weaponName);
            }

            return true;
        }

        /// <summary>
        /// Equips armor for a character
        /// </summary>
        /// <param name="characterId">ID of the character</param>
        /// <param name="armorName">Name of the armor to equip</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool EquipArmor(Guid characterId, string armorName)
        {
            if (!Characters.TryGetValue(characterId, out var character))
                return false;

            var armor = _equipmentList.GetArmorByName(armorName);
            if (armor == null || armor.Type == ArmorType.Shield)
                return false;

            if (!CharacterEquipment.TryGetValue(characterId, out var equipData))
            {
                equipData = new CharacterEquipmentData { CharacterId = characterId };
                CharacterEquipment[characterId] = equipData;
            }

            equipData.ArmorSlot = armorName;

            // Add to inventory if not already present
            if (!character.Inventory.Contains(armorName))
            {
                character.Inventory.Add(armorName);
            }

            return true;
        }

        /// <summary>
        /// Calculates armor class for a character based on their equipment and attributes
        /// </summary>
        /// <param name="characterId">ID of the character</param>
        /// <returns>The calculated armor class value</returns>
        public int CalculateArmorClass(Guid characterId)
        {
            if (!Characters.TryGetValue(characterId, out var character))
                return 10; // Default AC

            if (!CharacterEquipment.TryGetValue(characterId, out var equipData))
                return 10 + CalculateAbilityModifier(character.Attributes.GetValueOrDefault("Dexterity", 10));

            // Get Dexterity modifier
            int dexMod = CalculateAbilityModifier(character.Attributes.GetValueOrDefault("Dexterity", 10));

            int baseAC = 10; // Unarmored AC base
            int shieldBonus = 0;

            // Check for equipped shield
            if (!string.IsNullOrEmpty(equipData.OffhandSlot))
            {
                var shield = _equipmentList.GetArmorByName(equipData.OffhandSlot);
                if (shield != null && shield.Type == ArmorType.Shield)
                {
                    shieldBonus = shield.ArmorClass;
                }
            }

            // Calculate AC based on armor
            if (!string.IsNullOrEmpty(equipData.ArmorSlot))
            {
                var armor = _equipmentList.GetArmorByName(equipData.ArmorSlot);
                if (armor != null)
                {
                    baseAC = armor.ArmorClass;

                    // Add limited Dex modifier for certain armor types
                    if (armor.AddDexModifier)
                    {
                        if (armor.MaxDexModifier.HasValue)
                        {
                            dexMod = Math.Min(dexMod, armor.MaxDexModifier.Value);
                        }

                        baseAC += dexMod;
                    }
                }
            }
            else
            {
                // Unarmored - use base AC + full Dex mod
                baseAC += dexMod;
            }

            return baseAC + shieldBonus;
        }

        /// <summary>
        /// Adds a map to the world
        /// </summary>
        public void AddMap(WorldMap map)
        {
            Maps[map.Id] = map;
        }

        /// <summary>
        /// Gets a map by its ID
        /// </summary>
        public WorldMap? GetMap(Guid mapId)
        {
            Maps.TryGetValue(mapId, out var map);
            return map;
        }

        /// <summary>
        /// Gets all the maps in the world
        /// </summary>
        public List<WorldMap> GetAllMaps()
        {
            return Maps.Values.ToList();
        }

        /// <summary>
        /// Adds a point of interest to the world
        /// </summary>
        public void AddPointOfInterest(PointOfInterest poi)
        {
            PointsOfInterest[poi.Id] = poi;
        }

        /// <summary>
        /// Adds a structure to the world
        /// </summary>
        public void AddStructure(MapStructure structure)
        {
            Structures[structure.Id] = structure;
        }

        /// <summary>
        /// Sets a player's position on a map
        /// </summary>
        public void SetPlayerPosition(Guid characterId, Guid mapId, int x, int y)
        {
            // Get the map
            var map = GetMap(mapId);
            if (map == null)
                throw new KeyNotFoundException($"Map with ID {mapId} not found");

            // Validate coordinates
            if (x < 0 || x >= 9 || y < 0 || y >= 9)
                throw new ArgumentOutOfRangeException("Coordinates must be within the 9x9 grid");

            // Clear player flag from previous position if it exists
            if (PlayerPositions.TryGetValue(characterId, out var oldPosition))
            {
                if (Maps.TryGetValue(oldPosition.CurrentMapId, out var oldMap))
                {
                    oldMap.Grid[oldPosition.X, oldPosition.Y].HasPlayer = false;
                }
            }

            // Set new position
            PlayerPositions[characterId] = new PlayerMapPosition
            {
                CharacterId = characterId,
                CurrentMapId = mapId,
                X = x,
                Y = y
            };

            // Update the map cell
            map.Grid[x, y].HasPlayer = true;
        }

        /// <summary>
        /// Gets a player's current map position
        /// </summary>
        public PlayerMapPosition? GetPlayerPosition(Guid characterId)
        {
            PlayerPositions.TryGetValue(characterId, out var position);
            return position;
        }

        /// <summary>
        /// Gets the cell a player is currently standing on
        /// </summary>
        public MapCell? GetPlayerCell(Guid characterId)
        {
            var position = GetPlayerPosition(characterId);
            if (position == null)
                return null;

            var map = GetMap(position.CurrentMapId);
            if (map == null)
                return null;

            return map.Grid[position.X, position.Y];
        }

        /// <summary>
        /// Gets all characters on a specific map
        /// </summary>
        public List<Character> GetCharactersOnMap(Guid mapId)
        {
            var charactersOnMap = PlayerPositions
                .Where(p => p.Value.CurrentMapId == mapId)
                .Select(p => p.Key)
                .ToList();

            return Characters
                .Where(c => charactersOnMap.Contains(c.Key))
                .Select(c => c.Value)
                .ToList();
        }

        /// <summary>
        /// Calculates ability modifier from an ability score
        /// </summary>
        private static int CalculateAbilityModifier(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }

        /// <summary>
        /// Prepares equipment data for serialization
        /// </summary>
        private void PrepareEquipmentDataForSerialization()
        {
            // Ensure all characters with equipment have entries in the CharacterEquipment dictionary
            foreach (var character in Characters.Values)
            {
                if (!CharacterEquipment.ContainsKey(character.Id))
                {
                    CharacterEquipment[character.Id] = new CharacterEquipmentData
                    {
                        CharacterId = character.Id
                    };
                }
            }
        }

        /// <summary>
        /// Restores equipment references after deserialization
        /// </summary>
        private void RestoreEquipmentReferences()
        {
            // Recreate equipment list
            _equipmentList = new EquipmentList();
        }
    }
}

