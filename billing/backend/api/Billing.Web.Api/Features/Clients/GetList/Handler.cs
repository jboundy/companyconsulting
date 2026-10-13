using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.GetList
{
    public sealed class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(CancellationToken ct)
        {
            var clients = await _context.QueryRecords<Client, ClientDTO>(
                   cl => new ClientDTO
                   {
                       Id = cl.Id,
                       Name = cl.Name,
                       Email = cl.Email,
                       PhoneNumber = cl.PhoneNumber
                   },
                   ct);

            return new Response
            {
                Clients = clients
            };
        }
    }
}