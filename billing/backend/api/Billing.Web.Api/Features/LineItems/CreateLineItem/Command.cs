using Billing.Domain.Models;

namespace Billing.Web.Api.Features.LineItems.CreateLineItem
{
    public record Command(List<LineItem> entities, CancellationToken ct);
}