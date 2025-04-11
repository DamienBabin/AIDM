// DnDAdventure.Core/models/Location.cs
namespace DnDAdventure.Core
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> ConnectedLocations { get; set; } = new();
        public List<Guid> NPCsPresent { get; set; } = new();
        public List<string> AvailableItems { get; set; } = new();
        public Dictionary<string, string> EnvironmentDetails { get; set; } = new();
    }
}