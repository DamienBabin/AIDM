using DnDAdventure.Core.Models;

namespace DnDAdventure.Core.Services
{
    public interface IMapService
    {
        Task<bool> MoveCharacter(Guid characterId, Direction direction);
        Task<ExplorationResult> ExploreCurrentCell(Guid characterId);
        Task<InteractionResult> InteractWithPOI(Guid characterId, Guid poiId, string action);
        (WorldMap map, int x, int y) GetCharacterLocation(Guid characterId);
        List<POIBriefInfo> GetMapPOIs(Guid mapId);
        List<WorldMap> GetAllMaps();
        WorldMap? GetMap(Guid mapId);
        string GetMapDisplay(Guid characterId);
    }
} 