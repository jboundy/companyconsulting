using Billing.Domain.DataAccess;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.GetClientDetail
{
    public class Handler(IBillingContext context) : IWolverineHandler
    {
        IBillingContext _context = context;

        public async Task<Response> Handle(Command cmd)
        {
            var client = await _context.GetById<Domain.Models.Client>(cmd.id, cmd.ct) ?? throw new Exception("Client not found");
            var dto = new ClientDetailDTO
            {
                Id = client.Id,
                TaxId = client.TaxId,
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                BillingAddress = client.BillingAddress,
                MailingAddress = client.MailingAddress
            };

            return new Response
            {
                ClientDTO = dto
            };
        }
    }
}