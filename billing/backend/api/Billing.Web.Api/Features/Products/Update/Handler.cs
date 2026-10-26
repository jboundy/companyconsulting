using Billing.Domain.ServiceWrapper.ProductWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Products.Update
{
    public sealed class Handler(IProductService service) : IWolverineHandler
    {
        IProductService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.UpdateProductAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}