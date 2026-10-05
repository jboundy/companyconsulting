namespace Billing.Web.Api.Features.Invoices.GetClientInvoices
{
    public record Command(int clientId, CancellationToken ct);
}