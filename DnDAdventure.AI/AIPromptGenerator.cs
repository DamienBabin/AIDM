// DnDAdventure.AI/AIPromptGenerator.cs
using DnDAdventure.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnDAdventure.AI
{
    public static class AIPromptGenerator
    {
        public static string GenerateAdventurePrompt(
            Character character, 
            GameState gameState, 
            string userAction,
            List<NPC> npcsAtLocation)
        {
            var sb = new StringBuilder();
            
            // Basic character and action info
            sb.AppendLine($"Generate a D&D adventure node for a {character.Race} {character.Class} named {character.Name} who is currently at {gameState.CurrentLocation} and has just chosen to {userAction}.");
            
            // Add NPC information
            if (npcsAtLocation != null && npcsAtLocation.Any())
            {
                sb.AppendLine("\nThe following NPCs are present:");
                foreach (var npc in npcsAtLocation)
                {
                    sb.AppendLine($"- {npc.Name}: {npc.Race} {npc.Occupation}. {npc.Description}");
                }
                
                sb.AppendLine("\nYou can create dialog and interactions with these NPCs. The player can:");
                sb.AppendLine("- Talk to NPCs to gather information");
                sb.AppendLine("- Trade with NPCs to acquire items");
                sb.AppendLine("- Complete quests for NPCs");
            }
            
            // Add character context
            sb.AppendLine("\nCharacter context:");
            sb.AppendLine($"- Level: {character.Level}");
            sb.AppendLine($"- Health: {character.HealthPoints}/{character.MaxHealthPoints}");
            sb.AppendLine($"- Main attributes: STR {character.Attributes.GetValueOrDefault("Strength")}, DEX {character.Attributes.GetValueOrDefault("Dexterity")}, CON {character.Attributes.GetValueOrDefault("Constitution")}");
            
            // Add inventory context
            if (character.Inventory.Any())
            {
                sb.AppendLine($"- Inventory: {string.Join(", ", character.Inventory)}");
            }
            
            // Add quest context
            if (gameState.ActiveQuests.Any())
            {
                sb.AppendLine($"- Active quests: {string.Join(", ", gameState.ActiveQuests)}");
            }
            
            // Provide guidance on the response format
            sb.AppendLine("\nGenerate a response with:");
            sb.AppendLine("1. A detailed description of what happens");
            sb.AppendLine("2. Any NPC dialog in quotation marks");
            sb.AppendLine("3. A list of 2-4 choices for the player's next action");
            sb.AppendLine("4. Include NPC interaction options where appropriate");
            
            return sb.ToString();
        }
        
        public static AdvancedPrompt GenerateAdvancedPrompt(
            Character character, 
            GameState gameState, 
            string userAction,
            List<NPC> npcsAtLocation)
        {
            return new AdvancedPrompt
            {
                Character = new CharacterPromptInfo
                {
                    Name = character.Name,
                    Race = character.Race,
                    Class = character.Class,
                    Level = character.Level,
                    HealthPoints = character.HealthPoints,
                    MaxHealthPoints = character.MaxHealthPoints,
                    Attributes = character.Attributes,
                    Inventory = character.Inventory
                },
                GameState = new GameStatePromptInfo
                {
                    CurrentLocation = gameState.CurrentLocation,
                    ActiveQuests = gameState.ActiveQuests,
                    CompletedQuests = gameState.CompletedQuests
                },
                UserAction = userAction,
                NPCsPresent = npcsAtLocation.Select(npc => new NPCPromptInfo
                {
                    Name = npc.Name,
                    Race = npc.Race,
                    Occupation = npc.Occupation,
                    Description = npc.Description,
                    IsTalkative = npc.Dialogs.Count > 0,
                    HasQuestsAvailable = npc.AvailableQuestIds.Count > 0,
                    HasItemsForSale = npc.Inventory.Any(i => i.IsTradeable)
                }).ToList(),
                Instructions = "Generate a rich, descriptive adventure node with NPC interactions where appropriate. Include dialog and provide meaningful choices that reflect the player's options with NPCs present."
            };
        }
    }
    
    // Classes for structured AI prompts
    public class AdvancedPrompt
    {
        public CharacterPromptInfo Character { get; set; } = new();
        public GameStatePromptInfo GameState { get; set; } = new();
        public string UserAction { get; set; } = string.Empty;
        public List<NPCPromptInfo> NPCsPresent { get; set; } = new();
        public string Instructions { get; set; } = string.Empty;
    }
    
    public class CharacterPromptInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Level { get; set; }
        public int HealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }
        public Dictionary<string, int> Attributes { get; set; } = new();
        public List<string> Inventory { get; set; } = new();
    }
    
    public class GameStatePromptInfo
    {
        public string CurrentLocation { get; set; } = string.Empty;
        public List<string> ActiveQuests { get; set; } = new();
        public List<string> CompletedQuests { get; set; } = new();
    }
    
    public class NPCPromptInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsTalkative { get; set; }
        public bool HasQuestsAvailable { get; set; }
        public bool HasItemsForSale { get; set; }
    }
}
