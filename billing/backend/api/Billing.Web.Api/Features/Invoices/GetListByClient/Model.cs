using FastEndpoints;
using FluentValidation;

namespace Billing.Web.Api.Features.Invoices.GetListByClient
{
    public sealed class Request
    {
        public int ClientId { get; set; }
    }

    public sealed class Response
    {
        public ICollection<InvoiceSummaryDTO> InvoiceSummaries { get; set; } = [];
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ClientId).NotEqual(0).WithMessage("Must provide valid Client Id.");
        }
    }

    public sealed class InvoiceSummaryDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}