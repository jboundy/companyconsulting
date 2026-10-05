using System.Text.Json.Serialization;

namespace Billing.Domain.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sku")]
        public required string Sku { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}