using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetDetail
{
    public sealed class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var (invoice, client, contract) = (await _context.LeftJoinAsync<Invoice, Client, Contract, int, int>(
                    outerWhere: inv => inv.Id == command.invoiceId,
                    outer1KeySelector: inv => inv.ClientId,
                    inner1KeySelector: client => client.Id,
                    outer2KeySelector: inv => inv.ContractId,
                    inner2KeySelector: contract => contract.Id,
                    ct: ct
                )).FirstOrDefault();

            if (invoice is null || client is null || contract is null) throw new Exception("Invoice, Client or Contract not found");

            var invoiceDTO = new InvoiceDetailDTO
            {
                Id = invoice.Id,
                ClientName = client.Name,
                ContractName = contract.Name,
                Status = invoice.Status.ToString(),
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                PaidAt = invoice.PaidAt,
                Amount = invoice.Amount
            };

            return new Response
            {
                InvoiceDTO = invoiceDTO
            };
        }
    }
}