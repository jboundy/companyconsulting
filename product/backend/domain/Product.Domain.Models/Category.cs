using System.Text.Json.Serialization;

namespace Product.Domain.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("parent_category_id")]
        public int? ParentCategoryId { get; set; }

        [JsonPropertyName("parent_category")]
        public Category? ParentCategory { get; set; }

        [JsonPropertyName("sub_categories")]
        public ICollection<Category> SubCategories { get; set; } = [];

        [JsonPropertyName("products")]
        public ICollection<ProductRecord> Products { get; set; } = [];

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}