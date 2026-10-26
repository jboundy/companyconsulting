using Billing.Domain.Models;

namespace Billing.Web.Api.Features.Products.Update
{
    public sealed record Command(Product entity);
}