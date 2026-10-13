using Billing.Domain.ServiceWrapper.ClientWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.Update
{
    public sealed class Handler(IClientService service) : IWolverineHandler
    {
        IClientService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.UpdateClientAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}