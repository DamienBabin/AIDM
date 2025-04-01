// DnDAdventure.AI/AdventureGenerator.cs
using DnDAdventure.Core.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DnDAdventure.AI
{
    public class AdventureGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiEndpoint;
        private readonly JsonSerializerOptions _jsonOptions;

        // Fallback content in case the AI service is not available
        private readonly Dictionary<int, AdventureNode> _fallbackNodes;

        public AdventureGenerator(HttpClient httpClient, string aiEndpoint)
        {
            _httpClient = httpClient;
            _aiEndpoint = aiEndpoint;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Initialize fallback nodes
            _fallbackNodes = InitializeFallbackNodes();
        }

        public async Task<AdventureNode> GenerateNextNode(
            Character character, 
            GameState gameState, 
            string userAction)
        {
            try
            {
                var request = new
                {
                    character = character,
                    gameState = gameState,
                    userAction = userAction,
                    prompt = $"Generate a D&D adventure node for a {character.Race} {character.Class} named {character.Name} who is currently at {gameState.CurrentLocation} and has just chosen to {userAction}."
                };

                var response = await _httpClient.PostAsJsonAsync(_aiEndpoint, request);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AdventureNode>(_jsonOptions);
                    return result ?? GetFallbackNode(gameState.CurrentStoryNode);
                }
                
                return GetFallbackNode(gameState.CurrentStoryNode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating adventure node: {ex.Message}");
                return GetFallbackNode(gameState.CurrentStoryNode);
            }
        }

        private AdventureNode GetFallbackNode(int nodeId)
        {
            if (_fallbackNodes.TryGetValue(nodeId, out var node))
            {
                return node;
            }

            return _fallbackNodes[1]; // Return the first node if the requested one doesn't exist
        }

        private Dictionary<int, AdventureNode> InitializeFallbackNodes()
        {
            return new Dictionary<int, AdventureNode>
            {
                {
                    1, new AdventureNode
                    {
                        Id = 1,
                        Description = "You find yourself in the village of Northaven. The air is crisp, and the smell of freshly baked bread wafts from the nearby bakery. Villagers bustle about their daily routines, occasionally glancing at you with curious eyes. A notice board stands in the center of the square, while the road continues north towards the mountains and south towards the forest.",
                        Choices = new List<Choice>
                        {
                            new Choice
                            {
                                Text = "Check the notice board",
                                NextNodeId = 2,
                                Effects = new Dictionary<string, string>()
                            },
                            new Choice
                            {
                                Text = "Head to the local tavern",
                                NextNodeId = 3,
                                Effects = new Dictionary<string, string>()
                            },
                            new Choice
                            {
                                Text = "Explore the village",
                                NextNodeId = 4,
                                Effects = new Dictionary<string, string>()
                            }
                        }
                    }
                },
                {
                    2, new AdventureNode
                    {
                        Id = 2,
                        Description = "The notice board is filled with various announcements, but one catches your eye. A reward of 100 gold pieces is offered for the elimination of wolves that have been harassing livestock on the outskirts of town. Another notice seeks brave adventurers to explore an ancient tomb discovered in the nearby hills.",
                        Choices = new List<Choice>
                        {
                            new Choice
                            {
                                Text = "Accept the wolf hunting quest",
                                NextNodeId = 5,
                                Effects = new Dictionary<string, string>
                                {
                                    { "quest_add", "Wolf Hunters" },
                                    { "flag", "accepted_wolf_quest=true" }
                                }
                            },
                            new Choice
                            {
                                Text = "Accept the tomb exploration quest",
                                NextNodeId = 6,
                                Effects = new Dictionary<string, string>
                                {
                                    { "quest_add", "Tomb Explorer" },
                                    { "flag", "accepted_tomb_quest=true" }
                                }
                            },
                            new Choice
                            {
                                Text = "Return to the village square",
                                NextNodeId = 1,
                                Effects = new Dictionary<string, string>()
                            }
                        }
                    }
                },
                {
                    3, new AdventureNode
                    {
                        Id = 3,
                        Description = "The Drunken Dragon tavern is warm and inviting. A hearty fire crackles in the hearth, and the room is filled with the murmur of conversation and occasional laughter. The bartender, a stout dwarf with a bushy beard, nods as you enter. In the corner, a group of locals are engaged in what appears to be a heated discussion.",
                        Choices = new List<Choice>
                        {
                            new Choice
                            {
                                Text = "Approach the bartender",
                                NextNodeId = 7,
                                Effects = new Dictionary<string, string>()
                            },
                            new Choice
                            {
                                Text = "Listen in on the locals' conversation",
                                NextNodeId = 8,
                                Effects = new Dictionary<string, string>()
                            },
                            new Choice
                            {
                                Text = "Leave the tavern",
                                NextNodeId = 1,
                                Effects = new Dictionary<string, string>()
                            }
                        }
                    }
                }
                // Add more fallback nodes as needed
            };
        }
    }
}