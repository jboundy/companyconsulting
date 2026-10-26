using System.Text.Json.Serialization;

namespace Product.Domain.Models
{
    public class ProductRecord
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sku")]
        public required string Sku { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("base_price")]
        public decimal BasePrice { get; set; }

        [JsonPropertyName("is_bundle")]
        public bool IsBundle { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category")]
        public Category? Category { get; set; }

        [JsonPropertyName("tags")]
        public ICollection<Tag> Tags { get; set; } = [];

        [JsonPropertyName("attributes")]
        public Dictionary<string, object> Attributes { get; set; } = [];

        [JsonPropertyName("bundle_items")]
        public ICollection<Bundle> BundleItems { get; set; } = [];

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}