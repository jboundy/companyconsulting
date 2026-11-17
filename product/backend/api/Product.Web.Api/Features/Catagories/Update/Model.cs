using Product.Domain.Models;
using FastEndpoints;
using System.Text.Json.Serialization;

namespace Product.Web.Api.Features.Categories.Update
{
    public sealed class Request
    {
        public required CategoryUpdateDTO CategoryUpdate { get; set; }
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

    public sealed class CategoryUpdateDTO
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

    public class UpdateCategoryMapper : RequestMapper<Request, Category>
    {
        public override Category ToEntity(Request r)
        {
            return new Category
            {
                Id = r.CategoryUpdate.Id,
                Name = r.CategoryUpdate.Name,
                Slug = r.CategoryUpdate.Slug,
                ParentCategoryId = r.CategoryUpdate.ParentCategoryId,
                ParentCategory = r.CategoryUpdate.ParentCategory,
                SubCategories = r.CategoryUpdate.SubCategories,
                Products = r.CategoryUpdate.Products,
                IsActive = r.CategoryUpdate.IsActive
            };
        }
    }
}