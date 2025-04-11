// DnDAdventure.Core/models/World.cs
using System;
using System.Collections.Generic;
namespace DnDAdventure.Core.Models
{
    public class World
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Collections of game objects
        public Dictionary<Guid, Character> Characters { get; set; } = new();
        public Dictionary<Guid, GameState> GameStates { get; set; } = new();
        public Dictionary<Guid, NPC> NPCs { get; set; } = new();
        public Dictionary<Guid, PointOfInterest> PointsOfInterest { get; set; } = new();
        public Dictionary<Guid, Structure> Structures { get; set; } = new();
        public Dictionary<Guid, Map> Maps { get; set; } = new();

        // Player positions
        public List<PlayerPosition> PlayerPositions { get; set; } = new();
        // Character equipment data
        public List<CharacterEquipmentData> EquipmentData { get; set; } = new();

        // Methods referenced in other services
        public PlayerPosition GetPlayerPosition(Guid characterId)
        {
            return PlayerPositions.FirstOrDefault(p => p.CharacterId == characterId);
        }

        public Map GetMap(Guid mapId)
        {
            if (Maps.TryGetValue(mapId, out var map))
            return map;
                return null;
        }

        public void SetPlayerPosition(Guid characterId, Guid mapId, int x, int y)
        {
            var position = PlayerPositions.FirstOrDefault(p => p.CharacterId == characterId);
            if (position != null)
        {
                position.CurrentMapId = mapId;
                position.X = x;
                position.Y = y;
            }
            else
        {
                PlayerPositions.Add(new PlayerPosition
        {
                    CharacterId = characterId,
                    CurrentMapId = mapId,
                    X = x,
                    Y = y
                });
            }
        }

        public class PlayerPosition
            {
            public Guid CharacterId { get; set; }
            public Guid CurrentMapId { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class CharacterEquipmentData
                {
            public Guid CharacterId { get; set; }
            public string? WeaponSlot { get; set; }
            public string? OffhandSlot { get; set; }
            public string? ArmorSlot { get; set; }
                }
            }
        }

