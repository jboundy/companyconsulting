using Product.Domain.DataAccess;
using Product.Domain.Models;
using Wolverine;

namespace Product.Web.Api.Features.Categories.GetList
{
    public sealed class Handler(IProductContext context) : IWolverineHandler
    {
        IProductContext _context = context;

        public async Task<Response> Handle(CancellationToken ct)
        {
            var categories = await _context.QueryRecords<Category, CategoryDTO>(
               c => new CategoryDTO
               {
                   Id = c.Id,
                   Name = c.Name,
                   Slug = c.Slug,
                   ParentCategoryId = c.ParentCategoryId,
                   IsActive = c.IsActive
               },
               ct);

            return new Response
            {
                Categories = categories
            };
        }
    }
}