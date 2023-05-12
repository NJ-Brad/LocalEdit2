using System.Text.Json.Serialization;

namespace LocalEdit2.FlowTypes
{
    public class FlowItem
    {
        public string? id { get; set; } = "";
        [JsonPropertyName("type")]
        public string? itemType { get; set; }
        public string? title { get; set; } = "";
        public List<object> queryLogic { get; set; } = new();
        public List<object> flowEntryLogic { get; set; } = new();
        public List<FlowRelationship>? NextItems { get; set; } = new List<FlowRelationship>();
    }
}
