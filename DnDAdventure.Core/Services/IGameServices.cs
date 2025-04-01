// DnDAdventure.Core/Services/IGameService.cs
namespace DnDAdventure.Core.Services
{
    using DnDAdventure.Core.Models;

    public interface IGameService
    {
        Task<GameState> CreateNewGame(Character character);
        Task<GameState> GetGameStateById(Guid id);
        Task<Character> GetCharacterById(Guid id);
        Task<AdventureNode> GetCurrentNode(Guid gameStateId);
        Task<AdventureNode> ProcessChoice(Guid gameStateId, int choiceIndex);
    }
}

