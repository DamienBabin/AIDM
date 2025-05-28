using DnDAdventure.Core.Models;
using System;
using System.Collections.Generic;

namespace DnDAdventure.Core.Services
{
    // SaveFileInfo needs to be moved to the interface to prevent circular references
    public class SaveFileInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string WorldName { get; set; } = string.Empty;
        public DateTime LastSaved { get; set; }
        public DateTime CreatedAt { get; set; }
        public long FileSize { get; set; }
        public string FormattedSize { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public interface IWorldService
    {
        World CurrentWorld { get; }
        World CreateNewWorld(string name, string description = "");
        bool LoadWorld(string filePath);
        bool LoadWorldFromJson(string jsonContent);
        (bool IsValid, string Message, World? World) ValidateJsonContent(string jsonContent);
        string? SaveWorld(string? worldName = null);
        string? CreateQuickSave();
        List<SaveFileInfo> GetAvailableSaves();
        void AddCharacter(Character character);
        void AddGameState(GameState gameState);
    }
}
