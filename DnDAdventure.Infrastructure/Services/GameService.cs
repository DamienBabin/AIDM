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

        public GameService(
            IRepository<GameState> gameStateRepository,
            IRepository<Character> characterRepository,
            AdventureGenerator adventureGenerator,
            IWorldService worldService)
        {
            _gameStateRepository = gameStateRepository;
            _characterRepository = characterRepository;
            _adventureGenerator = adventureGenerator;
            _worldService = worldService;
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
            
            // Parse worldId to Guid if provided, otherwise create a new one
            Guid parsedWorldId = Guid.Empty;
            if (!string.IsNullOrEmpty(worldId) && Guid.TryParse(worldId, out parsedWorldId))
            {
                // Use provided world ID
            }
            else
            {
                // Create a new world ID for new worlds
                parsedWorldId = Guid.NewGuid();
            }
            
            // Create initial game state
            var gameState = new GameState
            {
                Id = Guid.NewGuid(),
                CharacterId = characterId,
                WorldId = parsedWorldId,
                CurrentLocation = "Village of Northaven",
                CurrentStoryNode = 1 // Starting node
            };
            
            await _gameStateRepository.AddAsync(gameState);
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
