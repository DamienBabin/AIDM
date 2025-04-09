// DnDAdventure.API/Controllers/GameController.cs
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IWorldService _worldService;
        
        public GameController(IGameService gameService, IWorldService worldService)
        {
            _gameService = gameService;
            _worldService = worldService;
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<GameState>> CreateGame([FromBody] Character character)
        {
            var gameState = await _gameService.CreateNewGame(character);
            return Ok(gameState);
        }
        
        [HttpGet("{gameStateId}")]
        public async Task<ActionResult<GameState>> GetGameState(Guid gameStateId)
        {
            try
            {
                var gameState = await _gameService.GetGameStateById(gameStateId);
                return Ok(gameState);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("character/{characterId}")]
        public async Task<ActionResult<Character>> GetCharacter(Guid characterId)
        {
            try
            {
                var character = await _gameService.GetCharacterById(characterId);
                return Ok(character);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("{gameStateId}/node")]
        public async Task<ActionResult<AdventureNode>> GetCurrentNode(Guid gameStateId)
        {
            try
            {
                var node = await _gameService.GetCurrentNode(gameStateId);
                return Ok(node);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("{gameStateId}/choice/{choiceIndex}")]
        public async Task<ActionResult<AdventureNode>> MakeChoice(Guid gameStateId, int choiceIndex)
        {
            try
            {
                var nextNode = await _gameService.ProcessChoice(gameStateId, choiceIndex);
                return Ok(nextNode);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid choice index");
            }
        }

        [HttpPost("game/{gameStateId}/interact-npc")]
        public async Task<IActionResult> InteractWithNPC(Guid gameStateId, [FromBody] NPCInteractionRequest request)
        {
            try
            {
                var gameState = await _gameService.GetGameStateById(gameStateId);
                var character = await _gameService.GetCharacterById(gameState.CharacterId);

                // Get the world from the gameState
                var world = _worldService.CurrentWorld; // You would need a reference to WorldService

                // Get the NPC
                var npc = world.GetNPCById(request.NPCId);
                if (npc == null)
                {
                    return NotFound($"NPC with ID {request.NPCId} not found");
                }

                // Process the interaction based on the type
                AdventureNode resultNode = new AdventureNode
                {
                    Id = gameState.CurrentStoryNode,
                    Description = $"You interact with {npc.Name}."
                };

                switch (request.InteractionType.ToLower())
                {
                    case "talk":
                        // Get dialog options for this NPC
                        resultNode.Description = $"{npc.Name} says: \"{GetNPCDialog(npc, request.DialogId)}\"";
                        // Add dialog options as choices
                        resultNode.Choices = GetDialogChoices(npc, request.DialogId);
                        break;

                    case "trade":
                        // Get items available from this NPC
                        resultNode.Description = $"You approach {npc.Name} to trade.";
                        resultNode.Choices = GetTradeChoices(npc, character);
                        break;

                    case "quest":
                        // Get quest information from this NPC
                        resultNode.Description = $"You talk to {npc.Name} about available quests.";
                        resultNode.Choices = GetQuestChoices(npc, gameState);
                        break;

                    default:
                        return BadRequest($"Unknown interaction type: {request.InteractionType}");
                }

                // Update the game state to record this interaction
                gameState.RecentNPCInteractions.Add(new GameState.NPCInteraction
                {
                    NPCId = npc.Id,
                    NPCName = npc.Name,
                    InteractionType = request.InteractionType,
                    Details = resultNode.Description,
                    Timestamp = DateTime.UtcNow
                });

                await _gameService.UpdateGameState(gameState);

                return Ok(resultNode);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing NPC interaction: {ex.Message}");
            }
        }

        // Helper methods for NPC interactions
        private string GetNPCDialog(NPC npc, Guid? dialogId)
        {
            if (!dialogId.HasValue)
            {
                // Return default greeting
                var greeting = npc.Dialogs.FirstOrDefault(d => d.Topic.Equals("Greeting", StringComparison.OrdinalIgnoreCase));
                return greeting?.Text ?? $"Hello there, traveler. I am {npc.Name}.";
            }

            var dialog = npc.Dialogs.FirstOrDefault(d => d.Id == dialogId.Value);
            return dialog?.Text ?? $"Hello there, traveler. I am {npc.Name}.";
        }

        private List<Choice> GetDialogChoices(NPC npc, Guid? dialogId)
        {
            NPCDialog? dialog = null;

            if (!dialogId.HasValue)
            {
                // Get default greeting dialog
                dialog = npc.Dialogs.FirstOrDefault(d => d.Topic.Equals("Greeting", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                dialog = npc.Dialogs.FirstOrDefault(d => d.Id == dialogId.Value);
            }

            if (dialog == null)
            {
                // Return a default choice to end conversation
                return new List<Choice>
                {
                    new Choice
                    {
                        Text = "End conversation",
                        Effects = new Dictionary<string, string>
                        {
                            { "EndNPCInteraction", "true" }
                        }
                    }
                };
            }

            // Convert dialog options to choices
            return dialog.Options.Select((option, index) => new Choice
            {
                Text = option.Text,
                NextNodeId = 0, // We use special handling for NPC dialogs
                Effects = new Dictionary<string, string>
                {
                    { "NPCDialogOption", index.ToString() },
                    { "NPCDialogId", dialog.Id.ToString() }
                }
            }).ToList();
        }

        private List<Choice> GetTradeChoices(NPC npc, Character character)
        {
            // Implement trade choices logic
            return new List<Choice>();
        }

        private List<Choice> GetQuestChoices(NPC npc, GameState gameState)
        {
            // Implement quest choices logic
            return new List<Choice>();
        }
    }

    public class NPCInteractionRequest
    {
        public Guid NPCId { get; set; }
        public string InteractionType { get; set; } = string.Empty;
        public Guid? DialogId { get; set; }
        public string? ItemName { get; set; }
        public string? QuestId { get; set; }
    }
}
