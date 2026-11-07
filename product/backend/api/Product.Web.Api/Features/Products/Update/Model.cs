using Product.Domain.Models;
using FastEndpoints;
using System.Text.Json.Serialization;
using Billing.Domain.Taxes.Models;

namespace Product.Web.Api.Features.Products.Update
{
    public sealed class Request
    {
        public required ProductUpdateDTO ProductUpdate { get; set; }
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

    public sealed class ProductUpdateDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sku")]
        public required string Sku { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("base_price")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// The categorized tax type
        /// </summary>
        [JsonPropertyName("tax_code")]
        public required TaxCode TaxCode { get; set; }

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

    public class UpdateProductMapper : RequestMapper<Request, ProductRecord>
    {
        public override ProductRecord ToEntity(Request r)
        {
            return new ProductRecord
            {
                Id = r.ProductUpdate.Id,
                Name = r.ProductUpdate.Name,
                Sku = r.ProductUpdate.Sku,
                Description = r.ProductUpdate.Description ?? "",
                BasePrice = r.ProductUpdate.BasePrice,
                TaxCode = r.ProductUpdate.TaxCode,
                IsBundle = r.ProductUpdate.IsBundle,
                CategoryId = r.ProductUpdate.CategoryId,
                Tags = r.ProductUpdate.Tags,
                Attributes = r.ProductUpdate.Attributes,
                BundleItems = r.ProductUpdate.BundleItems,
                IsActive = r.ProductUpdate.IsActive
            };
        }
    }
}