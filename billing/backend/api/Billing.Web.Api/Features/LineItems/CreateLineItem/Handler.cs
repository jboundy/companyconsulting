using Billing.Domain.ServiceWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.LineItems.CreateLineItem
{
    public class Handler(ILineItemService liService) : IWolverineHandler
    {
        ILineItemService _liService = liService;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _liService.CreateLineItemsAsync(command.entities, ct);
            return new Response { Result = result };
        }
    }
}