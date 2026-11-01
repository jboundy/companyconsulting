
using Product.Domain.Models;

namespace Product.Domain.ServiceWrapper.ProductWrapper
{
    public interface IProductService
    {
        Task<OperationResult> CreateProductAsync(ProductRecord entity, CancellationToken ct);
        Task<OperationResult> UpdateProductAsync(ProductRecord entity, CancellationToken ct);
    }
}