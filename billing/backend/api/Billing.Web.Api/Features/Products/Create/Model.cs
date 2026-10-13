using FastEndpoints;
using FluentValidation;

namespace Billing.Web.Api.Features.Products.Create
{
    public sealed class Request
    {
        public required ProductDTO ProductDTO { get; set; }
    }

    public sealed class Response
    {

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
        public required string Sku { get; set; }

        public string? Description { get; set; }

    }
}