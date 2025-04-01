// DnDAdventure.Core/Models/AdventureNode.cs
namespace DnDAdventure.Core.Models
{
    public class AdventureNode
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Choice> Choices { get; set; } = new();
        public Dictionary<string, string> Requirements { get; set; } = new();
    }

    public class Choice
    {
        public string Text { get; set; } = string.Empty;
        public int NextNodeId { get; set; }
        public Dictionary<string, string> Effects { get; set; } = new();
    }
}
