using Product.Domain.DataAccess;
using Product.Domain.Models;
using Wolverine;

namespace Product.Web.Api.Features.Categories.GetDetail
{
    public sealed class Handler(IProductContext context) : IWolverineHandler
    {
        IProductContext _context = context;

        public async Task<Response> Handle(Command cmd, CancellationToken ct)
        {
            var category = await _context.GetById<Category>(cmd.id, ct) ?? throw new Exception("Category not found");
            var dto = new CategoryDetailDTO
            {

            };

            return new Response
            {
                CategoryDetailDTO = dto
            };
        }
    }
}