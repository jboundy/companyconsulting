namespace Billing.Web.Api.Features.Invoices.GetInvoiceDetail
{
    public record Command(int invoiceId, CancellationToken ct);
}