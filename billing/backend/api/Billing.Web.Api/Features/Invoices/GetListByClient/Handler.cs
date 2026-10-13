using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetListByClient
{
    public sealed class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var summaries = await _context.QueryRecords<Invoice, InvoiceSummaryDTO>(
                    inv => inv.ClientId == command.clientId,
                    inv => new InvoiceSummaryDTO
                    {
                        Id = inv.Id,
                        Amount = inv.Amount
                    },
                    ct);

            return new Response
            {
                InvoiceSummaries = [.. summaries]
            };
        }
    }
}