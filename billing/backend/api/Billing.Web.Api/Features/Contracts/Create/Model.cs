using System.Text.Json.Serialization;
using Billing.Domain.Models;
using Billing.Domain.ServiceWrapper;
using FastEndpoints;
using FluentValidation;
using StatusCodes = Billing.Domain.Models.StatusCodes;

namespace Billing.Web.Api.Features.Contracts.Create
{
    public sealed class Request
    {
        public required Contract Contract { get; set; }
    }

    public sealed class Response
    {
        public required OperationResult Result { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Contract.ClientId).Empty().WithMessage("Must contain client id.");
            RuleFor(x => x.Contract.Name).Empty().WithMessage("Must contain name.");
            RuleFor(x => x.Contract.Status).Empty().WithMessage("Must have status.");
            RuleFor(x => x.Contract.StartDate).Empty().WithMessage("Must have start date.");
            RuleFor(x => x.Contract.EndDate).Empty().WithMessage("Must have end date.");
        }
    }

    public sealed class ContractDTO
    {
        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("status")]
        public StatusCodes Status { get; set; }

        [JsonPropertyName("start_date")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateOnly EndDate { get; set; }

        [JsonPropertyName("renew")]
        public bool Renew { get; set; }
    }

    public class CreateContractMapper : RequestMapper<Request, Contract>
    {
        public override Contract ToEntity(Request r)
        {
            return new Contract
            {
                ClientId = r.Contract.ClientId,
                Name = r.Contract.Name,
                Status = r.Contract.Status,
                StartDate = r.Contract.StartDate,
                EndDate = r.Contract.EndDate,
                Renew = r.Contract.Renew
            };
        }
    }
}