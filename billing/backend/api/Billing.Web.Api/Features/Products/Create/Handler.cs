using Billing.Domain.ServiceWrapper.ProductWrapper;
using Wolverine;

namespace Billing.Web.Api.Features.Products.Create
{
    public sealed class Handler(IProductService service) : IWolverineHandler
    {
        IProductService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.CreateProductAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}