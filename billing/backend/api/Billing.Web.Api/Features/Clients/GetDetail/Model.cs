using Billing.Domain.Models;
using FastEndpoints;

namespace Billing.Web.Api.Features.Clients.GetDetail
{
    public sealed class Request
    {
        public int ClientId { get; set; }
    }

    public sealed class Response
    {
        public required ClientDetailDTO ClientDTO { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public sealed class ClientDetailDTO
    {
        public int Id { get; set; }
        public required string TaxId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required Address BillingAddress { get; set; }
        public Address? MailingAddress { get; set; }

    }
}