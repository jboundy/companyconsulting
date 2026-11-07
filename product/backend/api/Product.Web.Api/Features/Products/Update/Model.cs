using Product.Domain.Models;
using FastEndpoints;

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
            // RuleFor(x => x.LineItemUpdate.InvoiceId).Equals(0).WithMessage("Collection contains invalid invoice id.");
            // RuleFor(x => x.LineItemUpdate.Quantity == 0).WithMessage("Collection contains invalid quantity.");
            // RuleFor(x => x.LineItemUpdate.ItemPrice == 0).WithMessage("Collection contains invalid item price.");
        }
    }

    public sealed class ProductUpdateDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public required string Description { get; set; }
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
                Description = r.ProductUpdate.Description,
            };
        }
    }
}