using Billing.Domain.Models;

namespace Billing.Domain.ServiceWrapper.ProductWrapper
{
    public interface IProductService
    {
        Task<OperationResult> CreateProductAsync(Product entity, CancellationToken ct);
        Task<OperationResult> UpdateProductAsync(Product entity, CancellationToken ct);
    }
}