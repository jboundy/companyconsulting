using Billing.Domain.Models;

namespace Billing.Web.Api.Features.Products.Create
{
    public sealed record Command(Product entity);
}