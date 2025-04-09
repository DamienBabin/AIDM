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
                Inventory = new List<NPCItem>
                {
                    new NPCItem
                    {
                        Name = "Healing Potion",
                        Type = ItemType.Potion,
                        Value = 50,
                        IsTradeable = true
                    },
                    new NPCItem
                    {
                        Name = "Tavern Key",
                        Type = ItemType.QuestItem,
                        Value = 0,
                        IsTradeable = false,
                        IsQuestItem = true,
                        RelatedQuestId = "tavern_cellar_quest"
                    }
                },
                Dialogs = new List<NPCDialog>
                {
                    new NPCDialog
                    {
                        Id = Guid.NewGuid(),
                        Topic = "Greeting",
                        Text = "Welcome to The Drunken Dragon! What can I get for you, traveler?",
                        Options = new List<NPCDialogOption>
                        {
                            new NPCDialogOption
                            {
                                Text = "I'd like a room for the night.",
                                NextDialogId = Guid.NewGuid() // We'd set this properly in a real implementation
                            },
                            new NPCDialogOption
                            {
                                Text = "What's the latest gossip around town?",
                                NextDialogId = Guid.NewGuid()
                            },
                            new NPCDialogOption
                            {
                                Text = "Have you heard about any work available?",
                                NextDialogId = Guid.NewGuid()
                            }
                        }
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
                Inventory = new List<NPCItem>
                {
                    new NPCItem
                    {
                        Name = "Iron Sword",
                        Type = ItemType.Weapon,
                        Value = 100,
                        IsTradeable = true
                    },
                    new NPCItem
                    {
                        Name = "Leather Armor",
                        Type = ItemType.Armor,
                        Value = 75,
                        IsTradeable = true
                    }
                },
                Dialogs = new List<NPCDialog>
                {
                    new NPCDialog
                    {
                        Id = Guid.NewGuid(),
                        Topic = "Greeting",
                        Text = "Ye looking for quality steel? Ye've come to the right place.",
                        Options = new List<NPCDialogOption>
                        {
                            new NPCDialogOption
                            {
                                Text = "Show me what you have for sale.",
                                NextDialogId = Guid.NewGuid()
                            },
                            new NPCDialogOption
                            {
                                Text = "I'm looking for something special.",
                                NextDialogId = Guid.NewGuid(),
                                Requirements = new Dictionary<string, string>
                                {
                                    { "MinLevel", "2" }
                                }
                            }
                        }
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

