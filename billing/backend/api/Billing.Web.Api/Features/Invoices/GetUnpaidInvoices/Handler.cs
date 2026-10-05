using Billing.Domain.ServiceWrapper.InvoiceWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetUnpaidInvoices
{
    public class Handler(IInvoiceService service) : IWolverineHandler
    {
        IInvoiceService _service = service;

        public async Task<Response> Handle(Command command)
        {
            var summaries = (await _service.GetUnPaidInvoices(command.ct)).Select(x =>
            {
                return new InvoiceSummaryDTO
                {
                    Id = x.Invoice.Id,
                    Amount = x.Invoice.Amount,
                    IssueDate = x.Invoice.IssueDate,
                    ClientName = x.Client != null ? x.Client.Name : "Unknown Client"
                };
            });

            return new Response
            {
                InvoiceSummaries = [.. summaries]
            };
        }
    }
}