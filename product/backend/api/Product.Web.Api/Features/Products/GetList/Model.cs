using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Web.Api.Features.Products.GetList
{
    public sealed class Response
    {
        public IReadOnlyList<ProductDTO> Products { get; set; } = [];
    }

    public sealed class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}