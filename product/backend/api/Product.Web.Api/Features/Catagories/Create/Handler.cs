using Product.Domain.ServiceWrapper.CategoryWrapper;
using Product.Domain.ServiceWrapper.ProductWrapper;
using Wolverine;

namespace Product.Web.Api.Features.Categories.Create
{
    public sealed class Handler(ICategoryService service) : IWolverineHandler
    {
        ICategoryService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.CreateCategoryAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}