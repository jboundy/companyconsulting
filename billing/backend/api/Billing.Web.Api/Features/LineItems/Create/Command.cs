using Billing.Domain.Models;

namespace Billing.Web.Api.Features.LineItems.Create
{
    public sealed record Command(List<LineItem> entities);
}