// DnDAdventure.Infrastructure/Services/WorldService.cs
using DnDAdventure.Core.Models;
using DnDAdventure.Core.Services;

namespace DnDAdventure.Infrastructure.Services
{
    /// <summary>
    /// Service for managing the game world, including saves and world state
    /// </summary>
    public class WorldService : IWorldService
    {
        private readonly string _savesDirectory;
        private World _currentWorld;
        
        public WorldService(string savesDirectory = "Saves")
        {
            _savesDirectory = savesDirectory;
            _currentWorld = new World();
            
            // Ensure saves directory exists
            if (!Directory.Exists(_savesDirectory))
            {
                Directory.CreateDirectory(_savesDirectory);
            }
        }
        
        /// <summary>
        /// Gets the current active world
        /// </summary>
        public World CurrentWorld => _currentWorld;
        
        /// <summary>
        /// Creates a new world with the given name
        /// </summary>
        /// <param name="name">Name of the new world</param>
        /// <param name="description">Description of the new world</param>
        /// <returns>The newly created world</returns>
        public World CreateNewWorld(string name, string description = "")
        {
            _currentWorld = new World
            {
                Name = name,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                LastSavedAt = DateTime.UtcNow
            };
            
            return _currentWorld;
        }
        
        /// <summary>
        /// Saves the current world to a file
        /// </summary>
        /// <param name="filename">Optional custom filename (without extension)</param>
        /// <returns>Path to the saved file, or null if saving failed</returns>
        public string? SaveWorld(string? filename = null)
        {
            try
            {
                string filePath;
                
                if (string.IsNullOrEmpty(filename))
                {
                    // Use world name and timestamp if no filename is provided
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string safeWorldName = _currentWorld.Name.Replace(" ", "_");
                    filePath = Path.Combine(_savesDirectory, $"{safeWorldName}_{timestamp}.json");
                }
                else
                {
                    filePath = Path.Combine(_savesDirectory, $"{filename}.json");
                }
                
                if (_currentWorld.SaveToJson(filePath))
                {
                    return filePath;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving world: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Loads a world from a save file
        /// </summary>
        /// <param name="filePath">Path to the save file</param>
        /// <returns>True if loading succeeded, false otherwise</returns>
        public bool LoadWorld(string filePath)
        {
            try
            {
                var loadedWorld = World.LoadFromJson(filePath);
                
                if (loadedWorld != null)
                {
                    _currentWorld = loadedWorld;
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading world: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets a list of available save files
        /// </summary>
        /// <returns>List of save file metadata</returns>
        public List<SaveFileInfo> GetAvailableSaves()
        {
            List<SaveFileInfo> saveFiles = new List<SaveFileInfo>();
            
            try
            {
                var files = World.GetAvailableSaves(_savesDirectory);
                
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    
                    // Try to read basic metadata without loading the entire file
                    try
                    {
                        string jsonStart = ReadJsonStart(file, 1000); // Read first 1000 chars
                        
                        string worldName = ExtractJsonValue(jsonStart, "name") ?? "Unknown World";
                        string timestamp = ExtractJsonValue(jsonStart, "lastSavedAt") ?? fileInfo.LastWriteTime.ToString();
                        
                        saveFiles.Add(new SaveFileInfo
                        {
                            FilePath = file,
                            FileName = fileInfo.Name,
                            WorldName = worldName,
                            LastSaved = DateTime.TryParse(timestamp, out var date) ? date : fileInfo.LastWriteTime,
                            FileSize = fileInfo.Length,
                            FormattedSize = FormatFileSize(fileInfo.Length)
                        });
                    }
                    catch
                    {
                        // Fallback if we can't parse the JSON
                        saveFiles.Add(new SaveFileInfo
                        {
                            FilePath = file,
                            FileName = fileInfo.Name,
                            WorldName = Path.GetFileNameWithoutExtension(file),
                            LastSaved = fileInfo.LastWriteTime,
                            FileSize = fileInfo.Length,
                            FormattedSize = FormatFileSize(fileInfo.Length)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available saves: {ex.Message}");
            }
            
            return saveFiles;
        }
        
        /// <summary>
        /// Creates a quick save of the current world
        /// </summary>
        /// <returns>Path to the saved file, or null if saving failed</returns>
        public string? CreateQuickSave()
        {
            return _currentWorld.CreateQuickSave(_savesDirectory);
        }
        
        /// <summary>
        /// Adds a character to the current world
        /// </summary>
        /// <param name="character">The character to add</param>
        public void AddCharacter(Character character)
        {
            _currentWorld.AddCharacter(character);
        }
        
        /// <summary>
        /// Adds a game state to the current world
        /// </summary>
        /// <param name="gameState">The game state to add</param>
        public void AddGameState(GameState gameState)
        {
            _currentWorld.AddGameState(gameState);
        }
        
        /// <summary>
        /// Reads the beginning of a JSON file
        /// </summary>
        private string ReadJsonStart(string filePath, int chars)
        {
            using var reader = new StreamReader(filePath);
            char[] buffer = new char[chars];
            reader.ReadBlock(buffer, 0, chars);
            return new string(buffer);
        }
        
        /// <summary>
        /// Extracts a value from a JSON string (simple implementation)
        /// </summary>
        private string? ExtractJsonValue(string json, string key)
        {
            // Simple regex-based extraction
            var match = System.Text.RegularExpressions.Regex.Match(json, $"\"{key}\"\\s*:\\s*\"([^\"]+)\"");
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            
            return null;
        }
        
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }
            
            return $"{size:0.##} {sizes[order]}";
        }
    }
}
