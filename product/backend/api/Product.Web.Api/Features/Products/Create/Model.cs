using Product.Domain.Models;
using FastEndpoints;
using FluentValidation;
using System.Text.Json.Serialization;
using Billing.Domain.Taxes.Models;

namespace Product.Web.Api.Features.Products.Create
{
    public sealed class Request
    {
        public required ProductDTO ProductDTO { get; set; }
    }

    public sealed class Response
    {
        public required OperationResult Result { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ProductDTO).NotNull().WithMessage("Product must have details.");
            RuleFor(x => x.ProductDTO.Name).NotEmpty().WithMessage("Product must have a name.");
            RuleFor(x => x.ProductDTO.Sku).NotEmpty().WithMessage("Product must contain a sku number.");
            RuleFor(x => x.ProductDTO.BasePrice).NotEqual(0).WithMessage("Product must have a base price.");
            RuleFor(x => x.ProductDTO.TaxCode).NotNull().WithMessage("Product must have a tax code.");
            RuleFor(x => x.ProductDTO.CategoryId).NotNull().NotEqual(0).WithMessage("Product must have a valid category");
        }
    }

    public sealed class ProductDTO
    {
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

    public class CreateProductMapper : RequestMapper<Request, ProductRecord>
    {
        public override ProductRecord ToEntity(Request r)
        {
            return new ProductRecord
            {
                Sku = r.ProductDTO.Sku,
                Name = r.ProductDTO.Name,
                Description = r.ProductDTO.Description,
                BasePrice = r.ProductDTO.BasePrice,
                TaxCode = r.ProductDTO.TaxCode,
                IsBundle = r.ProductDTO.IsBundle,
                CategoryId = r.ProductDTO.CategoryId,
                Tags = r.ProductDTO.Tags,
                Attributes = r.ProductDTO.Attributes,
                BundleItems = r.ProductDTO.BundleItems,
                IsActive = true
            };
        }
    }
}