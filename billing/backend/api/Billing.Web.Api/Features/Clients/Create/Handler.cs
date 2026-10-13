using Billing.Domain.ServiceWrapper.ClientWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.Create
{
    public sealed class Handler(IClientService clientService) : IWolverineHandler
    {
        IClientService _clientService = clientService;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _clientService.CreateClientAsync(command.client, ct);
            return new Response { Result = result };
        }
    }
}