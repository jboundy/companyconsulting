using Product.Domain.Models;

namespace Product.Web.Api.Features.Categories.Create
{
    public sealed record Command(Category entity);
}