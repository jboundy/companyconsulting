using System.Text.Json.Serialization;

namespace Product.Domain.Models
{
    public class Bundle
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("products")]
        public List<ProductRecord> Product { get; set; } = [];

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Optional bundle-specific pricing override or discount percentage
        /// </summary>
        [JsonPropertyName("discount_percentage")]
        public decimal? DiscountPercentage { get; set; }
    }
}