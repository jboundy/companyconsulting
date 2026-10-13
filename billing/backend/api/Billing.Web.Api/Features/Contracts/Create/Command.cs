using Billing.Domain.Models;

namespace Billing.Web.Api.Features.Contracts.Create
{
    public sealed record Command(Contract entity);
}