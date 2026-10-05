namespace Billing.Web.Api.Features.Clients.GetClientDetail
{
    public record Command(int id, CancellationToken ct);
}