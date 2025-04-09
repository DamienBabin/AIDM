// DnDAdventure.Core/Models/AdventureNode.cs
namespace DnDAdventure.Core
{
    public class AdventureNode
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Choice> Choices { get; set; } = new();
        public Dictionary<string, string> Requirements { get; set; } = new();
        public List<NPCInteractionOption> NPCInteractions { get; set; } = new();
    }

    public class Choice
    {
        public string Text { get; set; } = string.Empty;
        public int NextNodeId { get; set; }
        public Dictionary<string, string> Effects { get; set; } = new();
    }

    public class NPCInteractionOption
    {
        public Guid NPCId { get; set; }
        public string NPCName { get; set; } = string.Empty;
        public string InteractionType { get; set; } = string.Empty; // "Talk", "Trade", "Quest"
        public string InteractionDescription { get; set; } = string.Empty;
        public Dictionary<string, string> Effects { get; set; } = new();
    }
}
