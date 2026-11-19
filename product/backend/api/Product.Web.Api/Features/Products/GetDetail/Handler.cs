using Product.Domain.DataAccess;
using Product.Domain.Models;
using Wolverine;

namespace Product.Web.Api.Features.Products.GetDetail
{
    public sealed class Handler(IProductContext context) : IWolverineHandler
    {
        IProductContext _context = context;

        public async Task<Response> Handle(Command cmd, CancellationToken ct)
        {
            var product = await _context.GetById<ProductRecord>(cmd.id, ct) ?? throw new Exception("Product not found");
            //TODO: need to build this object
            var dto = new ProductDetailDTO
            {

            };

            return new Response
            {
                ProductDetailDTO = dto
            };
        }
    }
}