using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetClientInvoices
{
    public class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(Command command)
        {
            var summaries = await _context.QueryRecords<Invoice, InvoiceSummaryDTO>(
                    inv => inv.ClientId == command.clientId,
                    inv => new InvoiceSummaryDTO
                    {
                        Id = inv.Id,
                        Amount = inv.Amount
                    },
                    command.ct);

            return new Response
            {
                InvoiceSummaries = [.. summaries]
            };
        }
    }
}