using Product.Domain.Models;
using FastEndpoints;
using FluentValidation;

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
            RuleFor(x => x.ProductDTO.Sku).NotEmpty().WithMessage("Product must contain a sku number.");
        }
    }

    public sealed class ProductDTO
    {
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public required string Description { get; set; }
    }

    public class CreateProductMapper : RequestMapper<Request, ProductRecord>
    {
        public override ProductRecord ToEntity(Request r)
        {
            return new ProductRecord
            {
                Name = r.ProductDTO.Name,
                Sku = r.ProductDTO.Sku,
                Description = r.ProductDTO.Description,
            };
        }
    }
}