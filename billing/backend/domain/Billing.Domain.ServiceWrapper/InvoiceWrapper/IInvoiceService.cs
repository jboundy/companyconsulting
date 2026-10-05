using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.InvoiceWrapper
{
    public interface IInvoiceService
    {
        Task<IReadOnlyList<(Invoice Invoice, Client? Client)>> GetUnPaidInvoices(CancellationToken ct);
    }
}