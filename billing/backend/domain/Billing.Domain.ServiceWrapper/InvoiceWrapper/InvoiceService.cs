using Billing.Domain.DataAccess;
using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.InvoiceWrapper
{
    public class InvoiceService(IBillingContext context) : IInvoiceService
    {
        public async Task<IReadOnlyList<(Invoice Invoice, Client? Client)>> GetUnPaidInvoices(CancellationToken ct)
        {
            return await context.LeftJoinAsync<Invoice, Client, int>(
                    inv => inv.Status == InvoiceStatus.Issued ||
                                        inv.Status == InvoiceStatus.Uncollectible,
                    inv => inv.ClientId,
                    client => client.Id,
                    ct
                );
        }
    }
}