// DnDAdventure.Infrastructure/Services/GameService.cs
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Repositories;
using DnDAdventure.Core.Services;
using DnDAdventure.AI;

namespace DnDAdventure.Infrastructure.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<GameState> _gameStateRepository;
        private readonly IRepository<Character> _characterRepository;
        private readonly AdventureGenerator _adventureGenerator;
        private readonly IWorldService _worldService;
        private readonly MapInitializer _mapInitializer;

        public GameService(
            IRepository<GameState> gameStateRepository,
            IRepository<Character> characterRepository,
            AdventureGenerator adventureGenerator,
            IWorldService worldService,
            MapInitializer mapInitializer)
        {
            _gameStateRepository = gameStateRepository;
            _characterRepository = characterRepository;
            _adventureGenerator = adventureGenerator;
            _worldService = worldService;
            _mapInitializer = mapInitializer;
        }

        public async Task<Character> SaveCharacter(Character character)
        {
            // Ensure character has an ID
            if (character.Id == Guid.Empty)
            {
                character.Id = Guid.NewGuid();
            }
            
            await _characterRepository.AddAsync(character);
            return character;
        }

        public async Task<GameState> StartNewGame(Guid characterId, string? worldId, string? worldName, string? worldDescription)
        {
            // Verify character exists
            var character = await GetCharacterById(characterId);

            var startLocation = "Village of Northaven";
            Guid parsedWorldId;

            if (string.Equals(worldId, "starter-5e", StringComparison.OrdinalIgnoreCase))
            {
                var starterWorld = CreateBeginnerTutorialWorld();
                parsedWorldId = starterWorld.Id;
                startLocation = "Training Green";
            }
            else if (!string.IsNullOrWhiteSpace(worldId) && Guid.TryParse(worldId, out parsedWorldId))
            {
                // Use provided world ID
            }
            else
            {
                var newWorld = _worldService.CreateNewWorld(
                    string.IsNullOrWhiteSpace(worldName) ? "New Adventure World" : worldName,
                    worldDescription ?? string.Empty);
                parsedWorldId = newWorld.Id;
            }
            
            // Create initial game state
            var gameState = new GameState
            {
                Id = Guid.NewGuid(),
                CharacterId = characterId,
                WorldId = parsedWorldId,
                CurrentLocation = startLocation,
                CurrentStoryNode = 1 // Starting node
            };

            if (string.Equals(worldId, "starter-5e", StringComparison.OrdinalIgnoreCase))
            {
                gameState.ActiveQuests.Add("Learn the Core Loop");
                gameState.Flags["tutorial_started"] = true;
            }
            
            await _gameStateRepository.AddAsync(gameState);
            _worldService.AddCharacter(character);
            _worldService.AddGameState(gameState);
            EnsureMapPosition(characterId);
            return gameState;
        }

        public async Task<GameState> CreateNewGame(Character character)
        {
            // Save character
            await _characterRepository.AddAsync(character);
            
            // Create initial game state
            var gameState = new GameState
            {
                Id = Guid.NewGuid(),
                CharacterId = character.Id,
                CurrentLocation = "Village of Northaven",
                CurrentStoryNode = 1 // Starting node
            };
            
            await _gameStateRepository.AddAsync(gameState);
            _worldService.AddCharacter(character);
            _worldService.AddGameState(gameState);
            EnsureMapPosition(character.Id);
            return gameState;
        }

        public async Task<GameState> GetGameStateById(Guid id)
        {
            var gameState = await _gameStateRepository.GetByIdAsync(id);
            if (gameState == null)
            {
                throw new KeyNotFoundException($"Game state with ID {id} not found");
            }
            return gameState;
        }

        public async Task<Character> GetCharacterById(Guid id)
        {
            var character = await _characterRepository.GetByIdAsync(id);
            if (character == null)
            {
                throw new KeyNotFoundException($"Character with ID {id} not found");
            }
            return character;
        }

        // AI-powered adventure node generation
        public async Task<AdventureNode> GetCurrentNode(Guid gameStateId)
        {
            var gameState = await GetGameStateById(gameStateId);
            var character = await GetCharacterById(gameState.CharacterId);
            
            // Get NPCs at current location from world service
            var npcsAtLocation = await GetNPCsAtCurrentLocation(gameState);
            
            // Generate initial adventure node using AI
            var adventureNode = await _adventureGenerator.GenerateNextNode(
                character, 
                gameState, 
                "start adventure");
            
            return adventureNode;
        }

        public async Task<AdventureNode> ProcessChoice(Guid gameStateId, int choiceIndex)
        {
            var gameState = await GetGameStateById(gameStateId);
            var character = await GetCharacterById(gameState.CharacterId);
            
            // Get NPCs at current location from world service
            var npcsAtLocation = await GetNPCsAtCurrentLocation(gameState);
            
            // Get the current node to determine what choice was made
            var currentNode = await GetCurrentNode(gameStateId);
            
            if (choiceIndex < 0 || choiceIndex >= currentNode.Choices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(choiceIndex), "Invalid choice index");
            }
            
            var selectedChoice = currentNode.Choices[choiceIndex];
            
            // Apply choice effects to game state
            await ApplyChoiceEffects(gameState, selectedChoice);
            
            // Generate next adventure node based on the choice
            var nextNode = await _adventureGenerator.GenerateNextNode(
                character, 
                gameState, 
                selectedChoice.Text);
            
            // Update game state with new node
            gameState.CurrentStoryNode = nextNode.Id;
            await _gameStateRepository.UpdateAsync(gameState);
            
            return nextNode;
        }

        private Task<List<NPC>> GetNPCsAtCurrentLocation(GameState gameState)
        {
            try
            {
                // Try to get world and NPCs from world service
                var world = _worldService.CurrentWorld;
                if (world?.NPCs != null)
                {
                    return Task.FromResult(world.NPCs.Values.Where(npc => npc.CurrentLocation == gameState.CurrentLocation).ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting NPCs from world service: {ex.Message}");
            }
            
            // Return empty list if world service fails
            return Task.FromResult(new List<NPC>());
        }

        private World CreateBeginnerTutorialWorld()
        {
            var world = _worldService.CreateNewWorld(
                "First Steps: 5E Tutorial",
                "A guided beginner world for learning D&D 5E through short scenes: checks, saves, roleplay, combat basics, and rests.");

            world.Locations["Training Green"] = "A safe village practice field with chalk circles, straw targets, and patient mentors. New adventurers learn the core D&D 5E loop here: describe an action, roll when the outcome is uncertain, add the right modifier, and live with the result.";
            world.Locations["Oakbridge Village"] = "A small, friendly village where social choices matter. NPCs explain advantage, proficiency, inspiration, and how ability checks differ from attacks and saving throws.";
            world.Locations["Old Mill Road"] = "A low-risk exploration path used to teach marching order, perception, hazards, armor class, hit points, initiative, and short rests.";

            world.Quests["Learn the Core Loop"] = "Complete three beginner lessons: make an ability check, resolve a social scene, and survive a practice combat using D&D 5E basics.";
            world.Quests["Help Oakbridge"] = "Use your new skills to investigate harmless but mysterious trouble near the old mill.";

            world.CustomData["ruleset"] = "D&D 5E";
            world.CustomData["tutorialMode"] = true;
            world.CustomData["beginnerTips"] = new[]
            {
                "Ability checks use d20 + ability modifier + proficiency when trained.",
                "Attack rolls try to meet or beat Armor Class.",
                "Saving throws resist danger after something happens to you.",
                "Hit points track how much punishment you can take before falling unconscious.",
                "Advantage means roll two d20s and use the higher result; disadvantage uses the lower."
            };

            world.AddNPC(new NPC
            {
                Name = "Mira Quickstep",
                Race = "Halfling",
                Occupation = "Beginner Guide",
                Description = "A cheerful mentor who explains 5E rules in plain language and keeps the first adventure low pressure.",
                CurrentLocation = "Training Green",
                Disposition = 90,
                Dialogs = new List<string>
                {
                    "Tell me what you want to try. If success is uncertain, we roll a d20.",
                    "Your character sheet is not homework. It is a menu of things your hero is good at.",
                    "In 5E, the story decides when a roll matters. The dice decide how cleanly it goes."
                }
            });

            world.AddNPC(new NPC
            {
                Name = "Sergeant Bram",
                Race = "Human",
                Occupation = "Practice Master",
                Description = "A retired guard captain who teaches armor class, initiative, actions, bonus actions, and safe practice combat.",
                CurrentLocation = "Training Green",
                Disposition = 75,
                Dialogs = new List<string>
                {
                    "Roll initiative when timing matters and everyone acts in turns.",
                    "On your turn, think action, movement, and sometimes bonus action.",
                    "Armor Class is the number an attack roll needs to meet or beat."
                }
            });

            return world;
        }

        private void EnsureMapPosition(Guid characterId)
        {
            var world = _worldService.CurrentWorld;

            if (!world.Maps.Any())
            {
                _mapInitializer.InitializeDefaultMaps();
            }

            if (world.CustomData.TryGetValue("tutorialMode", out var tutorialMode) && tutorialMode is bool isTutorial && isTutorial)
            {
                ConfigureTutorialMap();
            }

            var startingMap = world.Maps.Values.FirstOrDefault(m => m.Name.Contains("Northaven"))
                ?? world.Maps.Values.FirstOrDefault();

            if (startingMap == null)
                return;

            var startX = 4;
            var startY = 4;
            if (!startingMap.Grid[startX][startY].Passable)
            {
                var fallback = startingMap.Grid
                    .SelectMany(column => column)
                    .FirstOrDefault(cell => cell.Passable);

                if (fallback == null)
                    return;

                startX = fallback.X;
                startY = fallback.Y;
            }

            world.SetPlayerPosition(characterId, startingMap.Id, startX, startY);
        }

        private void ConfigureTutorialMap()
        {
            var world = _worldService.CurrentWorld;
            var map = world.Maps.Values.FirstOrDefault(m => m.Name.Contains("Northaven"));
            if (map == null)
                return;

            map.Name = "Oakbridge Training Grounds";
            map.Description = "A compact beginner area laid out like a tutorial board: mentors in the center, practice spaces nearby, and safe roads to the village edge.";

            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    var cell = map.Grid[x][y];
                    cell.Name = string.Empty;
                    cell.Description = "Open village ground. You can cross this hex freely.";
                    cell.TerrainType = TerrainType.Plains;
                    cell.Passable = true;
                    cell.MovementCost = 1;
                    cell.NPCId = Guid.Empty;
                    cell.PointOfInterestId = Guid.Empty;
                    cell.StructureId = Guid.Empty;
                    cell.Properties.Clear();
                }
            }

            for (var x = 0; x < 9; x++)
            {
                map.Grid[x][4].TerrainType = TerrainType.Road;
                map.Grid[x][4].Name = x == 0 || x == 8 ? "Village Road Entry" : "Village Road";
                map.Grid[x][4].Description = "A clear road crossing the training grounds. Edge road hexes are good entry points for new scenes.";
            }

            for (var y = 0; y < 9; y++)
            {
                map.Grid[4][y].TerrainType = TerrainType.Road;
                map.Grid[4][y].Name = y == 0 || y == 8 ? "North-South Entry" : "Training Path";
                map.Grid[4][y].Description = "A north-south path used for marching order, movement, and positioning lessons.";
            }

            map.Grid[4][4].Name = "Training Green";
            map.Grid[4][4].Description = "The center of the tutorial area. Mira explains the 5E loop: describe an action, roll when uncertain, add the right modifier, and resolve the outcome.";
            SetTutorialCell(map, 4, 4, "Training Green", "The center of the tutorial area. Mira explains the 5E loop: describe an action, roll when uncertain, add the right modifier, and resolve the outcome.", POIType.Landmark, "Ask Mira for the Basics", "Mira walks you through the core loop: say what you do, roll a d20 when the outcome is uncertain, add the right modifier, then let the story respond.", new Dictionary<string, string>
            {
                { "SetFlag", "tutorial_intro=true" }
            });

            SetTutorialCell(map, 3, 4, "Ability Check Ring", "A chalk circle with simple obstacles. This is where players learn d20 ability checks and proficiency.", POIType.Puzzle, "Practice Ability Check", "You try a guided Strength or Dexterity check. Mira explains d20 + ability modifier + proficiency when trained.", new Dictionary<string, string>
            {
                { "AddExperience", "25" },
                { "SetFlag", "tutorial_ability_check=true" }
            });

            SetTutorialCell(map, 5, 4, "Saving Throw Yard", "Soft mats and swinging padded beams teach the difference between choosing an action and reacting to danger.", POIType.Puzzle, "Practice Saving Throw", "You duck a padded beam and learn that saving throws resist danger after it comes for you.", new Dictionary<string, string>
            {
                { "AddExperience", "25" },
                { "SetFlag", "tutorial_saving_throw=true" }
            });

            SetTutorialCell(map, 4, 3, "Practice Dummy Lane", "Straw dummies stand in a row with painted armor class numbers on their shields.", POIType.Encounter, "Practice Attack Roll", "You make a safe attack roll against a dummy and learn that attacks try to meet or beat Armor Class.", new Dictionary<string, string>
            {
                { "AddExperience", "25" },
                { "SetFlag", "tutorial_attack_roll=true" }
            });

            SetTutorialCell(map, 4, 5, "Resting Bench", "A shaded bench with water skins and bandages. This spot teaches short rests, hit dice, and recovery.", POIType.Resource, "Take a Short Rest Lesson", "You take a few minutes to recover and learn how rests help adventurers stay in the field.", new Dictionary<string, string>
            {
                { "AddExperience", "25" },
                { "SetFlag", "tutorial_short_rest=true" },
                { "CompleteQuest", "Learn the Core Loop" },
                { "AddQuest", "Help Oakbridge" }
            });

            SetTutorialCell(map, 2, 4, "Notice Board", "Simple beginner jobs are posted here, including a harmless mystery near the old mill.", POIType.Quest, "Read Beginner Jobs", "You read a starter quest: investigate strange tracks near the old mill without rushing into danger.", new Dictionary<string, string>
            {
                { "SetFlag", "tutorial_notice_board=true" },
                { "AddQuest", "Help Oakbridge" }
            });

            map.Grid[6][4].Name = "Old Mill Road";
            map.Grid[6][4].Description = "The road toward your first real quest. You should understand checks, saves, attacks, and rests before heading out.";
            map.Grid[6][4].TerrainType = TerrainType.Road;
        }

        private void SetTutorialCell(WorldMap map, int x, int y, string name, string description, POIType type, string actionName, string actionDescription, Dictionary<string, string> effects)
        {
            var poi = new PointOfInterest
            {
                Name = name,
                Description = description,
                Type = type,
                AvailableActions = new List<POIAction>
                {
                    new POIAction
                    {
                        Name = actionName,
                        Description = actionDescription,
                        Effects = effects
                    }
                }
            };

            _worldService.CurrentWorld.AddPointOfInterest(poi);
            map.Grid[x][y].Name = name;
            map.Grid[x][y].Description = description;
            map.Grid[x][y].PointOfInterestId = poi.Id;
        }

        private async Task ApplyChoiceEffects(GameState gameState, Choice choice)
        {
            if (choice.Effects == null || !choice.Effects.Any())
                return;

            foreach (var effect in choice.Effects)
            {
                switch (effect.Key.ToLower())
                {
                    case "quest_add":
                        if (!gameState.ActiveQuests.Contains(effect.Value))
                        {
                            gameState.ActiveQuests.Add(effect.Value);
                        }
                        break;
                        
                    case "quest_complete":
                        if (gameState.ActiveQuests.Contains(effect.Value))
                        {
                            gameState.ActiveQuests.Remove(effect.Value);
                            gameState.CompletedQuests.Add(effect.Value);
                        }
                        break;
                        
                    case "location_change":
                        gameState.CurrentLocation = effect.Value;
                        break;
                        
                    case "flag":
                        // Handle game flags (e.g., "accepted_wolf_quest=true")
                        var flagParts = effect.Value.Split('=');
                        if (flagParts.Length == 2)
                        {
                            gameState.Flags[flagParts[0]] = bool.Parse(flagParts[1]);
                        }
                        break;
                        
                    case "health_change":
                        if (int.TryParse(effect.Value, out var healthChange))
                        {
                            var character = await GetCharacterById(gameState.CharacterId);
                            character.HealthPoints = Math.Max(0, Math.Min(character.MaxHealthPoints, 
                                character.HealthPoints + healthChange));
                            await _characterRepository.UpdateAsync(character);
                        }
                        break;
                        
                    case "item_add":
                        var character2 = await GetCharacterById(gameState.CharacterId);
                        if (!character2.Inventory.Contains(effect.Value))
                        {
                            character2.Inventory.Add(effect.Value);
                            await _characterRepository.UpdateAsync(character2);
                        }
                        break;
                }
            }
            
            // Save updated game state
            await _gameStateRepository.UpdateAsync(gameState);
        }
    }
}
