using Billing.Domain.Models;
using FastEndpoints;
using FluentValidation;

namespace Billing.Web.Api.Features.Invoices.GetInvoiceDetail
{
    public class Request
    {
        public int InvoiceId { get; set; }
    }

    public class Response
    {
        public required InvoiceDetailDTO InvoiceDTO { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.InvoiceId).NotEqual(0).WithMessage("Must provide valid Invoice Id.");
        }
    }

    public class InvoiceDetailDTO
    {
        public int Id { get; set; }
        public required string ClientName { get; set; }
        public required string ContractName { get; set; }
        public required string Status { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
    }
}