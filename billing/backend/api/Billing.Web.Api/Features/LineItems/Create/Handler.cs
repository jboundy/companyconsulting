using Billing.Domain.ServiceWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.LineItems.Create
{
    public sealed class Handler(ILineItemService service) : IWolverineHandler
    {
        ILineItemService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.CreateLineItemsAsync(command.entities, ct);
            return new Response { Result = result };
        }
    }
}