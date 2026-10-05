using Billing.Domain.DataAccess;
using Billing.Domain.Models;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.GetClients
{
    public class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(Command command)
        {
            var clients = await _context.QueryRecords<Client, ClientDTO>(
                   cl => new ClientDTO
                   {
                       Id = cl.Id,
                       Name = cl.Name,
                       Email = cl.Email,
                       PhoneNumber = cl.PhoneNumber
                   },
                   command.ct);

            return new Response
            {
                Clients = clients
            };
        }
    }
}