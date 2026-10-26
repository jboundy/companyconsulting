using System.Text.Json.Serialization;

namespace Product.Domain.Models
{
    public class Bundle
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product")]
        public ProductRecord? Product { get; set; }

        [JsonPropertyName("child_product_id")]
        public int ChildProductId { get; set; }

        [JsonPropertyName("child_product")]
        public ProductRecord? ChildProduct { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Optional bundle-specific pricing override or discount percentage
        /// </summary>
        [JsonPropertyName("discount_percentage")]
        public decimal? DiscountPercentage { get; set; }
    }
}