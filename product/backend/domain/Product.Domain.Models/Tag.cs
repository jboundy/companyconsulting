using System.Text.Json.Serialization;

namespace Product.Domain.Models
{
    public class Tag
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}