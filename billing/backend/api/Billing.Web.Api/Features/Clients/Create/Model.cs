using Billing.Domain.Models;
using Billing.Domain.ServiceWrapper;
using Consulting.Encryption;
using FastEndpoints;
using FluentValidation;

namespace Billing.Web.Api.Features.Clients.Create
{
    public sealed class Request
    {
        public required ClientDTO ClientDTO { get; set; }
    }

    public sealed class Response
    {
        public required OperationResult Result { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ClientDTO.TaxId).NotEmpty().WithMessage("Record must contain Tax Id.");
            RuleFor(x => x.ClientDTO.Name).NotEmpty().WithMessage("Record must contain Name.");
            RuleFor(x => x.ClientDTO.Email).NotEmpty().WithMessage("Record must contain Email.");
            RuleFor(x => x.ClientDTO.BillingAddress).NotEmpty().WithMessage("Record must contain billing address.");
        }
    }

    public sealed class ClientDTO
    {
        public required string TaxId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required Address BillingAddress { get; set; }
        public Address? MailingAddress { get; set; }
        public bool IsBillingMailingMatch { get; set; }
    }

    public class CreateClientMapper : RequestMapper<Request, Client>
    {
        public override Client ToEntity(Request r)
        {
            var taxId = EncryptionWrapper.Encrypt(r.ClientDTO.TaxId);
            return new Client
            {
                TaxId = taxId,
                Name = r.ClientDTO.Name,
                Email = r.ClientDTO.Email,
                PhoneNumber = r.ClientDTO.PhoneNumber,
                BillingAddress = r.ClientDTO.BillingAddress,
                MailingAddress = r.ClientDTO.MailingAddress,
                IsBillingMailingMatch = r.ClientDTO.IsBillingMailingMatch
            };
        }
    }
}