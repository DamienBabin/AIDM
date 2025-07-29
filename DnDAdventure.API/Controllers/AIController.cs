// DnDAdventure.API/Controllers/AIController.cs
using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;
using DnDAdventure.AI;
using System.Text.Json;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly Random _random = new Random();

        [HttpPost("generate")]
        public async Task<ActionResult<AdventureNode>> GenerateAdventure([FromBody] AIRequest request)
        {
            try
            {
                // For now, we'll generate rich adventure content based on the prompt
                var adventureNode = GenerateAdventureContent(request);
                return Ok(adventureNode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AI generation: {ex.Message}");
                return StatusCode(500, "AI generation failed");
            }
        }

        private AdventureNode GenerateAdventureContent(AIRequest request)
        {
            var promptData = request.PromptData;
            var character = promptData?.Character;
            var gameState = promptData?.GameState;
            var userAction = promptData?.UserAction ?? "start adventure";

            // Generate dynamic content based on character and action
            var scenarios = GetScenarios(character, gameState, userAction);
            var selectedScenario = scenarios[_random.Next(scenarios.Count)];

            return new AdventureNode
            {
                Id = _random.Next(1000, 9999),
                Description = selectedScenario.Description,
                Choices = selectedScenario.Choices
            };
        }

        private List<AdventureScenario> GetScenarios(CharacterPromptInfo? character, GameStatePromptInfo? gameState, string userAction)
        {
            var characterClass = character?.Class?.ToLower() ?? "fighter";
            var characterRace = character?.Race?.ToLower() ?? "human";
            var location = gameState?.CurrentLocation ?? "Village of Northaven";

            var scenarios = new List<AdventureScenario>();

            // Starting scenarios
            if (userAction.Contains("start") || userAction.Contains("begin"))
            {
                scenarios.AddRange(GetStartingScenarios(characterClass, characterRace, location));
            }
            // Exploration scenarios
            else if (userAction.Contains("explore") || userAction.Contains("investigate"))
            {
                scenarios.AddRange(GetExplorationScenarios(characterClass, characterRace, location));
            }
            // Combat scenarios
            else if (userAction.Contains("fight") || userAction.Contains("attack"))
            {
                scenarios.AddRange(GetCombatScenarios(characterClass, characterRace));
            }
            // Social scenarios
            else if (userAction.Contains("talk") || userAction.Contains("speak") || userAction.Contains("tavern"))
            {
                scenarios.AddRange(GetSocialScenarios(characterClass, characterRace, location));
            }
            // Quest scenarios
            else if (userAction.Contains("quest") || userAction.Contains("notice"))
            {
                scenarios.AddRange(GetQuestScenarios(characterClass, characterRace));
            }
            else
            {
                // Default mixed scenarios
                scenarios.AddRange(GetMixedScenarios(characterClass, characterRace, location));
            }

            return scenarios.Any() ? scenarios : GetDefaultScenarios();
        }

        private List<AdventureScenario> GetStartingScenarios(string characterClass, string characterRace, string location)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"As a {characterRace} {characterClass}, you arrive in the bustling {location}. The morning sun casts long shadows across the cobblestone streets, and the air is filled with the sounds of merchants hawking their wares and children playing. A weathered notice board stands prominently in the town square, covered with various announcements and requests for aid. The local tavern, 'The Prancing Pony,' emanates warm light and the sound of laughter. To the north, a winding path leads toward mysterious hills shrouded in morning mist.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Examine the notice board for potential quests", NextNodeId = 2, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Enter the tavern to gather information", NextNodeId = 3, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Head north toward the mysterious hills", NextNodeId = 4, Effects = new Dictionary<string, string> { {"location_change", "Northern Hills"} } },
                        new Choice { Text = "Explore the local shops and market", NextNodeId = 5, Effects = new Dictionary<string, string>() }
                    }
                },
                new AdventureScenario
                {
                    Description = $"Your journey as a {characterRace} {characterClass} has brought you to {location} just as the evening bells toll. The town is settling into twilight, with lanterns being lit along the main thoroughfare. You notice unusual activity - several townspeople are gathered in hushed conversations, occasionally glancing toward the old cemetery on the hill. A young woman approaches you with urgency in her eyes. 'Stranger,' she whispers, 'we need someone brave. Strange lights have been seen in the graveyard, and livestock has gone missing. The town guard won't investigate after dark.'",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Offer to investigate the cemetery tonight", NextNodeId = 6, Effects = new Dictionary<string, string> { {"quest_add", "Cemetery Investigation"} } },
                        new Choice { Text = "Ask for more details about the missing livestock", NextNodeId = 7, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Suggest waiting until morning for a proper investigation", NextNodeId = 8, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Decline and seek lodging for the night", NextNodeId = 9, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetExplorationScenarios(string characterClass, string characterRace, string location)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"Your keen {characterRace} senses guide you as you explore the area. As a {characterClass}, you notice details others might miss. You discover an ancient stone archway partially hidden by overgrown vines. Strange runes are carved into the weathered stone, and a faint magical aura emanates from within. The air grows cooler as you approach, and you hear the distant sound of running water echoing from the darkness beyond. Your adventurer's instincts tell you this could be significant, but also potentially dangerous.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Carefully examine the runes before proceeding", NextNodeId = 10, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Light a torch and venture through the archway", NextNodeId = 11, Effects = new Dictionary<string, string> { {"location_change", "Ancient Ruins"} } },
                        new Choice { Text = "Mark the location and return to town for supplies", NextNodeId = 12, Effects = new Dictionary<string, string> { {"flag", "discovered_ruins=true"} } },
                        new Choice { Text = "Search the surrounding area for other clues", NextNodeId = 13, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetCombatScenarios(string characterClass, string characterRace)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"Your {characterClass} training serves you well as three bandits emerge from the undergrowth, weapons drawn! The leader, a scarred human with a wicked grin, points his rusty sword at you. 'Well, well, what have we here? A lone {characterRace} with coin purse jingling. Hand over your valuables, and we might let you walk away with all your limbs!' His companions, a shifty halfling with a dagger and a brutish orc with a club, spread out to flank you. Your hand instinctively moves to your weapon as you assess the situation.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Draw your weapon and fight all three bandits", NextNodeId = 14, Effects = new Dictionary<string, string> { {"health_change", "-5"} } },
                        new Choice { Text = "Try to intimidate them with your presence", NextNodeId = 15, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Offer to share some coin to avoid bloodshed", NextNodeId = 16, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Attempt to flee into the dense forest", NextNodeId = 17, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetSocialScenarios(string characterClass, string characterRace, string location)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"The tavern buzzes with activity as you enter. As a {characterRace} {characterClass}, you draw some curious glances from the patrons. The barkeeper, a stout dwarf with a magnificent braided beard, nods in greeting. 'Welcome to The Golden Griffin, traveler! What brings you to our humble {location}?' At a corner table, a hooded figure sits alone, occasionally glancing your way. Near the fireplace, a group of local farmers are engaged in heated discussion about recent troubles with wolves. A well-dressed merchant at the bar seems eager to share tales of his travels.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Approach the barkeeper for local information", NextNodeId = 18, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Join the farmers' discussion about the wolf problem", NextNodeId = 19, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Carefully approach the mysterious hooded figure", NextNodeId = 20, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Strike up a conversation with the traveling merchant", NextNodeId = 21, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetQuestScenarios(string characterClass, string characterRace)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"The notice board is filled with various requests, but three catch your eye as suitable for a {characterRace} {characterClass}. The first is a plea from a local farmer: 'HELP! Dire wolves have been attacking my livestock. 50 gold pieces for proof of their elimination.' The second notice bears an official seal: 'Wanted: Brave souls to explore the recently discovered Tomb of Shadows. Generous compensation for artifacts recovered.' The third is written in an elegant hand: 'Seeking discrete individual to retrieve a family heirloom from bandits. Payment negotiable, discretion essential.'",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Accept the dire wolf elimination quest", NextNodeId = 22, Effects = new Dictionary<string, string> { {"quest_add", "Wolf Hunter"} } },
                        new Choice { Text = "Take on the tomb exploration mission", NextNodeId = 23, Effects = new Dictionary<string, string> { {"quest_add", "Tomb Explorer"} } },
                        new Choice { Text = "Investigate the discrete heirloom recovery job", NextNodeId = 24, Effects = new Dictionary<string, string> { {"quest_add", "Heirloom Recovery"} } },
                        new Choice { Text = "Look for other opportunities or gather more information", NextNodeId = 25, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetMixedScenarios(string characterClass, string characterRace, string location)
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = $"As you continue your adventure in {location}, you hear a commotion ahead. A merchant's cart has overturned, spilling exotic goods across the road. The merchant, a nervous gnome, is frantically trying to gather his wares while keeping an eye on the surrounding forest. 'Oh dear, oh dear!' he mutters. 'The wheel just snapped! And I swear I saw eyes watching from those trees!' As a {characterRace} {characterClass}, you're well-equipped to handle such situations, but you must choose your approach carefully.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Help the merchant gather his goods and repair the cart", NextNodeId = 26, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Investigate the forest for potential threats", NextNodeId = 27, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Offer to escort the merchant to safety", NextNodeId = 28, Effects = new Dictionary<string, string>() },
                        new Choice { Text = "Continue on your way, avoiding involvement", NextNodeId = 29, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }

        private List<AdventureScenario> GetDefaultScenarios()
        {
            return new List<AdventureScenario>
            {
                new AdventureScenario
                {
                    Description = "You find yourself at a crossroads, both literally and figuratively. The path ahead splits into three directions: one leading to a dark forest filled with ancient trees, another toward rolling hills dotted with ruins, and the third to a bustling town in the distance. The choice you make now will shape your adventure ahead.",
                    Choices = new List<Choice>
                    {
                        new Choice { Text = "Take the forest path", NextNodeId = 30, Effects = new Dictionary<string, string> { {"location_change", "Dark Forest"} } },
                        new Choice { Text = "Head toward the hills and ruins", NextNodeId = 31, Effects = new Dictionary<string, string> { {"location_change", "Ancient Hills"} } },
                        new Choice { Text = "Make your way to the town", NextNodeId = 32, Effects = new Dictionary<string, string> { {"location_change", "Riverside Town"} } },
                        new Choice { Text = "Rest here and consider your options", NextNodeId = 33, Effects = new Dictionary<string, string>() }
                    }
                }
            };
        }
    }

    public class AIRequest
    {
        public AdvancedPrompt? PromptData { get; set; }
        public string? RawPrompt { get; set; }
    }

    public class AdventureScenario
    {
        public string Description { get; set; } = string.Empty;
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }
}
