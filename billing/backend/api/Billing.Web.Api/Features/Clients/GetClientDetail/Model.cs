using Billing.Domain.Models;
using FastEndpoints;

namespace Billing.Web.Api.Features.Clients.GetClientDetail
{
    public class Request
    {
        public int ClientId { get; set; }
    }

    public class Response
    {
        public required ClientDetailDTO ClientDTO { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public class ClientDetailDTO
    {
        public int Id { get; set; }
        public required string TaxId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string? PhoneNumber { get; set; }
        public required Address BillingAddress { get; set; }
        public required Address? MailingAddress { get; set; }

    }
}