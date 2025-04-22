// DnDAdventure.Infrastructure/Services/WorldInitializer.cs
using DnDAdventure.Core.Models;
using System;
using System.Collections.Generic;

namespace DnDAdventure.Infrastructure.Services
{
    public class WorldInitializer
    {
        private readonly WorldService _worldService;
        
        public WorldInitializer(WorldService worldService)
        {
            _worldService = worldService;
        }
        
        /// <summary>
        /// Initializes a new world with default content including NPCs
        /// </summary>
        /// <param name="worldName">Name of the new world</param>
        /// <returns>The initialized world</returns>
        public World InitializeNewWorld(string worldName)
        {
            var world = _worldService.CreateNewWorld(worldName);
            
            // Initialize locations
            InitializeLocations(world);
            
            // Initialize NPCs and place them in locations
            InitializeNPCs(world);
            
            // Initialize any other world content
            InitializeItems(world);
            InitializeQuests(world);
            
            return world;
        }
        
        private void InitializeLocations(World world)
        {
            // Add key locations to the world
            world.Locations["village_square"] = "The central square of Elmridge, a small village with cobblestone streets and wooden buildings.";
            world.Locations["tavern"] = "The Drunken Dragon, a lively tavern with a crackling fireplace and the smell of ale and stew.";
            world.Locations["blacksmith"] = "Forge & Anvil, a hot smithy where the sound of hammering metal rings out.";
            world.Locations["forest_path"] = "A winding path through the dense Whispering Woods, sunlight filtering through the canopy.";
            world.Locations["abandoned_mine"] = "An old mine entrance, boards covering the opening with warning signs of danger.";
        }
        
        private void InitializeNPCs(World world)
        {
            // Create innkeeper NPC
            var innkeeper = new NPC
            {
                Id = Guid.NewGuid(),
                Name = "Barnaby Oakhart",
                Race = "Human",
                Occupation = "Innkeeper",
                Description = "A portly man with a bushy mustache and a jolly laugh. He knows all the local gossip.",
                CurrentLocation = "tavern",
                Disposition = 70,
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem
                    {
                        Name = "Healing Potion",
                        Value = 50,
                        IsTradeable = true
                    },
                    new InventoryItem
                    {
                        Name = "Tavern Key",
                        Value = 0,
                        IsTradeable = false
                    }
                }
            };
            
            // Create blacksmith NPC
            var blacksmith = new NPC
            {
                Id = Guid.NewGuid(),
                Name = "Grimhilda Steelhammer",
                Race = "Dwarf",
                Occupation = "Blacksmith",
                Description = "A stout dwarven woman with muscular arms and a soot-covered apron. Her braided beard has metal ornaments.",
                CurrentLocation = "blacksmith",
                Disposition = 50,
                Inventory = new List<InventoryItem>
                {
                    new InventoryItem
                    {
                        Name = "Iron Sword",
                        Value = 100,
                        IsTradeable = true
                    },
                    new InventoryItem
                    {
                        Name = "Leather Armor",
                        Value = 75,
                        IsTradeable = true
                    }
                }
            };
            
            // Add NPCs to the world
            world.AddNPC(innkeeper);
            world.AddNPC(blacksmith);
            
            // Add any other NPCs here
        }
        
        private void InitializeItems(World world)
        {
            // You would initialize any world items here
        }
        
        private void InitializeQuests(World world)
        {
            // You would initialize any available quests here
        }
    }
}

