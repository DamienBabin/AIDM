// DnDAdventure.Infrastructure/Services/GameService.cs
using DnDAdventure.AI;
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Repositories;
using DnDAdventure.Core.Services;

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

        public async Task<AdventureNode> GetCurrentNode(Guid gameStateId)
        {
            var gameState = await GetGameStateById(gameStateId);
            var character = await GetCharacterById(gameState.CharacterId);
            
            // Generate or retrieve the current node
            return await _adventureGenerator.GenerateNextNode(
                character, 
                gameState, 
                "look around");
        }

        public async Task<AdventureNode> ProcessChoice(Guid gameStateId, int choiceIndex)
        {
            var gameState = await GetGameStateById(gameStateId);
            var character = await GetCharacterById(gameState.CharacterId);
            
            // Get the current node
            var currentNode = await _adventureGenerator.GenerateNextNode(
                character,
                gameState,
                "look around");
            
            if (choiceIndex < 0 || choiceIndex >= currentNode.Choices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(choiceIndex));
            }
            
            var selectedChoice = currentNode.Choices[choiceIndex];
            
            // Update game state based on choice effects
            foreach (var effect in selectedChoice.Effects)
            {
                switch (effect.Key)
                {
                    case "location":
                        gameState.CurrentLocation = effect.Value;
                        break;
                    case "quest_add":
                        gameState.ActiveQuests.Add(effect.Value);
                        break;
                    case "quest_complete":
                        gameState.ActiveQuests.Remove(effect.Value);
                        gameState.CompletedQuests.Add(effect.Value);
                        break;
                    case "flag":
                        var flagParts = effect.Value.Split('=');
                        if (flagParts.Length == 2)
                        {
                            gameState.Flags[flagParts[0]] = bool.Parse(flagParts[1]);
                        }
                        break;
                }
            }
            
            // Update the current node
            gameState.CurrentStoryNode = selectedChoice.NextNodeId;
            await _gameStateRepository.UpdateAsync(gameState);
            
            // Generate the next node
            return await _adventureGenerator.GenerateNextNode(
                character,
                gameState,
                selectedChoice.Text);
        }

        public async Task<AdventureNode> ProcessNPCInteraction(
            Guid gameStateId,
            Guid npcId,
            string interactionType,
            Guid? dialogId = null)
        {
            var gameState = await GetGameStateById(gameStateId);
            var character = await GetCharacterById(gameState.CharacterId);

            // Get the NPC from the world
            var world = _worldService.CurrentWorld;
            var npc = world.GetNPCById(npcId);

            if (npc == null)
            {
                throw new KeyNotFoundException($"NPC with ID {npcId} not found");
            }

            // Create a response node based on the interaction
            var node = new AdventureNode
            {
                Id = gameState.CurrentStoryNode,
                Description = $"You interact with {npc.Name}."
            };

            // Process based on interaction type
            switch (interactionType.ToLower())
            {
                case "talk":
                    ProcessTalkInteraction(node, npc, dialogId, character, gameState);
                    break;

                case "trade":
                    ProcessTradeInteraction(node, npc, character, gameState);
                    break;

                case "quest":
                    ProcessQuestInteraction(node, npc, character, gameState);
                    break;

                default:
                    node.Description = $"You approach {npc.Name}, but you're not sure how to interact.";
                    node.Choices = new List<Choice>
                    {
                        new Choice { Text = "Return to exploration", NextNodeId = gameState.CurrentStoryNode }
                    };
                    break;
            }

            // Record the interaction
            gameState.RecentNPCInteractions.Add(new GameState.NPCInteraction
            {
                NPCId = npc.Id,
                NPCName = npc.Name
            });

            await _gameStateRepository.UpdateAsync(gameState);
            return node;
        }
    }
}
