using Billing.Domain.Models;

namespace Billing.Web.Api.Features.LineItems.Update
{
    public sealed record Command(LineItem entity);
}