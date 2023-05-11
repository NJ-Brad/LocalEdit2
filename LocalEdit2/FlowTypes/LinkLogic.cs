using System.Text.Json.Serialization;

namespace LocalEdit2.FlowTypes
{
    public class LinkLogic
    {
        public string jumpToItemId { get; set; }
        public string id { get; set; }
        public string value { get; set; }
        [JsonPropertyName("type")]
        public string linkType { get; set; }

        public string asString { get { return ToString(); } }

        public override string ToString()
        {
            return $"{linkType} {value}";
        }
    }
}
