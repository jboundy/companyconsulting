namespace Billing.Web.Api.Features.Invoices.GetUnpaidInvoices
{
    public class Response
    {
        public ICollection<InvoiceSummaryDTO>? InvoiceSummaries { get; set; } = [];
    }

    public class InvoiceSummaryDTO
    {
        public int Id { get; set; }
        public required string ClientName { get; set; }
        public DateOnly IssueDate { get; set; }
        public decimal Amount { get; set; }
    }
}