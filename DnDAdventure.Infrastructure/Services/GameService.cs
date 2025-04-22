// DnDAdventure.Infrastructure/Services/GameService.cs
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Repositories;
using DnDAdventure.Core.Services;

namespace DnDAdventure.Infrastructure.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<GameState> _gameStateRepository;
        private readonly IRepository<Character> _characterRepository;

        public GameService(
            IRepository<GameState> gameStateRepository,
            IRepository<Character> characterRepository)
        {
            _gameStateRepository = gameStateRepository;
            _characterRepository = characterRepository;
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

        // Stub implementations for the methods that previously used AI components
        public async Task<AdventureNode> GetCurrentNode(Guid gameStateId)
        {
            var gameState = await GetGameStateById(gameStateId);
            
            // Create a simple placeholder node
            return new AdventureNode
            {
                Id = gameState.CurrentStoryNode,
                Description = "You are in " + gameState.CurrentLocation + ". (This is a placeholder until AI integration is complete)",
                Choices = new List<Choice>
                {
                    new Choice { Text = "Continue exploring", NextNodeId = gameState.CurrentStoryNode }
                }
            };
        }

        public async Task<AdventureNode> ProcessChoice(Guid gameStateId, int choiceIndex)
        {
            var gameState = await GetGameStateById(gameStateId);
            
            // For now, just return a placeholder response
            return new AdventureNode
            {
                Id = gameState.CurrentStoryNode,
                Description = "You made a choice. (This is a placeholder until AI integration is complete)",
                Choices = new List<Choice>
                {
                    new Choice { Text = "Continue", NextNodeId = gameState.CurrentStoryNode }
                }
            };
        }
    }
}
