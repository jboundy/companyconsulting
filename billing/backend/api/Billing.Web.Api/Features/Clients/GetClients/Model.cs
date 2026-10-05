using Billing.Domain.Models;
using FastEndpoints;

namespace Billing.Web.Api.Features.Clients.GetClients
{
    public class Request
    {
    }

    public class Response
    {
        public IReadOnlyList<ClientDTO> Clients { get; set; } = [];
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public class ClientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}