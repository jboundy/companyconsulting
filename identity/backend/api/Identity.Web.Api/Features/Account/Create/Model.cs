using FastEndpoints;
using Identity.Domain.Models;
using Identity.Domain.ServiceWrapper;

namespace Identity.Web.Api.Features.Account.Create
{
    public sealed class Request
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public sealed class Response
    {
        public OperationResult? Result { get; set; }
    }

    public sealed class CreateDomainUserMapper : RequestMapper<Request, DomainUser>
    {
        public override DomainUser ToEntity(Request r)
        {
            return new DomainUser
            {
                UserName = r.UserName,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber
            };
        }
    }
}