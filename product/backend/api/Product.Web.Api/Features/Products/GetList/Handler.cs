using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.Domain.DataAccess;
using Product.Domain.Models;
using Wolverine;

namespace Product.Web.Api.Features.Products.GetList
{
    public sealed class Handler(IProductContext context) : IWolverineHandler
    {
        IProductContext _context = context;

        public async Task<Response> Handle(CancellationToken ct)
        {
            var products = await _context.QueryRecords<ProductRecord, ProductDTO>(
               c => new ProductDTO
               {
                   Id = c.Id,
                   Name = c.Name,
                   IsActive = c.IsActive
               },
               ct);

            return new Response
            {
                Products = products
            };
        }
    }
}