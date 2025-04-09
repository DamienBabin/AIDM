// DnDAdventure.Infrastructure/Services/MapInitializer.cs
using DnDAdventure.Core.Models;
using System;
using System.Collections.Generic;

namespace DnDAdventure.Infrastructure.Services
{
    public class MapInitializer
    {
        private readonly WorldService _worldService;
        
        public MapInitializer(WorldService worldService)
        {
            _worldService = worldService;
        }
        
        /// <summary>
        /// Initializes the default maps for a new world
        /// </summary>
        public void InitializeDefaultMaps()
        {
            var world = _worldService.CurrentWorld;
            
            // Create village map
            var villageMap = new WorldMap
            {
                Id = Guid.NewGuid(),
                Name = "Northaven Village",
                Description = "A peaceful farming village nestled between rolling hills and a small forest.",
                WorldX = 0,
                WorldY = 0
            };
            
            PopulateVillageMap(villageMap);
            world.AddMap(villageMap);
            
            // Create forest map
            var forestMap = new WorldMap
            {
                Id = Guid.NewGuid(),
                Name = "Whispering Woods",
                Description = "An ancient forest with towering trees and the occasional clearing. Strange sounds echo between the trees.",
                WorldX = 0,
                WorldY = 1
            };
            
            PopulateForestMap(forestMap);
            world.AddMap(forestMap);
            
            // Create mountains map
            var mountainsMap = new WorldMap
            {
                Id = Guid.NewGuid(),
                Name = "Frost Peak Mountains",
                Description = "Rugged mountains with snow-capped peaks. Narrow paths wind between rocky outcroppings.",
                WorldX = 1,
                WorldY = 0
            };
            
            PopulateMountainsMap(mountainsMap);
            world.AddMap(mountainsMap);
            
            // Connect the maps
            villageMap.ConnectedMaps[Direction.North] = forestMap.Id;
            villageMap.ConnectedMaps[Direction.East] = mountainsMap.Id;
            
            forestMap.ConnectedMaps[Direction.South] = villageMap.Id;
            
            mountainsMap.ConnectedMaps[Direction.West] = villageMap.Id;
        }
        
        private void PopulateVillageMap(WorldMap map)
        {
            var world = _worldService.CurrentWorld;
            
            // Set up basic terrain
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    map.Grid[x, y].TerrainType = TerrainType.Plains;
                    map.Grid[x, y].Passable = true;
                }
            }
            
            // Add roads
            for (int x = 0; x < 9; x++)
            {
                map.Grid[x, 4].TerrainType = TerrainType.Road;
                map.Grid[x, 4].Description = "A well-traveled dirt road running east to west through the village.";
            }
            
            for (int y = 0; y < 9; y++)
            {
                map.Grid[4, y].TerrainType = TerrainType.Road;
                map.Grid[4, y].Description = "A well-traveled dirt road running north to south through the village.";
            }
            
            // Village center (town square)
            map.Grid[4, 4].Name = "Village Square";
            map.Grid[4, 4].Description = "The central square of Northaven. A notice board stands in the middle, and villagers go about their daily business.";
            
            // Tavern
            var tavernStructure = new MapStructure
            {
                Id = Guid.NewGuid(),
                Name = "The Golden Tankard",
                Description = "A lively tavern with a thatched roof. The smell of ale and stew wafts from inside.",
                Type = StructureType.Tavern
            };
            world.AddStructure(tavernStructure);
            
            map.Grid[2, 3].Name = "Tavern";
            map.Grid[2, 3].Description = "The Golden Tankard tavern stands here, with a wooden sign depicting a foaming mug.";
            map.Grid[2, 3].StructureId = tavernStructure.Id;
            map.Grid[2, 3].TerrainType = TerrainType.Plains;
            
            // Blacksmith
            var smithyStructure = new MapStructure
            {
                Id = Guid.NewGuid(),
                Name = "Village Smithy",
                Description = "A sturdy stone building with a large forge. The sound of hammering can be heard from inside.",
                Type = StructureType.Shop
            };
            world.AddStructure(smithyStructure);
            
            map.Grid[6, 3].Name = "Blacksmith";
            map.Grid[6, 3].Description = "The village smithy stands here, smoke rising from its chimney.";
            map.Grid[6, 3].StructureId = smithyStructure.Id;
            
            // General store
            var storeStructure = new MapStructure
            {
                Id = Guid.NewGuid(),
                Name = "General Store",
                Description = "A wooden building with a variety of goods displayed in the windows.",
                Type = StructureType.Shop
            };
            world.AddStructure(storeStructure);
            
            map.Grid[3, 6].Name = "General Store";
            map.Grid[3, 6].Description = "The village's general store, selling all manner of goods.";
            map.Grid[3, 6].StructureId = storeStructure.Id;
            
            // Temple
            var templeStructure = new MapStructure
            {
                Id = Guid.NewGuid(),
                Name = "Temple of Light",
                Description = "A small but well-maintained temple dedicated to the gods of light and healing.",
                Type = StructureType.Temple
            };
            world.AddStructure(templeStructure);
            
            map.Grid[5, 6].Name = "Temple";
            map.Grid[5, 6].Description = "A stone temple with stained glass windows that catch the light.";
            map.Grid[5, 6].StructureId = templeStructure.Id;
            
            // Add NPCs
            
            // Tavern keeper
            var tavernKeeper = new NPC
            {
                Id = Guid.NewGuid(),
                Name = "Barley Goldfoam",
                Race = "Human",
                Occupation = "Tavern Keeper",
                Description = "A portly man with a bushy mustache and a jolly demeanor.",
                CurrentLocation = "Northaven Village"
            };
            
            world.AddNPC(tavernKeeper);
            map.Grid[2, 3].NPCId = tavernKeeper.Id;
            
            // Blacksmith
            var blacksmith = new NPC
            {
                Id = Guid.NewGuid(),
                Name = "Grimhilda Ironhammer",
                Race = "Dwarf",
                Occupation = "Blacksmith",
                Description = "A stout dwarven woman with muscular arms and soot-stained apron.",
                CurrentLocation = "Northaven Village"
            };
            
            world.AddNPC(blacksmith);
            map.Grid[6, 3].NPCId = blacksmith.Id;
            
            // Add Points of Interest
            
            // Village well
            var wellPOI = new PointOfInterest
            {
                Id = Guid.NewGuid(),
                Name = "Village Well",
                Description = "A stone well in the village square. The water here is clean and refreshing.",
                Type = POIType.Landmark,
                AvailableActions = new List<POIAction>
                {
                    new POIAction
                    {
                        Name = "Drink",
                        Description = "You drink from the well. The water is cool and refreshing.",
                        Effects = new Dictionary<string, string>
                        {
                            { "RestoreHealth", "1" }
                        }
                    }
                }
            };
            
            world.AddPointOfInterest(wellPOI);
            map.Grid[4, 5].Name = "Village Well";
            map.Grid[4, 5].Description = "A stone well with a wooden bucket and rope.";
            map.Grid[4, 5].PointOfInterestId = wellPOI.Id;
            
            // Notice board
            var noticeBoardPOI = new PointOfInterest
            {
                Id = Guid.NewGuid(),
                Name = "Notice Board",
                Description = "A wooden board with various notices and job postings pinned to it.",
                Type = POIType.Quest,
                AvailableActions = new List<POIAction>
                {
                    new POIAction
                    {
                        Name = "Read Notices",
                        Description = "You read the notices on the board. There are several requests for help from the villagers.",
                        Effects = new Dictionary<string, string>
                        {
                            { "AddQuest", "MissingShipment" }
                        }
                    }
                }
            };
            
            world.AddPointOfInterest(noticeBoardPOI);
            map.Grid[4, 4].PointOfInterestId = noticeBoardPOI.Id;
        }
        
        private void PopulateForestMap(WorldMap map)
        {
            var world = _worldService.CurrentWorld;
            
            // Set up basic forest terrain
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    map.Grid[x, y].TerrainType = TerrainType.Forest;
                    map.Grid[x, y].Passable = true;
                    map.Grid[x, y].Description = "Dense trees surround you, with dappled sunlight filtering through the canopy.";
                }
            }
            
            // Add a path through the forest
            for (int y = 8; y >= 0; y--)
            {
                map.Grid[4, y].TerrainType = TerrainType.Road;
                map.Grid[4, y].Description = "A narrow dirt path winds through the forest.";
            }
            
            // Add some clearings
            map.Grid[2, 3].TerrainType = TerrainType.Plains;
            map.Grid[2, 3].Name = "Forest Clearing";
            map.Grid[2, 3].Description = "A small clearing in the forest. Wildflowers grow here in abundance.";
            
            map.Grid[6, 5].TerrainType = TerrainType.Plains;
            map.Grid[6, 5].Name = "Ancient Circle";
            map.Grid[6, 5].Description = "A clearing with a circle of standing stones. They seem very old.";
            
            // Add Points of Interest
            
            // Hunter's camp
            var huntersCampPOI = new PointOfInterest
            {
                Id = Guid.NewGuid(),
                Name = "Hunter's Camp",
                Description = "A small campsite with a fire pit and a leather tent.",
                Type = POIType.Landmark,
                AvailableActions = new List<POIAction>
                {
                    new POIAction
                    {
                        Name = "Rest",
                        Description = "You rest at the hunter's camp and recover your strength.",
                        Effects = new Dictionary<string, string>
                        {
                            { "RestoreHealth", "5" }
                        }
                    },
                    new POIAction
                    {
                        Name = "Search",
                        Description = "You search the camp and find a small hunting knife.",
                        Effects = new Dictionary<string, string>
                        {
                            { "AddItem", "Hunting Knife" }
                        },
                        Requirements = new Dictionary<string, string>
                        {
                            { "NotSearched", "HuntersCamp" }
                        }
                    }
                }
            };
            
            world.AddPointOfInterest(huntersCampPOI);
            map.Grid[1, 4].Name = "Hunter's Camp";
            map.Grid[1, 4].Description = "A small camp with a fire pit and a leather tent.";
            map.Grid[1, 4].PointOfInterestId = huntersCampPOI.Id;
            
            // Ancient standing stones
            var standingStonesPOI = new PointOfInterest
            {
                Id = Guid.NewGuid(),
                Name = "Standing Stones",
                Description = "A circle of ancient standing stones covered in mysterious runes.",
                Type = POIType.Puzzle,
                AvailableActions = new List<POIAction>
                {
                    new POIAction
                    {
                        Name = "Examine Runes",
                        Description = "You examine the runes on the stones. They appear to be some form of ancient magic.",
                        Requirements = new Dictionary<string, string>
                        {
                            { "Skill", "Arcana" }
                        }
                    },
                    new POIAction
                    {
                        Name = "Place Offering",
                        Description = "You place an offering at the stones. There's a brief flash of light, and you feel slightly more attuned to magic.",
                        Effects = new Dictionary<string, string>
                        {
                            { "AddBonus", "MagicResistance" }
                        },
                        Requirements = new Dictionary<string, string>
                        {
                            { "HasItem", "Silver Coin" }
                        }
                    }
                }
            };
            
            world.AddPointOfInterest(standingStonesPOI);
            map.Grid[6, 5].PointOfInterestId = standingStonesPOI.Id;
            
            // Add NPCs
            
            // Forest ranger
            var ranger = new NPC
            {
                Id = Guid.NewGuid(),
                Name = "Sylvan Thornwalker",
                Race = "Elf",
                Occupation = "Forest Ranger",
                Description = "A slender elf with green clothing and a longbow. He seems at home in the forest.",
                CurrentLocation = "Whispering Woods"
            };
            
            world.AddNPC(ranger);
            map.Grid[4, 2].NPCId = ranger.Id;
            map.Grid[4, 2].Name = "Ranger's Post";
            map.Grid[4, 2].Description = "A small wooden platform built around a large tree. A perfect vantage point for watching the forest.";
        }
        
        private void PopulateMountainsMap(WorldMap map)
        {
            // Implementation similar to the other map population methods
            // This would set up mountain terrain, paths, etc.
        }
    }
}
