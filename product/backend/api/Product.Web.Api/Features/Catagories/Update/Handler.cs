using Product.Domain.ServiceWrapper.CategoryWrapper;
using Wolverine;

namespace Product.Web.Api.Features.Categories.Update
{
    public sealed class Handler(ICategoryService service) : IWolverineHandler
    {
        ICategoryService _service = service;

        public async Task<Response> Handle(Command command, CancellationToken ct)
        {
            var result = await _service.UpdateCategoryAsync(command.entity, ct);
            return new Response { Result = result };
        }
    }
}