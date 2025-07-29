// DnDAdventure.API/Controllers/GameController.cs
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DnDAdventure.API.Controllers
{
    public class StartGameRequest
    {
        public Guid CharacterId { get; set; }
        public string? WorldId { get; set; }
        public string? WorldName { get; set; }
        public string? WorldDescription { get; set; }
    }
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
        
        [HttpPost("character")]
        public async Task<ActionResult<Character>> CreateCharacter([FromBody] Character character)
        {
            var savedCharacter = await _gameService.SaveCharacter(character);
            return Ok(savedCharacter);
        }
        
        [HttpPost("start")]
        public async Task<ActionResult<GameState>> StartGame([FromBody] StartGameRequest request)
        {
            var gameState = await _gameService.StartNewGame(request.CharacterId, request.WorldId, request.WorldName, request.WorldDescription);
            return Ok(gameState);
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
    }
}
