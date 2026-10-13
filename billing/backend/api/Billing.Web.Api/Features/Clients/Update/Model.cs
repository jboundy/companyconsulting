using System.Text.Json.Serialization;
using Billing.Domain.Models;
using Billing.Domain.ServiceWrapper;
using FastEndpoints;

namespace Billing.Web.Api.Features.Clients.Update
{
    public sealed class Request
    {
        public required ClientUpdateDTO ClientUpdate { get; set; }
    }

    public sealed class Response
    {
        public required OperationResult Result { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            // RuleFor(x => x.LineItemUpdate.InvoiceId).Equals(0).WithMessage("Collection contains invalid invoice id.");
            // RuleFor(x => x.LineItemUpdate.Quantity == 0).WithMessage("Collection contains invalid quantity.");
            // RuleFor(x => x.LineItemUpdate.ItemPrice == 0).WithMessage("Collection contains invalid item price.");
        }
    }

    public sealed class ClientUpdateDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("taxId")]
        public required string TaxId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone_number")]
        public required string? PhoneNumber { get; set; }

        [JsonPropertyName("billing_address")]
        public required Address BillingAddress { get; set; }

        [JsonPropertyName("mailing_address")]
        public required Address? MailingAddress { get; set; }

        [JsonPropertyName("is_billing_mailing_match")]
        public bool IsBillingMailingMatch { get; set; }
    }

    public class UpdateClientMapper : RequestMapper<Request, Client>
    {
        public override Client ToEntity(Request r)
        {
            return new Client
            {
                Id = r.ClientUpdate.Id,
                TaxId = r.ClientUpdate.TaxId,
                Name = r.ClientUpdate.Name,
                Email = r.ClientUpdate.Email,
                PhoneNumber = r.ClientUpdate.PhoneNumber,
                BillingAddress = r.ClientUpdate.BillingAddress,
                MailingAddress = r.ClientUpdate.MailingAddress,
                IsBillingMailingMatch = r.ClientUpdate.IsBillingMailingMatch
            };
        }
    }
}