using Product.Domain.Models;

namespace Product.Web.Api.Features.Products.Update
{
    public sealed record Command(ProductRecord entity);
}