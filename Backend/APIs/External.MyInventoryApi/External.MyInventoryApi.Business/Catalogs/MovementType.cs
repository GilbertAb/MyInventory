using System.Text.Json.Serialization;

namespace External.MyInventoryApi.Business.Catalogs
{
    public class MovementType
    {
        public int Id { get; set; }

        [JsonPropertyName("MovementType")]
        public string Type { get; set; } = string.Empty;
    }
}
