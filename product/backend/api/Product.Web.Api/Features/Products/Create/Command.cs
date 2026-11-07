using Product.Domain.Models;

namespace Product.Web.Api.Features.Products.Create
{
    public sealed record Command(ProductRecord entity);
}