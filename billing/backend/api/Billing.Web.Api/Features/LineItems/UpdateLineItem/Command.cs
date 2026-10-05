using Billing.Domain.Models;

namespace Billing.Web.Api.Features.LineItems.UpdateLineItem
{
    public record Command(LineItem entity, CancellationToken ct);
}