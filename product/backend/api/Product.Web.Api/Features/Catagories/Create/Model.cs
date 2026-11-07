using Product.Domain.Models;
using FastEndpoints;
using System.Text.Json.Serialization;

namespace Product.Web.Api.Features.Categories.Create
{
    public sealed class Request
    {
        public required CategoryDTO CategoryDTO { get; set; }
    }

    public sealed class Response
    {
        public required OperationResult Result { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public sealed class CategoryDTO
    {
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

    public class CreateCategoryMapper : RequestMapper<Request, Category>
    {
        public override Category ToEntity(Request r)
        {
            return new Category
            {
                Name = r.CategoryDTO.Name
            };
        }
    }

}